using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject[] heartsGO;
    public GameObject[] heartsShadowGO;

    public Image[] heartsImage;

    public void SetMaxHealth(int health)
    {
        for (int i = 0; i < heartsGO.Length; i++)
        {
            if(i + 1 > health)
            {
                heartsGO[i].SetActive(false);
                heartsShadowGO[i].SetActive(false);
            }
        }
    }

    public void SetHealthBarVisual(int health)
    {
        for (int i = 0; i < heartsImage.Length; i++)
        {
            if(i + 1 > health)
            {
                heartsImage[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                heartsImage[i].color = new Color(1, 1, 1, 1);
            }
        }
    }
}