using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace ActionBar.Model
{
    [CreateAssetMenu]
    public class ActionBarSO : ScriptableObject
    {
        [SerializeField]
        private List<ActionSlot> actionBarSpells;

        [field: SerializeField]
        public int Size { get; private set; }

        public event Action<Dictionary<int, ActionSlot>> OnActionBarChanged;

        public void Initialize()
        {
            actionBarSpells = new List<ActionSlot>();
            for (int i = 0; i < Size; i++)
            {
                actionBarSpells.Add(ActionSlot.GetEmptyItem());
            }
        }
        public ActionSlot GetSpellSlotAt(int slotIndex)
        {
            return actionBarSpells[slotIndex];
        }
    }

    [Serializable]
    public struct ActionSlot
    {
        public int cooldown;
        public SpellSO item;
        public bool IsEmpty => item == null;

        public ActionSlot ChangeQuantity()
        {
            return new ActionSlot
            {
                item = this.item
            };
        }

        public static ActionSlot GetEmptyItem()
            => new ActionSlot
            {
                item = null,
            };
    }
}
