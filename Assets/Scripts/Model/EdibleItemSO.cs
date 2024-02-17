using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modiferData = new List<ModifierData>();

        public string ActionName => "Consume";

        public AudioClip actionSFK { get; private set; }

        public bool PerformAction(GameObject character)
        {
            bool consumedItem = false;
            foreach (ModifierData data in modiferData)
            {
                consumedItem = data.statModifier.AffectCharacter(character, data.value);
            }
            return consumedItem;
        }

    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFK { get; }
        bool PerformAction(GameObject target);
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public int value;
    }
}
