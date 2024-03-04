using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
    public int SelectAction()
    {
        return (Random.Range(0, 4));
    }
}