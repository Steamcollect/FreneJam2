using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsBar : MonoBehaviour
{
    public GameObject healthGO, shieldGO, attackGO;
    public TMP_Text healthTxt, shieldTxt, AttackTxt, equipmentHealthPoint, equipmentShieldPoint, equipmentAttackPoint;

    public void SetHealthVisual(int health, int maxHealth, int equipmentHealth)
    {
        healthTxt.text = health.ToString();
        if(equipmentHealth > 0) equipmentHealthPoint.text = "+" + equipmentHealth.ToString();

        if (health >= maxHealth) healthTxt.color = new Color(1, .9f, .7f, 1);
        else healthTxt.color = new Color(1, 1, 1, 1);
        if (health >= maxHealth + equipmentHealth) equipmentHealthPoint.color = new Color(1, .9f, .7f, 1);
        else equipmentHealthPoint.color = new Color(1, 1, 1, 1);

        healthGO.transform.Bump(1.2f);
    }
    public void SetShieldVisual(int shield, int equipmentShield)
    {
        if (shield + equipmentShield <= 0 && shieldTxt.text != "0") shieldGO.transform.DesactiveInBump();
        else if (shield > 0)
        {
            shieldGO.transform.Bump(1.2f);
        }
        shieldTxt.text = shield.ToString();
        equipmentShieldPoint.text = "+" + equipmentShield.ToString();
    }
    
    public void SetAttackVisual(int attack, int equipmentAttack)
    {
        if (attack  + equipmentAttack<= 0 && AttackTxt.text != "+0") attackGO.transform.DesactiveInBump();
        else if(attack > 0)
        {
            attackGO.transform.Bump(1.2f);
        }
        AttackTxt.text = "+" + attack.ToString();
        equipmentAttackPoint.text = "+" + equipmentAttack.ToString();
    }
}