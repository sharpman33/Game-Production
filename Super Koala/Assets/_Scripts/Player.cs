using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    up,
    down,
    left,
    right
}

public class Player : MonoBehaviour
{
    public float speed;
    public bool notMoving;
    [Space(30)]
    public float powerUpTimer;
    public bool poweredUp;
    [Space(30)]
    public GameObject gameManager;
    public Transform startPos;

    public float respawnWaitTime;

    private Animator animator;
    private Rigidbody2D rb;

    public int blinkcount;
    [Space(20)]
    private Vector3 moveDirection;
    public Directions currentDirection;
    public Directions directions;
    public Directions[] currentNode;
    [Space(20)]
    private AudioSource audioSource;
    public AudioClip powerUP;
    public AudioClip die;
    public AudioClip kill;
    public AudioClip munching;
    public AudioClip enemyRespawn;

    void Start()
    {
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindWithTag("GameController");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        SetupGame();
        RespawnPlayer();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            directions = Directions.up;
            if (notMoving)
            {
                for (int i = 0; i < currentNode.Length; i++)
                {
                    if (currentNode[i] == directions)
                        DirectionCheck(directions);
                }
                return;
            }

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directions = Directions.left;
            if (notMoving)
            {
                for (int i = 0; i < currentNode.Length; i++)
                {
                    if (currentNode[i] == directions)
                        DirectionCheck(directions);
                }
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            directions = Directions.down;
            if (notMoving)
            {
                for (int i = 0; i < currentNode.Length; i++)
                {
                    if (currentNode[i] == directions)
                        DirectionCheck(directions);
                }
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            directions = Directions.right;
            if (notMoving)
            {
                for (int i = 0; i < currentNode.Length; i++)
                {
                    if (currentNode[i] == directions)
                        DirectionCheck(directions);
                }
                return;
            }
        }

        if (moveDirection == new Vector3(speed, 0, 0) && directions == Directions.left)
        {
            DirectionCheck(directions);
        }
        else if (moveDirection == new Vector3(-speed, 0, 0) && directions == Directions.right)
        {
            DirectionCheck(directions);
        }
        else if (moveDirection == new Vector3(0, speed, 0) && directions == Directions.down)
        {
            DirectionCheck(directions);
        }
        else if (moveDirection == new Vector3(0, -speed, 0) && directions == Directions.up)
        {
            DirectionCheck(directions);
        }
        transform.Translate(moveDirection.x * Time.deltaTime, moveDirection.y * Time.deltaTime, moveDirection.z);

    }


    public void SetupGame()
    {
        directions = Directions.left;
        DirectionCheck(directions);
    }

    public void DirectionCheck(Directions dir)
    {
        switch (dir)
        {
            case Directions.up:
                moveDirection = new Vector3(0, speed, 0);
                currentDirection = Directions.up;
                animator.SetInteger("DirectionState", 1);
                notMoving = false;
                break;
            case Directions.down:
                moveDirection = new Vector3(0, -speed, 0);
                currentDirection = Directions.down;
                animator.SetInteger("DirectionState", 2);
                notMoving = false;
                break;
            case Directions.right:
                moveDirection = new Vector3(speed, 0, 0);
                currentDirection = Directions.right;
                animator.SetInteger("DirectionState", 4);
                notMoving = false;
                break;
            default:
                moveDirection = new Vector3(-speed, 0, 0);
                currentDirection = Directions.left;
                animator.SetInteger("DirectionState", 3);
                notMoving = false;
                break;
        }
    }

    public void StopMovingPlayer()
    {
        moveDirection = new Vector3(0, 0, 0);
    }

    public void RespawnPlayer()
    {
        StartCoroutine(PlayerRespawn());
    }

    IEnumerator PlayerPowerUp()
    {
        while(true)
        {
            poweredUp = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            yield return new WaitForSeconds(powerUpTimer);
            var flashing = true;
            while(flashing)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                flashing = false;
            }
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            poweredUp = false;
            yield break;
        }
    }

    IEnumerator PlayerRespawn()
    {
        while(true)
        {
            var enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].transform.position = enemyList[i].GetComponent<EnemyAI>().spawnPos.position;
                enemyList[i].GetComponent<EnemyAI>().RespawnEnemy();
            }
            transform.position = startPos.position;
            directions = Directions.left;
            DirectionCheck(directions);
            StopMovingPlayer();
            var flashing = true;
            while (flashing)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                flashing = false;
            }
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(respawnWaitTime/2);
            directions = Directions.left;
            DirectionCheck(directions);
            yield break;        
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("node") || other.CompareTag("PlayerNode"))
        {
            transform.position = other.gameObject.transform.position;
            currentNode = new Directions[other.GetComponent<Node>().nodeDirections.Length];

            for (int i = 0; i < other.GetComponent<Node>().nodeDirections.Length; i++)
            {
                currentNode[i] = other.GetComponent<Node>().nodeDirections[i];
            }

            for (int i = 0; i < other.GetComponent<Node>().nodeDirections.Length; i++)
            {
                if (other.GetComponent<Node>().nodeDirections[i] == directions)
                {
                    DirectionCheck(directions);
                    return;
                }
            }

            for (int i = 0; i < other.GetComponent<Node>().nodeDirections.Length; i++)
            {
                if(other.GetComponent<Node>().nodeDirections[i] == currentDirection)
                {
                    DirectionCheck(currentDirection);
                    return;
                }
            }
            
            if (other.GetComponent<Node>().cornerNode)
            {
                moveDirection = new Vector3(0, 0, 0);
                notMoving = true;
            }
        }

        if(other.CompareTag("PowerUp"))
        {
            audioSource.PlayOneShot(powerUP, 0.9f);
            Destroy(other.gameObject);
            if (!poweredUp)
            {               
                StartCoroutine(PlayerPowerUp());
            }
            else
            {
                StopAllCoroutines();
                poweredUp = false;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                StartCoroutine(PlayerPowerUp());
            }
            
        }

        if(other.CompareTag("Portal"))
        {
            transform.position = other.GetComponent<Portal>().endPoint.position;
        }

        if (other.CompareTag("pellete"))
        {
            //audioSource.PlayOneShot(munching, 0.1f);
            gameManager.GetComponent<GameManager>().AddScore(other.GetComponent<PickUps>().worthPoint);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            if(poweredUp)
            {
                audioSource.PlayOneShot(kill, 0.7f);
                audioSource.PlayOneShot(enemyRespawn, 0.3f);
                other.GetComponent<EnemyAI>().RespawnEnemy();
                gameManager.GetComponent<GameManager>().AddScore(other.GetComponent<EnemyAI>().worthPoints);
            }
            else
            {
                Debug.Log("LoseList");
                audioSource.PlayOneShot(die, 0.7f);
                gameManager.GetComponent<GameManager>().LoseLife();
                RespawnPlayer();
            }           
        }
    }
}
