using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    public TMP_Text scoreTxt;

    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scoreTxt.text = score + " pts";
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M)) AddScore(5);
    }

    public void AddScore(int scoreGiven)
    {
        score += scoreGiven;
        scoreTxt.transform.DOScale(1.2f, .08f).OnComplete(() => {
            scoreTxt.text = score + " pts";
            scoreTxt.transform.DOScale(1, .05f);
        });
    }
}
