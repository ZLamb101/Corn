using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUIController : MonoBehaviour
{
    [SerializeField]
    private EquipmentPage equipmentUI;

    [SerializeField]
    private EquipmentSO equipmentData;

    public List<EquipmentItem> initialItems = new List<EquipmentItem>();

    private void Start()
    {
        PrepareEquipmentUI();
        PrepareEquipmentData();
    }

    private void PrepareEquipmentData()
    {
        equipmentData.Initialize();
        equipmentData.OnInventoryChanged += UpdateEquipmentUI;

        foreach (EquipmentItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;

            equipmentData.EquipItem(item);
        }
    }

    private void UpdateEquipmentUI(Dictionary<int, EquipmentItem> inventoryState)
    {
        equipmentUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            equipmentUI.UpdateData(item.Key, item.Value.item.Icon);
        }
    }

    private void PrepareEquipmentUI()
    {
        equipmentUI.InitializeEquipmentUI();

        //equipmentUI.OnDescriptionRequested += HandleDescriptionRequest;
        equipmentUI.OnSwapItems += HandleSwapItems;
        equipmentUI.OnStartDragging += HandleDragging;
        equipmentUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleItemActionRequest(int itemIndex)
    {
        EquipmentItem inventoryItem = equipmentData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;

        equipmentData.RemoveItem(itemIndex);

    }


    private void HandleDragging(int itemIndex)
    {
        EquipmentItem inventoryItem = equipmentData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        equipmentUI.CreateDraggedItem(inventoryItem.item.Icon);
    }



    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        equipmentData.SwapItems(itemIndex_1, itemIndex_2);
    }

    public void SetWeapon(EquippableItemSO equippableItem, List<ItemParameter> itemState)
    {
        if (equippableItem != null)
        {

            equipmentData.EquipItem(equippableItem, itemState);

        }

        //this.equipmentData = equippableItem;
        //this.itemCurrentState = new List<ItemParameter>(itemState);
       // ModifyParameters();
    }


    public void ToggleEquipmentDisplay()
    {
        Debug.Log("Toggling Equipment Display");
        if (equipmentUI.isActiveAndEnabled)
        {
            equipmentUI.Hide();
        }
        else
        {
            equipmentUI.Show();
            foreach (var item in equipmentData.GetCurrentInventoryState())
            {
                equipmentUI.UpdateData(item.Key, item.Value.item.Icon);
            }
        }
    }
}
