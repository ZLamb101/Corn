
using UnityEngine;

namespace ActionBar.Model
{
    public class SpellSO : ScriptableObject
    {
        public int ID => GetInstanceID();

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }

        public int Rank { get; private set; }
    }
}
