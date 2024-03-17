using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class Presets
{
    public static void ActiveInBump(this Transform t)
    {
        t.DOScale(1.2f, .35f).OnComplete(() => {
            t.DOScale(1, .07f);
        });
    }
    public static void DesactiveInBump(this Transform t)
    {
        t.DOScale(1.2f, .08f).OnComplete(() => {
            t.DOScale(0, .15f);
        });
    }
    public static void Bump(this Transform t, float bumpScale)
    {
        t.DOScale(bumpScale, .08f).OnComplete(() => {
            t.DOScale(1, .07f);
        });
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    public static T GetRandom<T>(this T[] list)
    {
        return list[Random.Range(0, list.Length)];
    }

    public static float GetRandomBetween(this Vector2 vector2)
    {
        return Random.Range(vector2.x, vector2.y);
    }
}