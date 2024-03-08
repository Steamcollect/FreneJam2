using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    // Inventory references
    public Image helmetImage, plateImage, feetImage, weaponImage;
    public ItemData helmetData, plateData, feetData, weaponData;

    public Transform inventoryButtonGO;
    public GameObject inventoryPanelGO;
    bool isInventoryOpon = false;

    public AudioClip[] openCloseInventorySound;
    [HideInInspector] public CharacterController playerController;

    // Add item panel
    public ItemData item;
    public GameObject addItemPanelGO;
    public Image itemVisual;
    public TMP_Text itemNameTxt, itemDescriptionTxt;

    private void Start()
    {
        EquipItemButton();
    }

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
                    playerController.equipmentAttackPoint -= helmetData.damageGiven;
                    playerController.equipmentDefensePoint -= helmetData.defenseGiven;
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
                }
                weaponImage.sprite = item.visual;
                weaponImage.transform.Bump(1.2f);
                weaponData = item;
                break;
        }

        playerController.equipmentAttackPoint += item.damageGiven;
        playerController.equipmentDefensePoint += item.defenseGiven;

        addItemPanelGO.SetActive(false);
    }
}