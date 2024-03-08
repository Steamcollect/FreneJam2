using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
    public CharacterController controller, playerController;

    int lastAction = 0;

    /* 1 = attack
     * 2 = defend
     * 3 = focus
     * 4 = heal
    */

    public int SelectAction()
    {
        if (playerController.health.currentHealth <= 2)
        {
            //Debug.Log("Attack");
            lastAction = 1;
            return lastAction;
        }
        else if (controller.health.currentHealth <= playerController.damage)
        {
            //Debug.Log("Heal");
            lastAction = 4;
            return lastAction;
        }
        else if(lastAction == 3)
        {
            //Debug.Log("attack");
            lastAction = 1;
            return lastAction;
        }
        else
        {
            int[] tmp = { 1, 2, 3, 4 };
            lastAction = Random.Range(1, tmp.Length);
            if (lastAction == 4) lastAction = Random.Range(1, tmp.Length - 1);
            //Debug.Log("Random : "+ lastAction);
            return lastAction;
        }
    }
}