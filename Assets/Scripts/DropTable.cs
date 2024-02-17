using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Drop Table", menuName = "Drop Table")]
public class DropTable : ScriptableObject
{
    [System.Serializable]
    public class DropTableEntry
    {
        public GameObject item; // The item to drop

        //public int Amount; // The amount to drop

        [Range(0f, 1f)]
        public float dropRate; // The drop rate (0-1)
    }

    public List<DropTableEntry> entries = new List<DropTableEntry>(); // List of drop table entries
}