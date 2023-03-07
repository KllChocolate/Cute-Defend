using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    public GameObject pauseUI;
public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainScrene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToOption()
    {
        SceneManager.LoadScene("Option");
    }
    public void PlayAgain() 
    {
        SceneManager.LoadScene("SceneGame");
        Time.timeScale = 1;
    }
    public void Resume()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
        }
        else
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
