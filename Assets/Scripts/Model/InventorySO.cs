using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] 
    private List<ItemSO> inventoryItems;

    [field: SerializeField]
    public int Size { get; private set; } = 10;

    public void Awake() {
        inventoryItems = new List<ItemSO>();
    
    }
}
