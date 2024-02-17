using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "New Equippable Item", menuName = "Inventory/Equippable Item")]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public enum EquipmentSlot
        {
            Head,
            Chest,
            Legs,
            Feet,
            Hands,
            Weapon,
            Shield,
            Accessory
        }

        public EquipmentSlot equipmentSlot;

        public void DestroyItem()
        {
            Debug.Log("Destroying " + itemName);
        }

        public void UseItem()
        {
            Debug.Log("Using " + itemName);
        }
 
        public string ActionName => "Equip";

        public AudioClip actionSFK { get; private set; }

        public bool PerformAction(GameObject target)
        {
            throw new System.NotImplementedException();
        }
    }
}
