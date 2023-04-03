using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuManager
{
    public static bool PAUSED {get; private set;} = false;
    public GameObject pausePanel;

    private void PauseGame()
    {
        pausePanel.transform.position = gameObject.transform.position;
        Time.timeScale = 0f;

        AudioManager.Singleton.PauseAllSounds();

        PAUSED = true;
    }

    public void ResumeGame()
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        Time.timeScale = 1f;
        if (optionsPanel.activeSelf)
            OptionsButton();
        pausePanel.transform.position = gameObject.transform.position + new Vector3(0f, (40 * (gameObject.transform.position.y + 1)), 0f);

        AudioManager.Singleton.ResumeAllSounds();

        PAUSED = false;
    }

    public void BackToMenu()
    {
        if(PAUSED)
        {
            ResumeGame();
        }
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        switch(Input.GetKeyUp(KeyCode.Escape)) //Esse escape hardcoded é para ser momentâneo
        {
            case true:
                if(PAUSED)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
                break;

            default:
                break;
        }
    }
}