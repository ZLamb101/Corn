using ActionBar.Model;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarUIController : MonoBehaviour
{
    [SerializeField]
    private ActionBarPage actionBarUI;

    [SerializeField]
    private ActionBarSO actionBarData;

    public List<ActionSlot> initialSpells = new List<ActionSlot>();

    private void Start()
    {
        PrepareActionBarUI();
        PrepareActionBarData();
    }

    private void PrepareActionBarData()
    {
        actionBarData.Initialize();
       // actionBarData.OnActionBarChanged += UpdateActionBarUI;

        foreach (ActionSlot item in initialSpells)
        {
            if (item.IsEmpty)
                continue;

           // actionBarData.EquipItem(item);
        }
    }

    private void UpdateActionBarUI(Dictionary<int, InventoryItem> inventoryState)
    {
        if (actionBarUI == null)
        {
            Debug.LogError("Inventory UI is null");
            return;
        }
        //actionBarUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            //actionBarUI.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity);
        }
    }

    private void PrepareActionBarUI()
    {
        actionBarUI.InitializeActionBarUI(actionBarData.Size);
        actionBarUI.OnDescriptionRequested += HandleDescriptionRequest;
        actionBarUI.OnSwapItems += HandleSwapItems;
        actionBarUI.OnStartDragging += HandleDragging;
        actionBarUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleItemActionRequest(int itemIndex)
    {


    }

    private void HandleDragging(int itemIndex)
    {
        ActionSlot inventoryItem = actionBarData.GetSpellSlotAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        //actionBarUI.CreateDraggedItem(inventoryItem.item.Icon, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        //actionBarData.SwapItems(itemIndex_1, itemIndex_2);
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        ActionSlot inventoryItem = actionBarData.GetSpellSlotAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            //actionBarUI.ResetSelection();
            return;
        }
        SpellSO item = inventoryItem.item;
      /*  string description = PrepareDescription(inventoryItem);
        actionBarUI.UpdateDescription(itemIndex, item.Icon, item.name, description);*/
    }
}
