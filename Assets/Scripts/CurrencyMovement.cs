using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    GameManager gameManager;

    public float moveSpeed = 25f; 

    public float detectionDistance = 10f; 

    private void Awake()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distance.ToString());

        if(distance <= detectionDistance)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.setScore(1);
            player.GetComponent<PlayerManager>().setMoneyAudioSource();
            Destroy(gameObject);
        }
    }
}
