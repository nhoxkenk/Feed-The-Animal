using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum ProjectileType
{
    Peashooter,
    Dynamite
}

public class ProjectileBehaviour : MonoBehaviour
{

    private Transform target;
    private float speed = 15.0f;
    private bool homing = false;
    private PlayerManager playerManager;

    public ParticleSystem dynamiteParticle;
    public ProjectileType projectileType;

    private float aliveTimer = 3.0f;

    public void Fire(Transform target)
    {
        this.target = target;
        homing = true;
        Destroy(gameObject, aliveTimer);
    }

    private void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(projectileType == ProjectileType.Peashooter)
        {
            FireAwayProjectile();
        }
        
    }

    void FireAwayProjectile()
    {
        if (homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - gameObject.transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(projectileType == ProjectileType.Peashooter && other != null)
        {
            if (target != null)
            {
                if (other.gameObject.CompareTag(this.target.tag))
                {
                    Destroy(gameObject);
                    //Destroy(other.gameObject);
                    other.GetComponent<EnemyBehaviour>().OnHit();
                }
            }
        }else if (projectileType == ProjectileType.Dynamite)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                playerManager.setBoomAudioSource();
                Destroy(gameObject);
                //Destroy(other.gameObject);
                Instantiate(dynamiteParticle, transform.position + Vector3.up * 2f, transform.rotation);               
            }
        }
        
    }

    private void OnDestroy()
    {
        homing = false;
    }
}
