using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CharacterEvents
{
    //Character damaged and damage value
    public static UnityAction<GameObject, int> characterDamaged;
    //Character damaged and amount healed
    public static UnityAction<GameObject, int> characterHealed;

    public static UnityAction<GameObject, int> characterXpGained;
}
