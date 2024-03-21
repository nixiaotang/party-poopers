using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class NumberHelper
{
    // defender is strong to type
    const float strongMultiplier = 0.5f;
    // weak to type
    const float weakMultipler = 2f;


    public static readonly (Type, Type)[] oppositeTypes = new (Type, Type)[] {
        (Type.Fire, Type.Ice),
        (Type.Earth, Type.Elec),
        (Type.Light, Type.Dark)
    };

    // calculate basic damage based on damage type, base damage, and defender type
    public static int calculateBasicDamage(int baseDamage, Type damageType, Type defenderType)
    {
        if (damageType == Type.None)
        {
            // todo: This might be the the wrong error type
            throw new ArgumentException("Effect for damage cannot be none");
        }

        if (damageType == Type.Absolute)
        {
            return baseDamage;
        }

        if (defenderType == damageType)
        {
            // we assume baseDamage is positive
            return (int)(baseDamage * strongMultiplier);
        }

        if (oppositeTypes.Contains((defenderType, damageType)) ||
            oppositeTypes.Contains((damageType, defenderType)))
        {
            return (int)(baseDamage * weakMultipler);
        }

        // no type weaknesses so return base damage
        return baseDamage;
    }
    
}
