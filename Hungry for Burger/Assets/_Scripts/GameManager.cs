using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject loseScreen;
    public GameObject winScreen;

    [Header("Texts")]
    //public Text text;

    [Header("Game States")]
    public bool lockedDown;
    public bool gameOver;
    public bool victory;

    [Header("References")]
    public GameObject guards;
    public GameObject lockDownGates;
    public GameObject lockDownUI;

    public bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

  
    void Update()
    {
        if(guards == null)
            guards = GameObject.FindGameObjectWithTag("Guards");

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
        {
            Pause();
        }

        if(guards.GetComponent<EnemyAI>().state == EnemyStates.Following && !lockedDown)
        {
            StartCoroutine(LockDownText(lockDownUI));
            StartCoroutine(LockDown()); 
        }

        if(lockedDown && !gameOver)
        {
            gameOver = true;
            Lose();
        }


    }

    public void Pause()
    {
        if(isPaused)
        {
            pauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
        else if(!isPaused)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void Win()
    {
        victory = true;
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    IEnumerator LockDownText(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(10);
        panel.SetActive(false);
    }

    IEnumerator LockDown()
    {
        while(true)
        {
            lockDownGates.gameObject.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime);
            yield return new WaitForSeconds(8);
            lockDownGates.gameObject.transform.Translate(new Vector3(0, 0, 0));
            lockedDown = true;
            yield break;
        }
    }
}
