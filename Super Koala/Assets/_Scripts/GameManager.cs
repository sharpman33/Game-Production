using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public Text highscoreText;
    public Text scoreText;
    public GameObject[] livesIndicator;

    public bool win;
    public bool gameOver;

    public int currLevel;
    public GameObject winScreen;
    public Text winScore;
    public Text winHighscore;

    public GameObject loseScreen;
    public Text loseScore;
    public Text loseHighscore;

    public static GameManager instance;
    public GameObject highscoreKeeper;

    public int score;
    public int lives;
    public int[] highscore;
    public float timer;

    public AudioSource audioSource;
    public AudioClip ac_Win;
    public AudioClip ac_Lose;

    public List<GameObject> pelletes;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        if (highscoreKeeper == null)
        {
            highscoreKeeper = GameObject.FindGameObjectWithTag("HighscoreKeeper");
        }
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1f;

        currLevel = 1;
        lives = 3;
        highscore = new int[3];

        for (int i = 0; i < highscore.Length; i++)
        {
            highscore[i] = highscoreKeeper.GetComponent<HighscoreKeeper>().highscores[i];
        }
        CalculateHighScores();
        PrintHighScores(highscoreText);
       
    }
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameOver();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ProceedToNextLevel();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            WinGame();
        }
        */

        pelletes = new List<GameObject>(GameObject.FindGameObjectsWithTag("pellete"));
        timer += Time.deltaTime;
        scoreText.text = "Score: " + score.ToString();

        if(pelletes.Count < 1 && currLevel == 1)
        {
            ProceedToNextLevel();
        }
        else if(pelletes.Count < 1 && currLevel == 2)
        {
            if(!win)
                WinGame();
        }

        LivesCheck();
    }

    public void PrintHighScores(Text highscoreTexts)
    {
        highscoreTexts.text = "1. " + highscore[0].ToString() + " \n" +
                            "2. " + highscore[1].ToString() + " \n" +
                            "3. " + highscore[2].ToString();
    }
    
    public void CalculateHighScores()
    {
        if(score > highscore[2]) // 3
        {
            if(score > highscore[1]) //2
            {
                if (score > highscore[0]) //1
                {
                    highscore[2] = highscore[1];
                    highscore[1] = highscore[0];
                    highscore[0] = score;
                    return;
                }
                else
                {
                    highscore[2] = highscore[1];
                    highscore[1] = score;
                    
                }
                
            }
            else
            {
                highscore[2] = score;
            }
        }
        else
        {
            return;
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public void Reset()
    {
        currLevel = 1;
        score = 0;
        lives = 3;
        gameOver = false;
        win = false;
        PrintHighScores(highscoreText);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void WinGame()
    {
        audioSource.PlayOneShot(ac_Win, 0.7f);
        win = true;
        if (timer < 1000)
        {
            score += (1000 - (int)timer);
        }       
        winScore.text = "Score: " + score.ToString();        
        CalculateHighScores();
        PrintHighScores(winHighscore);
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        audioSource.PlayOneShot(ac_Lose, 0.7f);
        gameOver = true;
        loseScore.text = "Score: " + score.ToString();
        CalculateHighScores();
        PrintHighScores(loseHighscore);
        loseScreen.SetActive(true);        
        Time.timeScale = 0f;
    }

    public void LoseLife()
    {
        lives--;
    }

    public void ProceedToNextLevel()
    {
        currLevel++;
        lives = 3;
        SceneManager.LoadScene("Level_2");
    }

    public void LivesCheck()
    {
        if(lives == 3)
        {
            for (int i = 0; i < livesIndicator.Length; i++)
            {
                livesIndicator[i].gameObject.SetActive(true);
            }
        }
        else if(lives == 2)
        {
            livesIndicator[0].gameObject.SetActive(true);
            livesIndicator[1].gameObject.SetActive(true);
            livesIndicator[2].gameObject.SetActive(false);
        }
        else if (lives == 1)
        {
            livesIndicator[0].gameObject.SetActive(true);
            livesIndicator[1].gameObject.SetActive(false);
            livesIndicator[2].gameObject.SetActive(false);
        }
        else if (lives <= 0)
        {
            livesIndicator[0].gameObject.SetActive(false);
            livesIndicator[1].gameObject.SetActive(false);
            livesIndicator[2].gameObject.SetActive(false);
            if (!gameOver)
                GameOver();
        }
    }
}
