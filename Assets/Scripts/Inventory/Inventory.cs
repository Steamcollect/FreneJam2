using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    // Inventory references
    public Image helmetImage, plateImage, feetImage, weaponImage;
    ItemData helmetData, plateData, feetData, weaponData;

    public Transform inventoryButtonGO;
    public GameObject inventoryPanelGO;
    [HideInInspector] public bool isInventoryOpon = false;

    public AudioClip[] openCloseInventorySound;
    [HideInInspector] public CharacterController playerController;

    // Add item panel
    ItemData item;
    public GameObject addItemPanelGO;
    public Image itemVisual;
    public TMP_Text itemNameTxt, itemDescriptionTxt;
    public TMP_Text takeItemButtonTxt;

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

        if (!isInventoryOpon) InventoryButton();

        if (item.equipmentType == EquipmentType.Helmet && helmetData != null) takeItemButtonTxt.text = "Replace '" + helmetData.name + "'";
        else if (item.equipmentType == EquipmentType.Plate && plateData != null) takeItemButtonTxt.text = "Replace '" + plateData.name + "'";
        else if (item.equipmentType == EquipmentType.Feet && feetData != null) takeItemButtonTxt.text = "Replace '" + feetData.name + "'";
        else if (item.equipmentType == EquipmentType.Weapon && weaponData != null) takeItemButtonTxt.text = "Replace '" + weaponData.name + "'";
        else takeItemButtonTxt.text = "Equip";

        addItemPanelGO.SetActive(true);
        addItemPanelGO.transform.Bump(1.2f);

        itemVisual.sprite = item.visual;
        itemNameTxt.text = item.itemName;
        itemDescriptionTxt.text = item.itemDescription;
    }

    public void SkipItemButton()
    {
        addItemPanelGO.SetActive(false);
        StartCoroutine(loopManager.CreateNewEnemy());
    }
    public void EquipItemButton()
    {
        switch (item.equipmentType)
        {
            case EquipmentType.Helmet:
                if(helmetData != null)
                {
                    playerController.equipmentAttackPoint -= helmetData.damageGiven;
                    playerController.equipmentDefensePoint -= helmetData.defenseGiven;
                    playerController.equipmentHealthPointGiven -= helmetData.healthPointGiven;
                    playerController.equipmentAttackPointGiven -= helmetData.attackPointGiven;
                    playerController.equipmentDefensePointGiven -= helmetData.defensePointGiven;
                }
                helmetImage.sprite = item.visual;
                helmetImage.transform.Bump(1.2f);
                helmetData = item;
                break;

            case EquipmentType.Plate:
                if (plateData != null)
                {
                    playerController.equipmentAttackPoint -= plateData.damageGiven;
                    playerController.equipmentDefensePoint -= plateData.defenseGiven;
                    playerController.equipmentHealthPointGiven -= plateData.healthPointGiven;
                    playerController.equipmentAttackPointGiven -= plateData.attackPointGiven;
                    playerController.equipmentDefensePointGiven -= plateData.defensePointGiven;
                }
                plateImage.sprite = item.visual;
                plateImage.transform.Bump(1.2f);
                plateData = item;
                break;

            case EquipmentType.Feet:
                if (feetData != null)
                {
                    playerController.equipmentAttackPoint -= feetData.damageGiven;
                    playerController.equipmentDefensePoint -= feetData.defenseGiven;
                    playerController.equipmentHealthPointGiven -= feetData.healthPointGiven;
                    playerController.equipmentAttackPointGiven -= feetData.attackPointGiven;
                    playerController.equipmentDefensePointGiven -= feetData.defensePointGiven;
                }
                feetImage.sprite = item.visual;
                feetImage.transform.Bump(1.2f);
                feetData = item;
                break;

            case EquipmentType.Weapon:
                if (weaponData != null)
                {
                    playerController.equipmentAttackPoint -= weaponData.damageGiven;
                    playerController.equipmentDefensePoint -= weaponData.defenseGiven;
                    playerController.equipmentHealthPointGiven -= weaponData.healthPointGiven;
                    playerController.equipmentAttackPointGiven -= weaponData.attackPointGiven;
                    playerController.equipmentDefensePointGiven -= weaponData.defensePointGiven;
                }
                weaponImage.sprite = item.visual;
                weaponImage.transform.Bump(1.2f);
                weaponData = item;
                break;
        }

        playerController.equipmentAttackPoint += item.damageGiven;
        playerController.equipmentDefensePoint += item.defenseGiven;
        playerController.equipmentHealthPointGiven += item.healthPointGiven;
        playerController.equipmentAttackPointGiven += item.attackPointGiven;
        playerController.equipmentDefensePointGiven += item.defensePointGiven;

        playerController.SetStatBar();

        addItemPanelGO.SetActive(false);

        StartCoroutine(loopManager.CreateNewEnemy());
    }
}