using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Animator settingsAnimator;
    bool isSettingsOpen;

    GameStateManager gameStateManager;

    private void Awake()
    {
        gameStateManager = FindFirstObjectByType<GameStateManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseButton();
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseButton()
    {
        if (isSettingsOpen)
        {
            gameStateManager.ResumeGameState();
            isSettingsOpen = false;
        }
        else
        {
            gameStateManager.PauseGameState();
            isSettingsOpen = true;
        }
        settingsAnimator.SetBool("IsOpen", isSettingsOpen);
    }
}