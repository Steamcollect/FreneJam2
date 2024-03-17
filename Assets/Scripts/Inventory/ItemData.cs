using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea]public string itemDescription;

    public Sprite visual;

    public int healthPointGiven;
    public int attackPointGiven;
    public int defensePointGiven;

    public EquipmentType equipmentType;
    public ConsumableType consumableType;
}

[System.Serializable]
public enum EquipmentType
{
    Comsumable,
    Helmet,
    Plate,
    Feet,
    Weapon
}
[System.Serializable]
public enum ConsumableType
{
    HealthPotion,
    DefensePotion,
    AttackPotion
}