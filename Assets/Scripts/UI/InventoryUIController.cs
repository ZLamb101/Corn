using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField]
    private InventoryPage inventoryUI;

    public int inventorySize = 10;

    private void Start() {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    public void ToggleInventoryDisplay() {
        Debug.Log("Toggling Inventory Display");
        if(inventoryUI.isActiveAndEnabled) {
            inventoryUI.Hide();
        } else {
            inventoryUI.Show();
        }
    }



}
