using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GUIController : MonoBehaviour
{
    public GameObject optionsMenu;
    public bool optionMenuOpened;

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        if(optionMenuOpened)
        {
            optionsMenu.SetActive(false);
            optionMenuOpened = false;
        }
        else
        {
            optionsMenu.SetActive(true);
            optionMenuOpened = true;
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
