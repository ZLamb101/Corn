using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPage : MonoBehaviour
{
    private void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
