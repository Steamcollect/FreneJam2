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
    bool isInventoryOpon = false;

    public AudioClip[] openCloseInventorySound;
    [HideInInspector] public CharacterController playerController;

    // Add item panel
    ItemData item;
    public GameObject addItemPanelGO;
    public Image itemVisual;
    public TMP_Text itemNameTxt, itemDescriptionTxt;

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

        addItemPanelGO.SetActive(true);
        addItemPanelGO.transform.Bump(1.2f);

        itemVisual.sprite = item.visual;
        itemNameTxt.text = item.itemName;
        itemDescriptionTxt.text = item.itemDescription;
    }

    public void SkipItemButton()
    {
        addItemPanelGO.SetActive(false);
    }
    public void EquipItemButton()
    {
        switch (item.equipmentType)
        {
            case EquipmentType.Helmet:
                if(helmetData != null)
                {
                    playerController.attackPointBonnus -= helmetData.damageGiven;
                    playerController.defensePointGiven -= helmetData.defenseGiven;
                }
                helmetImage.sprite = item.visual;
                helmetData = item;
                break;

            case EquipmentType.Plate:
                if (plateData != null)
                {
                    playerController.attackPointBonnus -= plateData.damageGiven;
                    playerController.defensePointGiven -= plateData.defenseGiven;
                }
                plateImage.sprite = item.visual;
                plateData = item;
                break;

            case EquipmentType.Feet:
                if (feetData != null)
                {
                    playerController.attackPointBonnus -= feetData.damageGiven;
                    playerController.defensePointGiven -= feetData.defenseGiven;
                }
                feetImage.sprite = item.visual;
                feetData = item;
                break;

            case EquipmentType.Weapon:
                if (weaponData != null)
                {
                    playerController.attackPointBonnus -= weaponData.damageGiven;
                    playerController.defensePointGiven -= weaponData.defenseGiven;
                }
                weaponImage.sprite = item.visual;
                weaponData = item;
                break;
        }

        playerController.attackPointBonnus += item.damageGiven;
        playerController.defensesPointBonnus += item.defenseGiven;

        addItemPanelGO.SetActive(false);
    }
}