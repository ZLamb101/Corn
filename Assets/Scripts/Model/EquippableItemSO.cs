using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
 
        public string ActionName => "Equip";

        public AudioClip actionSFK { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            EquipmentUIController weaponSystem = character.GetComponent<EquipmentUIController>();
            if (weaponSystem == null)
            {
                return false;
            }
            weaponSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
            return true;
        }
    }
}
