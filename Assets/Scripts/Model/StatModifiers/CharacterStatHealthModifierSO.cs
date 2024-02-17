using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override bool AffectCharacter(GameObject character, float value)
    {
        Damageable damageable = character.GetComponent<Damageable>();
        if(damageable != null)
        {
            return damageable.Heal((int)value);
        }
        else
        {
            Debug.LogWarning("Character does not have a damageable component");
        }
        return false;
    }

}
