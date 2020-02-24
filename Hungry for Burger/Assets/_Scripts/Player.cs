using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public float speed;
    public float jumpForce;
    public float gravity;

    [Header("Layer Masks")]
    public LayerMask groundMask;
    public LayerMask pipeMask;

    [Header("Player Info")]
    public Transform foot;
    public float groundDistance = 0.5f;
    private Vector3 velocity;
    public Vector3 move;

    [Header("Player Actions")]
    public bool isGrounded;
    public bool isSneaking;

    [Header("Player Inventory")]
    public int baitAmount;
    public GameObject bait;
    public bool hasKey;

    [Header("Shooting")]
    public float shootForce;
    public GameObject projectile;
    public Transform shootPoint;

    [Header("Audio Clips")]
    public AudioClip openGate;

    private GameObject gameManager;
    private CharacterController charController;
    private AudioSource audioSource;

    public GameObject burgerUI;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        charController = GetComponent<CharacterController>();
    }


    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        if(move == new Vector3(0,0,0))
        {
            
        }
        
        
        isSneaking = Physics.CheckSphere(foot.position, groundDistance, pipeMask);        
        isGrounded = Physics.CheckSphere(foot.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameManager.GetComponent<GameManager>().isPaused)
        {
            Shoot();
        }

        StatusCheck();
        
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        charController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        charController.Move(velocity * Time.deltaTime);
    }

    IEnumerator TextDisplay(GameObject panel)
    {
        panel.SetActive(true);
        yield return new  WaitForSeconds(15);
        panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bait"))
        {
            StartCoroutine(TextDisplay(burgerUI));
            baitAmount += other.GetComponent<Bait>().amount;
            Destroy(other.gameObject);           
        }

        if(other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Burger"))
        {
            gameManager.GetComponent<GameManager>().Win();
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Cage") && hasKey)
        {
            audioSource.PlayOneShot(openGate, 0.5f);
            other.GetComponent<MeshCollider>().enabled = false;
            other.GetComponent<Animator>().SetBool("isOpen", true);           
        }
    }

    public void StatusCheck()
    {
        if (baitAmount > 0)
        {
            bait.gameObject.SetActive(true);
        }
        else
        {
            bait.gameObject.SetActive(false);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }

        if (isSneaking && velocity.y < 0)
        {
            velocity.y = gravity / 2;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || isSneaking))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        if (velocity.y > -0.5 && velocity.y < 0.5)
        {
            velocity.y += gravity / 5;
        }
    }
    public void Shoot()
    {
        if(baitAmount > 0)
        {
            var baitShot = Instantiate(projectile, shootPoint.position, shootPoint.parent.rotation);
            baitShot.GetComponent<Rigidbody>().AddForce(baitShot.transform.forward * shootForce); 
            baitAmount--;
        }
    }
   
}
