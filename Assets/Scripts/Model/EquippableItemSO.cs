using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
       /* public enum EquipmentSlot
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

        public EquipmentSlot equipmentSlot;*/
 
        public string ActionName => "Equip";

        public AudioClip actionSFK { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
            if (weaponSystem == null)
            {
                return false;
            }
            weaponSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
            return true;
        }
    }
}
