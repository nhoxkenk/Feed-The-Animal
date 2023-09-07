using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    GameManager gameManager;

    public HealthBar healthBar;
    public GameObject[] projectile;
    GameObject tempProjectile;
    GameObject target;

    private float horizontalInput;
    private float verticalInput;
    //private float timerDelay = 1.5f;
    private Dictionary<ProjectileType, float> timerDelay = new Dictionary<ProjectileType, float>();
    private Dictionary<ProjectileType, float> timers = new Dictionary<ProjectileType, float>();


    public int maxHealth = 5;
    public int currentHealth;

    private AudioSource playerAudio;
    public AudioClip moneySound; 
    public AudioClip hurtSound;
    public AudioClip shooterSound;
    public AudioClip boomSound;

    public bool enableDynamite = false;

    public void setCurrentHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;
    }

    public int returnCurrentHealth()
    {
        return currentHealth;
    }

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void Update()
    {
        if(!gameManager.gameOver)
        {
            inputManager.HandleAllInputs();

            horizontalInput = inputManager.horizontalInput;
            verticalInput = inputManager.verticalInput;

            this.target = calculateNearestAnimal();
            FireBullet(ProjectileType.Peashooter);
            //FireBullet(ProjectileType.Dynamite);
            if(enableDynamite == true) 
            { FireBullet(ProjectileType.Dynamite); }
        }
        else
        {
            inputManager.onDeath();
        }
        
    }

    private void FixedUpdate()
    {
        if(gameManager.gameOver)
        {
            StopAllCoroutines();
            
        }
        playerLocomotion.HandleAllMovement();

    }

    private void Start()
    {
        timerDelay[ProjectileType.Peashooter] = 1.5f;
        timerDelay[ProjectileType.Dynamite] = 4.5f;
        currentHealth = maxHealth;
        healthBar.setMaxHealth(currentHealth);
        timers[ProjectileType.Peashooter] = 0f;
        timers[ProjectileType.Dynamite] = 0f;
        //StartCoroutine(AutoFireCoroutine(ProjectileType.Peashooter));
        //StartCoroutine(AutoFireCoroutine(ProjectileType.Dynamite));
    }

    public void setMoneyAudioSource()
    {
        playerAudio.PlayOneShot(moneySound, 1f);
    }

    public void setHurtAudioSource()
    {
        playerAudio.PlayOneShot(hurtSound, 1f);
    }

    public void setBoomAudioSource()
    {
        playerAudio.PlayOneShot(boomSound, 1f);
    }

    GameObject calculateNearestAnimal()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;

        if (enemyList.Length > 0)
        {
            float nearestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemyList)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        return nearestEnemy;
    }

    IEnumerator AutoFireCoroutine(ProjectileType type)
    {

        while (true)
        {
            switch (type)
            {
                case ProjectileType.Peashooter:

                    if (this.target != null)
                    {
                        FireProjectile(this.target.transform);
                    }

                    yield return new WaitForSeconds(1.5f);
                    break;

                case ProjectileType.Dynamite:

                    setDynamite();

                    yield return new WaitForSeconds(5);
                    break;
            }
            
        }
    }

    void FireBullet(ProjectileType type)
    {
        if (timers.ContainsKey(type))
        {
            timers[type] += Time.deltaTime;

            switch (type)
            {
                case ProjectileType.Peashooter:
                    if (timers[type] >= timerDelay[type] && this.target != null)
                    {
                        FireProjectile(this.target.transform);
                        timers[type] = 0f; // Đặt lại timer sau khi bắn đạn
                    }
                    break;

                case ProjectileType.Dynamite:
                    if (timers[type] >= timerDelay[type] * 3)
                    {
                        setDynamite();
                        timers[type] = 0; // Đặt lại timer sau khi thiết lập Dynamite
                    }
                    break;
            }
        }
        
    }

    public void ModifyFireRate(float time)
    {
        Debug.Log("Fire Rate");
        if (timerDelay[ProjectileType.Peashooter] > 0.6f)
        {
            timerDelay[ProjectileType.Peashooter] *= time;
        }
        
    }

    public void ModifyCooldown(float time)
    {
        Debug.Log("Cool Down");
        if (timerDelay[ProjectileType.Dynamite] > 2f)
        {
            timerDelay[ProjectileType.Dynamite] *= time;
        }

    }

    public void EnableDynamite()
    {
        Debug.Log("Dynamite");
        enableDynamite = true;
        Debug.Log(enableDynamite);
    }

    public void ExploseMultiEnemy()
    {
        GameManager.Instance.multiHit = false;
    }

    public void RunFaster()
    {
        playerLocomotion.movementSpeed *= 1.2f;
    }

    public void AddMoreHealth(int i)
    {
        currentHealth += i;
        GameManager.Instance.updateHealthCount();
        healthBar.setHealth(currentHealth);
    }

    public void ModifyBaseDame(int dame)
    {
        Debug.Log("Base Dame");
        GameManager.Instance.hitCount -= dame;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            enemyBehaviour.setHealth(2);
        }
    }

    Vector3 offsetVector(float yValue)
    {
        Vector3 offset;

        if (horizontalInput == 0 && verticalInput == 0)
        {
            offset = -transform.forward * 2;
        }
        else
        {
            offset = new Vector3(-horizontalInput, yValue, -verticalInput);
        }
        return offset; 
    }

    private void FireProjectile(Transform target)
    {
        Vector3 offset =  (target.transform.position - gameObject.transform.position).normalized;

        tempProjectile = Instantiate(projectile[0], transform.position + offset, Quaternion.identity);
        playerAudio.PlayOneShot(shooterSound, 1f);
        tempProjectile.GetComponent<ProjectileBehaviour>().Fire(target);

    }


    void setDynamite()
    {
        Vector3 offset = offsetVector(0.08741013f);

        Instantiate(projectile[1], transform.position + offset, Quaternion.identity);
    }
}
