using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyDeath : MonoBehaviour
{

    //Manage the currency when the enemy health equal 0.
    public GameObject currencyPrefabs;
    public float bouncyForce;

    private EnemyBehaviour enemyBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyBehaviour.isDestroyed)
        {
            Invoke("spawnCurrency",0);
            Destroy(gameObject);
        }
    }

    int calculatedCurrencyNumber()
    {
        return Random.Range(1, 3) * enemyBehaviour.returnHealth();
    }

    Vector3 offsetVector()
    {
        int x = Random.Range(0, 3);
        int z = Random.Range(0, 3);
        return new Vector3(x, 0, z);
    }

    void spawnCurrency()
    {
        int amount = calculatedCurrencyNumber();
        for(int i = 0; i < amount; i++)
        {
            bouncyForce = Random.Range(0, 2.5f);
            Instantiate(currencyPrefabs, transform.position + offsetVector(), Quaternion.identity).GetComponent<Rigidbody>().AddForce(Vector3.up * bouncyForce, ForceMode.Impulse);
        }
        
    }
}
