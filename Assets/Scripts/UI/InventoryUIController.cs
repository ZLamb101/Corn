using Inventory.UI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField]
        private InventoryPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioClip audioSource;

        private void Start()
        {
            PrepareInventoryUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryChanged += UpdateInventoryUI;

            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) 
                        continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            if(inventoryUI == null)
            {
                // TODO : Probably figure out why its being set to NULL D:
                Debug.LogError("InventoryUI is not set");
                return;
            }
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity);
            }
        }

        private void PrepareInventoryUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                // Add a way to not delete Equipable Gear on equip
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
                
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
                //audioSource.PlayOneShot(itemAction.actionSFX);
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            //audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {

            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                // Add a way to not delete Equipable Gear on equip
                if (itemAction.PerformAction(gameObject, inventoryItem.itemState))
                {
                    IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
                    if (destroyableItem != null)
                    {
                        inventoryData.RemoveItem(itemIndex, 1);
                    }
                }
                if(inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if(inventoryItem.IsEmpty) 
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.Icon, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.Icon, item.name, description);
        }

        public string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for(int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append(inventoryItem.itemState[i].itemParameter.ParameterName);
                sb.Append(" : ");
                sb.Append(inventoryItem.itemState[i].value);
                sb.Append(" / ");
                sb.Append(inventoryItem.item.DefaultParametersList[i].value);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void ToggleInventoryDisplay()
        {
            Debug.Log("Toggling Inventory Display");
            if (inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Hide();
            }
            else
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity);
                }
            }
        }
    }
}