using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    Patroling,
    Following,
    Jebaited
}

public class EnemyAI : MonoBehaviour
{
    
    private NavMeshAgent enemy;
    private GameObject player;
    private LineRenderer lineRenderer;
    public GameObject gameMangaer;

    public List<GameObject> globalBait;

    [Header("PathFinding INFO")]
    public int pathSize = 0;
    public Vector3[] destinations;
    public GameObject guards;
    public Vector3 baitDest;

    [Header("Enemy Stats")]
    public bool reachedEnd;
    public bool walking;
    public bool movingForward;
    public EnemyStates state;

    private AudioSource audioSource;
    public AudioClip caught;


    private void Awake()
    {
        gameMangaer = GameObject.FindGameObjectWithTag("GameController");
        audioSource = GetComponent<AudioSource>();
        movingForward = true;
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        GameObject[] guardList = GameObject.FindGameObjectsWithTag("Guards");
        for (int i = 0; i < guardList.Length; i++)
        {
            if (this.gameObject.name != guardList[i].gameObject.name)
            {
                guards = guardList[i];
            }
        }
        
        state = EnemyStates.Patroling;

        destinations = new Vector3[lineRenderer.positionCount];

        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i] = lineRenderer.GetPosition(i);
        }
    }
    
    void Update()
    {
        if(gameMangaer.GetComponent<GameManager>().victory || gameMangaer.GetComponent<GameManager>().gameOver)
        {
            audioSource.volume = 0f;
        }

        globalBait = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bait"));

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(Vector3.Distance(transform.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) < 0.2f) { reachedEnd = true; }
        if(Vector3.Distance(transform.position, lineRenderer.GetPosition(0)) < 0.2f) { reachedEnd = false; }


        switch (state)
        {
            case (EnemyStates.Patroling):
                GetComponent<FOV>().viewAngle = 90;
                if (movingForward)
                {
                    if (!reachedEnd)
                    {
                        walking = true;                        
                        var currDest = lineRenderer.GetPosition(pathSize);
                        if (pathSize != lineRenderer.positionCount - 1)
                        {
                            var nexDest = lineRenderer.GetPosition(pathSize + 1);
                        }

                        enemy.SetDestination(currDest);
                        if (Vector3.Distance(transform.position, currDest) < 0.2f) 
                        {
                            if (GenerateRandomNum() == 0)
                            {
                                StartCoroutine(LookAround(currDest));
                            }
                            pathSize++; 
                        }
                    }
                    else if (reachedEnd == guards.GetComponent<EnemyAI>().reachedEnd)
                    {
                        walking = true;
                        if (pathSize == lineRenderer.positionCount - 1)
                        {
                            movingForward = false;
                            break;
                        }
                    }
                    else
                    {
                        walking = false;
                        transform.Rotate(0, 1f, 0);
                    }
                }            
                else
                {
                    if (reachedEnd)
                    {
                        walking = true;
                        var currDest = lineRenderer.GetPosition(pathSize);
                        if (pathSize != 0)
                        {
                            var nexDest = lineRenderer.GetPosition(pathSize - 1);
                        }
                        enemy.SetDestination(currDest);
                        if (Vector3.Distance(transform.position, currDest) < 0.2f) 
                        {
                            if (GenerateRandomNum() == 0)
                            {
                                StartCoroutine(LookAround(currDest));
                            }
                            pathSize--; 
                        }
                    }
                    else if (reachedEnd == guards.GetComponent<EnemyAI>().reachedEnd)
                    {
                        walking = true;
                        if (pathSize == 0)
                        {
                            movingForward = true;
                            pathSize++;
                            break;
                        }
                    }
                    else
                    {
                        walking = true;
                        transform.Rotate(0, 1f, 0);
                    }
                }
                break;

            case (EnemyStates.Following):
                GetComponent<FOV>().viewAngle = 90;               
                if(audioSource.clip != caught)
                {
                    audioSource.clip = caught;
                    audioSource.Play();
                }
                guards.GetComponent<EnemyAI>().state = state;
                Vector3 dirToPlayer = transform.position - player.transform.position;
                Vector3 newPos = transform.position - dirToPlayer;
                enemy.SetDestination(newPos);
                break;

            case (EnemyStates.Jebaited):
                GetComponent<FOV>().viewAngle = 40;

                if(globalBait[0] != null)
                    baitDest = globalBait[0].transform.position;

                if (baitDest == null)
                {
                    state = EnemyStates.Patroling;
                }
                enemy.SetDestination(globalBait[0].transform.position);
                
                if(Vector3.Distance(transform.position, baitDest) < 1.5f)
                {
                    StartCoroutine(GetJebaited());
                }

                break;
        }
    }

    IEnumerator GetJebaited()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            state = EnemyStates.Patroling;
            yield break;
            
        }   
    }

    IEnumerator LookAround(Vector3 dest)
    {
        while (true)
        {
            enemy.isStopped = true;
            walking = false;
            for (int i = 0; i < 64; i++)
            {
                transform.Rotate(0, 5f, 0);
                yield return new WaitForSeconds(0.05f);
                if (state == EnemyStates.Following)
                    break;
            }            
            walking = true;
            enemy.isStopped = false;
            enemy.SetDestination(dest);
            yield break;
        }
    }

    public int GenerateRandomNum()
    {
        var random = Random.Range(0, 4);
        return random;
    }
}
