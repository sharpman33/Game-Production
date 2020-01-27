using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private GameObject gameManager;
    public GameObject highscoreKeeper;
    public GameObject creditsWindows;
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        if (highscoreKeeper == null)
        {
            highscoreKeeper = GameObject.FindGameObjectWithTag("HighscoreKeeper");
        }
    }

    public void showCredits()
    {
        creditsWindows.SetActive(true);
    }
    public void hideCredits()
    {
        creditsWindows.SetActive(false);
    }

    public void LoadMyScene()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void Restart()
    {
        for (int i = 0; i < gameManager.GetComponent<GameManager>().highscore.Length; i++)
        {
            highscoreKeeper.GetComponent<HighscoreKeeper>().highscores[i] = gameManager.GetComponent<GameManager>().highscore[i];
        }
        Destroy(gameManager.GetComponent<GameManager>().canvas);
        Destroy(gameManager);
        gameManager.GetComponent<GameManager>().Reset();
        SceneManager.LoadScene("Level_1");
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quited");
    }

    public void Donate()
    {
        Application.OpenURL("https://lp.panda.org/bushfire-appeal");
    }
    public void MainMenu()
    {
        for (int i = 0; i < gameManager.GetComponent<GameManager>().highscore.Length; i++)
        {
            highscoreKeeper.GetComponent<HighscoreKeeper>().highscores[i] = gameManager.GetComponent<GameManager>().highscore[i];
        }
        Destroy(gameManager.GetComponent<GameManager>().canvas);
        Destroy(gameManager);
        gameManager.GetComponent<GameManager>().Reset();
        SceneManager.LoadScene("MainMenu");
    }
}
