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

    public void SelectAction()
    {
        if (playerController.health.currentHealth <= 2)
        {
            //Debug.Log("Attack");
            lastAction = 1;
            controller.Attack();
        }
        else if (controller.health.currentHealth <= playerController.damage && controller.health.currentHealth < controller.health.maxHealth && controller.healPotionCount > 0)
        {
            //Debug.Log("Heal");
            lastAction = 4;
            controller.HealPotion();
        }
        else if(lastAction == 3)
        {
            //Debug.Log("attack");
            lastAction = 1;
            controller.Defend(0);
        }
        else
        {
            int[] tmpChoices = { 1, 2, 3, 4 };
            lastAction = Random.Range(1, tmpChoices.Length);

            if (lastAction == 1) controller.Attack();
            else if (lastAction == 2) controller.Defend(0);
            else if (lastAction == 3) controller.Focus(0);
            else if (lastAction == 4)
            {
                if (controller.attackPotionCount == 0 && controller.defensePotionCount == 0 && controller.healPotionCount == 0) SelectAction();

                List<int> potionUsable = new List<int>();

                if (controller.healPotionCount > 0 && controller.health.currentHealth < controller.health.maxHealth) potionUsable.Add(1);
                if (controller.defensePotionCount > 0) potionUsable.Add(2);
                if (controller.attackPotionCount > 0) potionUsable.Add(3);

                int tmp = potionUsable.GetRandom();
                if (tmp == 1) controller.HealPotion();
                else if (tmp == 2) controller.DefensePotion();
                else if (tmp == 3) controller.AttackPotion();
            }
        }
    }
}