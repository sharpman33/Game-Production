using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States
{
    following,
    patrolling
}
public class EnemyAI : MonoBehaviour
{
    public float speed;

    public Directions directions;
    public Directions startDir;

    public bool respawning;
    public float respawnWaitTime;
    public Transform spawnPos;

    public int worthPoints;
    public States state;
    private Vector3 moveDirection;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        RespawnEnemy();
        state = States.patrolling;
    }

    private void Update()
    {
        transform.Translate(moveDirection.x * Time.deltaTime, moveDirection.y * Time.deltaTime, moveDirection.z);
        if(!respawning)
            DirectionCheck();
    }

    public void DirectionCheck()
    {
        switch (directions)
        {
            case Directions.up:
                moveDirection = new Vector3(0, speed, 0);
                break;
            case Directions.down:
                moveDirection = new Vector3(0, -speed, 0);
                break;
            case Directions.right:
                moveDirection = new Vector3(speed, 0, 0);
                break;
            default:
                moveDirection = new Vector3(-speed, 0, 0);
                break;

        }
    }

    public void StopMovingEnemyAI()
    {
        moveDirection = new Vector3(0, 0, 0);
    }

    public void RespawnEnemy()
    {
        StartCoroutine(EnemyRespawn());
    }

    IEnumerator EnemyRespawn()
    {
        while (true)
        {
            respawning = true;
            transform.position = spawnPos.position;
            directions = startDir;
            StopMovingEnemyAI();
            yield return new WaitForSeconds(respawnWaitTime);
            DirectionCheck();
            respawning = false;
            yield break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("node") || other.CompareTag("EnemyNode"))
        {
            var nextDir = other.GetComponent<Node>().nodeDirections;
            var randInt = Random.Range(0, other.GetComponent<Node>().nodeDirections.Length);
            switch (randInt)
            {
                case 0:
                    directions = nextDir[0];
                    DirectionCheck();
                    this.transform.position = other.gameObject.transform.position;
                    return;
                case 1:
                    directions = nextDir[1];
                    DirectionCheck();
                    this.transform.position = other.gameObject.transform.position;
                    return;
                case 2:
                    directions = nextDir[2];
                    DirectionCheck();
                    this.transform.position = other.gameObject.transform.position;
                    return;
                default:
                    directions = nextDir[3];
                    DirectionCheck();
                    this.transform.position = other.gameObject.transform.position;
                    return;
            }
            
        }
    }
}
