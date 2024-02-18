using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUIController : MonoBehaviour
{
    [SerializeField]
    private EquipmentPage equipmentUI;

    public void ToggleEquipmentDisplay()
    {
        Debug.Log("Toggling Equipment Display");
        if (equipmentUI.isActiveAndEnabled)
        {
            equipmentUI.Hide();
        }
        else
        {
            equipmentUI.Show();
        }
    }
}
