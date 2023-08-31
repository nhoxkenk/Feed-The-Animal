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
        currentHealth = health;
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
                    isDestroyed = true;
                    break;
            }
            
        }
    }

    public int returnHealth()
    {
        return health;
    }

    IEnumerator RemoveWhiteMaterialAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);

            childRenderer.material = originMaterial;

        isHit = false;
    }
}
