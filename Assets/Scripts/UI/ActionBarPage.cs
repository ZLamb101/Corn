using ActionBar.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarPage : MonoBehaviour
{
    [SerializeField]
    private UIActionBarSlot itemPrefab;

/*    [SerializeField]
    private UIActionBarDescription SpellDescription;*/

    [SerializeField]
    private MouseFollower mouseFollower;

    List<UIActionBarSlot> listOfUIItems = new List<UIActionBarSlot>();

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested,
                OnItemActionRequested,
                OnStartDragging;

    public event Action<int, int> OnSwapItems;

 /*   [SerializeField]
    private ItemActionPanel actionPanel;*/


    private void Awake()
    {
        mouseFollower.Toggle(false);
        //itemDescription.ResetDescription();
    }

    public void InitializeActionBarUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
           // UIActionBarSlot newItem = Instantiate(itemPrefab, contentPanel);
            /*listOfUIItems.Add(newItem);
            newItem.OnItemClicked += HandleItemSelection;
            newItem.OnItemBeginDrag += HandleBeginDrag;
            newItem.OnItemDroppedOn += HandleSwap;
            newItem.OnItemEndDrag += HandleEndDrag;
            newItem.OnRightMouseBtnClick += HandleRightClick;*/
        }
    }

    private void HandleRightClick(UIActionBarSlot actionBarSlotUI)
    {
        int index = listOfUIItems.IndexOf(actionBarSlotUI);
        if (index == -1)
        {

            return;
        }
        OnItemActionRequested?.Invoke(index);
    }

    private void HandleEndDrag(UIActionBarSlot actionBarSlotUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIActionBarSlot actionBarSlotUI)
    {
        int index = listOfUIItems.IndexOf(actionBarSlotUI);
        if (index == -1)
        {
            ResetDraggedItem();
            return;
        }

        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        HandleItemSelection(actionBarSlotUI);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(UIActionBarSlot actionBarSlotUI)
    {
        int index = listOfUIItems.IndexOf(actionBarSlotUI);
        if (index == -1)
        {
            return;
        }
        currentlyDraggedItemIndex = index;
        HandleItemSelection(actionBarSlotUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quanitity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quanitity);
    }

    private void HandleItemSelection(UIActionBarSlot actionBarSlotUI)
    {
        int index = listOfUIItems.IndexOf(actionBarSlotUI);
        if (index == -1)
        {
            return;
        }
        OnDescriptionRequested?.Invoke(index);
    }
}
