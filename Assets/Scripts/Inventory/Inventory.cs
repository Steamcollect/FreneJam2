using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [Header("Inventory references")]
    public Image helmetImage, plateImage, feetImage, weaponImage;
    ItemData helmetData, plateData, feetData, weaponData;

    public Transform inventoryButtonGO;
    public GameObject inventoryPanelGO;
    [HideInInspector] public bool isInventoryOpon = false;

    public AudioClip[] openCloseInventorySound;
    [HideInInspector] public CharacterController playerController;

    [Header("Add item panel references")]
    ItemData item;
    public GameObject addItemPanelGO;
    public Image itemVisual;
    public TMP_Text itemNameTxt, itemDescriptionTxt;
    public TMP_Text takeItemButtonTxt;

    public TMP_Text convertToPointTxt;

    [Header("Current item panel references")]
    public GameObject currentItemPanelGO;
    public TMP_Text currentItemTypeTxt, currentItemNameTxt, currentItemDescriptionTxt;

    [HideInInspector]public LoopManager loopManager;

    public void InventoryButton()
    {
        if (openCloseInventorySound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, openCloseInventorySound[Random.Range(0, openCloseInventorySound.Length)]);

        isInventoryOpon = !isInventoryOpon;

        inventoryButtonGO.Bump(1.06f);
        inventoryPanelGO.SetActive(isInventoryOpon);
        inventoryPanelGO.transform.Bump(1.06f);
    }

    public void SetAddItemPanel(ItemData item)
    {
        this.item = item;

        convertToPointTxt.text = "Convert " + (item.attackPointGiven + item.healthPointGiven + item.defensePointGiven) + " pts";

        if (!isInventoryOpon) InventoryButton();

        if (item.equipmentType == EquipmentType.Comsumable) takeItemButtonTxt.text = "Add to inventory";
        else if (item.equipmentType == EquipmentType.Helmet && helmetData != null)
        {
            takeItemButtonTxt.text = "Replace '" + helmetData.itemName + "'";
            SetCurrentItemPanel(helmetData);
        }
        else if (item.equipmentType == EquipmentType.Plate && plateData != null)
        {
            takeItemButtonTxt.text = "Replace '" + plateData.itemName + "'";
            SetCurrentItemPanel(plateData);
        }
        else if (item.equipmentType == EquipmentType.Feet && feetData != null)
        {
            takeItemButtonTxt.text = "Replace '" + feetData.itemName + "'";
            SetCurrentItemPanel(feetData);
        }
        else if (item.equipmentType == EquipmentType.Weapon && weaponData != null)
        {
            takeItemButtonTxt.text = "Replace '" + weaponData.itemName + "'";
            SetCurrentItemPanel(weaponData);
        }
        else takeItemButtonTxt.text = "Equip";

        addItemPanelGO.SetActive(true);
        addItemPanelGO.transform.Bump(1.2f);

        itemVisual.sprite = item.visual;
        itemNameTxt.text = item.itemName;
        itemDescriptionTxt.text = item.itemDescription;
    }
    void SetCurrentItemPanel(ItemData item)
    {
        currentItemPanelGO.SetActive(true);

        currentItemTypeTxt.text = "Current " + item.equipmentType.ToString().ToLower();
        currentItemNameTxt.text = item.itemName;
        currentItemDescriptionTxt.text = item.itemDescription;
    }

    public void ConvertItemButton()
    {
        ScoreManager.instance.AddScore(item.attackPointGiven + item.healthPointGiven + item.defensePointGiven);
        addItemPanelGO.SetActive(false);
        StartCoroutine(loopManager.CreateNewEnemy());
    }
    public void EquipItemButton()
    {
        switch (item.equipmentType)
        {
            case EquipmentType.Comsumable:
                switch (item.consumableType)
                {
                    case ConsumableType.HealthPotion:
                        playerController.healPotionCount++;
                        break;
                    
                    case ConsumableType.DefensePotion:
                        playerController.defensePotionCount++;
                        break;
                    
                    case ConsumableType.AttackPotion:
                        playerController.attackPotionCount++;
                        break;
                }
                addItemPanelGO.SetActive(false);
                StartCoroutine(loopManager.CreateNewEnemy());
                return;

            case EquipmentType.Helmet:
                if(helmetData != null)
                {
                    playerController.equipmentHealthPoint -= helmetData.healthPointGiven;
                    playerController.equipmentAttackPoint -= helmetData.attackPointGiven;
                    playerController.equipmentDefensePoint -= helmetData.defensePointGiven;
                }
                helmetImage.sprite = item.visual;
                helmetImage.transform.Bump(1.2f);
                helmetData = item;
                break;

            case EquipmentType.Plate:
                if (plateData != null)
                {
                    playerController.equipmentHealthPoint -= plateData.healthPointGiven;
                    playerController.equipmentAttackPoint -= plateData.attackPointGiven;
                    playerController.equipmentDefensePoint -= plateData.defensePointGiven;
                }
                plateImage.sprite = item.visual;
                plateImage.transform.Bump(1.2f);
                plateData = item;
                break;

            case EquipmentType.Feet:
                if (feetData != null)
                {
                    playerController.equipmentHealthPoint -= feetData.healthPointGiven;
                    playerController.equipmentAttackPoint -= feetData.attackPointGiven;
                    playerController.equipmentDefensePoint -= feetData.defensePointGiven;
                }
                feetImage.sprite = item.visual;
                feetImage.transform.Bump(1.2f);
                feetData = item;
                break;

            case EquipmentType.Weapon:
                if (weaponData != null)
                {
                    playerController.equipmentHealthPoint -= weaponData.healthPointGiven;
                    playerController.equipmentAttackPoint -= weaponData.attackPointGiven;
                    playerController.equipmentDefensePoint -= weaponData.defensePointGiven;
                }
                weaponImage.sprite = item.visual;
                weaponImage.transform.Bump(1.2f);
                weaponData = item;
                break;
        }

        playerController.equipmentHealthPoint += item.healthPointGiven;
        playerController.equipmentAttackPoint += item.attackPointGiven;
        playerController.equipmentDefensePoint += item.defensePointGiven;

        playerController.SetStatBar();
        addItemPanelGO.SetActive(false);
        currentItemPanelGO.SetActive(false);

        StartCoroutine(loopManager.CreateNewEnemy());
    }
}