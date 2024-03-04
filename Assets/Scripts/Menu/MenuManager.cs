using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator settingsAnimator;
    bool isSettingsOpen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSettingsOpen) SettingButton();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingButton()
    {
        isSettingsOpen = !isSettingsOpen;
        settingsAnimator.SetBool("IsOpen", isSettingsOpen);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}