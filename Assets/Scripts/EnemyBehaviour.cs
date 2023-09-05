using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Material originMaterial;
    public Material targetMaterial;
    public SkinnedMeshRenderer childRenderer;
    public ParticleSystem particle;
    ParticleSystem[] tempParrticle;

    private bool isHit = false;
    public bool isDestroyed = false;    

    [SerializeField] int health;
    int currentHealth;

    private void Start()
    {
        currentHealth = health * GameManager.Instance.hitCount;
    }

    void Update()
    {
        if (isHit)
        {
            childRenderer.material = targetMaterial;

            StartCoroutine(RemoveWhiteMaterialAfterDelay());      

        }

    }

    public void OnHit()
    {
        isHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Projectile"))
        {
            ProjectileType currentType = other.gameObject.GetComponent<ProjectileBehaviour>().projectileType;
            switch (currentType)
            {
                case ProjectileType.Peashooter:
                    currentHealth--;
                    if (currentHealth == 0)
                    {
                        isDestroyed = true;
                        Instantiate(particle, transform.position + Vector3.up * 2f, transform.rotation);
                        //Destroy(gameObject);          
                    }
                    break;
                case ProjectileType.Dynamite:
                    //Debug.Log("Explosion");
                    Debug.Log(GameManager.Instance.multiHit);
                    if (GameManager.Instance.multiHit)
                    {
                        isDestroyed = true;
                        
                    }
                    else
                    {
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach (GameObject enemy in enemies)
                        {
                            float distance = Vector3.Distance(transform.position, enemy.transform.position);
                            if (distance < GameManager.Instance.explosionRange)
                            {
                                enemy.GetComponent<EnemyBehaviour>().isDestroyed = true;
                            }
                        }
                    }
                    break;
            }
            
        }
    }

    public int returnHealth()
    {
        return health;
    }

    public void setHealth(int health)
    {
        this.health /= health;
        //Debug.Log(this.health);
    }

    IEnumerator RemoveWhiteMaterialAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);

            childRenderer.material = originMaterial;

        isHit = false;
    }
}
