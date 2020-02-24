using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseControlLook : MonoBehaviour
{
    public GameObject slider;
    public GameObject gameManager;

    public float mouseSensitivity = 100f;
    public float mouseX, mouseY;

    public Transform playerMesh;

    public float rotation = 0f;

    void Start()
    {

    }

    void Update()
    {
        mouseSensitivity = slider.GetComponent<Slider>().value;
        if (gameManager.GetComponent<GameManager>().isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(gameManager.GetComponent<GameManager>().gameOver || gameManager.GetComponent<GameManager>().victory)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotation += mouseY;
        rotation = Mathf.Clamp(rotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        playerMesh.Rotate(Vector3.up, mouseX);
    }
}
