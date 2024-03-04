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

    public List<ActionSlot> initialSpells = new List<ActionSlot>(7);

    private void Start()
    {
        PrepareActionBarUI();
        PrepareActionBarData();
    }

    private void PrepareActionBarData()
    {
        actionBarData.Initialize();
        //actionBarData.OnActionBarChanged += UpdateActionBarUI;

        foreach (ActionSlot skill in initialSpells)
        {
            if (skill.IsEmpty)
                continue;

           actionBarData.AddSkill(skill);
        }

    }

    private void UpdateActionBarUI(Dictionary<int, ActionSlot> actionBarState)
    {
        if (actionBarUI == null)
        {
            Debug.LogError("actionBarState UI is null");
            return;
        }
        //actionBarUI.ResetAllItems();
        foreach (var item in actionBarState)
        {
            actionBarUI.UpdateData(item.Key, item.Value.skill.Icon);
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
        SpellSO item = inventoryItem.skill;
      /*  string description = PrepareDescription(inventoryItem);
        actionBarUI.UpdateDescription(itemIndex, item.Icon, item.name, description);*/
    }
}
