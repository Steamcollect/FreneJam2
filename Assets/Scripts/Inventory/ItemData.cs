using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea]public string itemDescription;

    public Sprite visual;

    public int defenseGiven;
    public int damageGiven;
    public int healthPointGiven;
    public int attackPointGiven;
    public int defensePointGiven;

    public EquipmentType equipmentType;
}

[System.Serializable]
public enum EquipmentType
{
    Helmet,
    Plate,
    Feet,
    Weapon
}