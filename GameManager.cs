using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseUI;

    private void Awake()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }
}
