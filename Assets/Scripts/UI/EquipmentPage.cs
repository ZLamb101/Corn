using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentPage : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem itemPrefab;

    [SerializeField]
    private RectTransform ArmorPanel;
    [SerializeField]
    private RectTransform WeaponPanel;
    [SerializeField]
    private RectTransform RingPanel;
    [SerializeField]
    private RectTransform JeweleryPanel;

/*    [SerializeField]
    private UIInventoryDescription itemDescription;*/

    [SerializeField]
    private MouseFollower mouseFollower;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested,
                   OnItemActionRequested,
                   OnStartDragging;

    public event Action<int, int> OnSwapItems;

    [SerializeField]
    private ItemActionPanel actionPanel;
    private void Awake()
    {
        Hide();
        mouseFollower.Toggle(false);
    }

    public void InitializeEquipmentUI()
    {
        InitializeArmorPanel();
        InitializeWeaponPanel();
        InitializeRingPanel();
        InitializeJeweleryPanel();
    }

    private void InitializeJeweleryPanel()
    {
        for (int i = 0; i < 2; i++)
        {
            UIInventoryItem newItem = Instantiate(itemPrefab, JeweleryPanel);
            listOfUIItems.Add(newItem);
            IntializeItem(newItem);
        }
    }

    private void InitializeRingPanel()
    {
        for (int i = 0; i < 2; i++)
        {
            UIInventoryItem newItem = Instantiate(itemPrefab, RingPanel);
            listOfUIItems.Add(newItem);
            IntializeItem(newItem);
        }
    }

    private void InitializeWeaponPanel()
    {
        for (int i = 0; i < 2; i++)
        {
            UIInventoryItem newItem = Instantiate(itemPrefab, WeaponPanel);
            listOfUIItems.Add(newItem);
            IntializeItem(newItem);
        }
    }

    private void InitializeArmorPanel()
    {
        for (int i = 0; i < 6; i++)
        {
            UIInventoryItem newItem = Instantiate(itemPrefab, ArmorPanel);
            listOfUIItems.Add(newItem);
            IntializeItem(newItem);
        }
    }

    public void IntializeItem(UIInventoryItem newItem)
    {
        //newItem.OnItemClicked += HandleItemSelection;
        newItem.OnItemBeginDrag += HandleBeginDrag;
        newItem.OnItemDroppedOn += HandleSwap;
        newItem.OnItemEndDrag += HandleEndDrag;
        newItem.OnRightMouseBtnClick += HandleRightClick;
        //newItem.OnPointerEnter += HandlePointerEnter;
        //newItem.OnPointerExit += HandlePointerExit;
    }

    public void UpdateData(int itemIndex, Sprite itemImage)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage, 1);
        }
    }

    private void HandleRightClick(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {

            return;
        }
        OnItemActionRequested?.Invoke(index);
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            ResetDraggedItem();
            return;
        }

        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        //HandleItemSelection(inventoryItemUI);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        currentlyDraggedItemIndex = index;
       // HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, 1);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    internal void ResetAllItems()
    {
        foreach (var item in listOfUIItems)
        {
            item.ResetData();
        }
    }
}
