using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class IdleButton : MonoBehaviour
{
    public TMP_Text IdleText;
    // Start is called before the first frame update

    
    public void Start()
    {
        if (GameManager.instance.IsIdle)
        {
            IdleText.text = "Idle (ON)";
            IdleText.color = Color.green;
        }
        else
        {
            IdleText.text = "Idle (OFF)";
            IdleText.color = Color.red;
        }
    }

    public void ToggleIdle()
    {
        GameManager.instance.IsIdle = !GameManager.instance.IsIdle;
        Debug.Log(GameManager.instance.IsIdle.ToString());
        
        if (GameManager.instance.IsIdle)
        {
            IdleText.text = "Idle (ON)";
            IdleText.color = Color.green;
        }
        else
        {
            IdleText.text = "Idle (OFF)";
            IdleText.color = Color.red;
        }
    }

}
