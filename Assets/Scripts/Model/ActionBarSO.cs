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

        internal bool AddSkill(ActionSlot actionSlot)
        {
            for (int i = 0; i < Size; i++)
            {
                if (!actionBarSpells[i].IsEmpty)
                {
                    //inventoryData.AddItem(equippedItems[i].item, 1, itemState);
                    //equippedItems[i] = EquipmentItem.GetEmptyItem();
                }
                AddSkillToActionSlot(actionSlot.skill);
                InformAboutChange();
                return true;
            }
            return false;
        }

        private void AddSkillToActionSlot(SpellSO skill)
        {
            for (int i = 0; i < actionBarSpells.Count; i++)
            {
                if (actionBarSpells[i].IsEmpty)
                {
                    actionBarSpells[i] = new ActionSlot
                    {
                        skill = skill
                    };
                    return;
                }
            }
        }

        private void InformAboutChange()
        {
            OnActionBarChanged?.Invoke(GetCurrentActionBarState());
        }

        public Dictionary<int, ActionSlot> GetCurrentActionBarState()
        {
            Dictionary<int, ActionSlot> returnValue =
                new Dictionary<int, ActionSlot>();

            for (int i = 0; i < actionBarSpells.Count; i++)
            {
                if (actionBarSpells[i].IsEmpty)
                    continue;
                returnValue[i] = actionBarSpells[i];
            }
            return returnValue;
        }
    }

    [Serializable]
    public struct ActionSlot
    {
        public int cooldown;
        public SpellSO skill;
        public bool IsEmpty => skill == null;

        public ActionSlot ChangeQuantity()
        {
            return new ActionSlot
            {
                skill = this.skill
            };
        }

        public static ActionSlot GetEmptyItem()
            => new ActionSlot
            {
                skill = null,
            };
    }
}
