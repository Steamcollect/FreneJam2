using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsBar : MonoBehaviour
{
    public GameObject healthGO, shieldGO, attackGO;
    public TMP_Text healthTxt, shieldTxt, AttackTxt;

    public void SetHealthVisual(int health, int maxHealth)
    {
        healthTxt.text = health.ToString();

        if (health >= maxHealth) healthTxt.color = new Color(1, .9f, .7f, 1);
        else healthTxt.color = new Color(1, 1, 1, 1);

        healthGO.transform.Bump(1.2f);
    }
    public void SetShieldVisual(int shield)
    {
        if (shield <= 0 && shieldTxt.text != "0") shieldGO.transform.DesactiveInBump();
        else if (shield > 0)
        {
            shieldGO.transform.Bump(1.2f);
        }
        shieldTxt.text = shield.ToString();
    }
    
    public void SetAttackVisual(int attack)
    {
        if (attack <= 0 && AttackTxt.text != "+0") attackGO.transform.DesactiveInBump();
        else if(attack > 0)
        {
            attackGO.transform.Bump(1.2f);
        }
        AttackTxt.text = "+" + attack.ToString();
    }
}