using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreKeeper : MonoBehaviour
{
    public static HighscoreKeeper instance;
    public GameObject gameManager;

    public int[] highscores = new int[3];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        gameManager = GameObject.FindGameObjectWithTag("GameController");

    }
}
