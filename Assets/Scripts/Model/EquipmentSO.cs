using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquipmentSO : ScriptableObject
    {
        [SerializeField]
        private List<EquipmentItem> equippedItems;

        [field: SerializeField]
        public int Size { get; private set; }

        public event Action<Dictionary<int, EquipmentItem>> OnInventoryChanged;

        [SerializeField]
        private InventorySO inventoryData;

        public void Initialize()
        {
            equippedItems = new List<EquipmentItem>();
            for (int i = 0; i < Size; i++)
            {
                equippedItems.Add(EquipmentItem.GetEmptyItem());
            }
        }

        public bool EquipItem(EquippableItemSO item,  List<ItemParameter> itemState = null)
        {
            if (item.IsStackable == false)
            {
                foreach (int i in Enum.GetValues(typeof(ItemSO.ItemType)))
                {
                    if(item.equipmentSlot.ToString() == Enum.GetName(typeof(ItemSO.ItemType), i))
                    {
                        if (!equippedItems[i].IsEmpty)
                        {
                            inventoryData.AddItem(equippedItems[i].item, 1, itemState);
                            equippedItems[i] = EquipmentItem.GetEmptyItem();
                        }
                        AddItemToEquippableSlot(item, itemState);
                        InformAboutChange();
                        return true;
                    }
                }              
            }   
            return false;
        }

        private void AddItemToEquippableSlot(EquippableItemSO item, List<ItemParameter> itemState)
        {   
            foreach (int i in Enum.GetValues(typeof(ItemSO.ItemType)))
            {
                if (item.equipmentSlot.ToString() == Enum.GetName(typeof(ItemSO.ItemType), i))
                {
                    equippedItems[i] = new EquipmentItem
                    {
                        item = item,
                        itemState = new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState)
                    };
                }
            }
        }

        public Dictionary<int, EquipmentItem> GetCurrentEquippedState()
        {
            Dictionary<int, EquipmentItem> returnValue =
                new Dictionary<int, EquipmentItem>();

            for (int i = 0; i < equippedItems.Count; i++)
            {
                if (equippedItems[i].IsEmpty)
                    continue;
                returnValue[i] = equippedItems[i];
            }
            return returnValue;
        }


        public EquipmentItem GetItemAt(int itemIndex)
        {
            return equippedItems[itemIndex];
        }

        internal void EquipItem(EquipmentItem item)
        {
            EquipItem(item.item, item.itemState);
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            /*InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();*/
        }

        public void RemoveItem(int index)
        {
            inventoryData.AddItem(equippedItems[index].item, 1, equippedItems[index].itemState);
            equippedItems[index] = EquipmentItem.GetEmptyItem();
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryChanged?.Invoke(GetCurrentInventoryState());
        }

        //  public GameObject GetItem(int itemIndex, int quantity = 1)
        // {
        /* string itemName = inventoryItems[itemIndex].item.name;
         GameObject itemToDrop = GameObject.Find(itemName);
         itemToDrop.GetComponent<ItemDrop>().Quantity = quantity;
         return itemToDrop;*/
        //}

        public Dictionary<int, EquipmentItem> GetCurrentInventoryState()
        {
            Dictionary<int, EquipmentItem> returnValue =
                new Dictionary<int, EquipmentItem>();

            for (int i = 0; i < equippedItems.Count; i++)
            {
                if (equippedItems[i].IsEmpty)
                    continue;
                returnValue[i] = equippedItems[i];
            }
            return returnValue;
        }
    }


    [Serializable]
    public struct EquipmentItem
    {
        public EquippableItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public EquipmentItem ChangeQuantity(int newQuantity)
        {
            return new EquipmentItem
            {
                item = this.item,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static EquipmentItem GetEmptyItem()
            => new EquipmentItem
            {
                item = null,
                itemState = new List<ItemParameter>()
            };
    }
}