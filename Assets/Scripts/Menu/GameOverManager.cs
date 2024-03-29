using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject deathPanelGO;
    public TMP_Text scoreTxt, waveCountTxt, enemysKilledTxt, damagedInflictedTxt, damageReceivedTxt, damagedBlockedTxt, lifeRecorveryTxt;

    public void SetTextInfos(int score, int waveCount, int enemysKilled, int damagedInflicted, int damagedReceived, int damagedBlocked, int lifeRecorvery)
    {
        scoreTxt.text = score.ToString();
        waveCountTxt.text = waveCount.ToString();
        enemysKilledTxt.text = enemysKilled.ToString();
        damagedInflictedTxt.text = damagedInflicted.ToString();
        damageReceivedTxt.text = damagedReceived.ToString();
        damagedBlockedTxt.text = damagedBlocked.ToString();
        lifeRecorveryTxt.text = lifeRecorvery.ToString();
    }

    public void OpenDeathPanel()
    {
        deathPanelGO.SetActive(true);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ReloadButton()
    {
        SceneManager.LoadScene("Game");
    }
}