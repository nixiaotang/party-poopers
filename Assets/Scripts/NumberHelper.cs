using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class NumberHelper
{
    // defender is strong to type
    const float strongMultiplier = 0.5f;
    // weak to type
    const float weakMultipler = 2f;

    const float casterBonusMult = 1.5f;

    // RNG systems
    // should all be seeded by one central RNG at the start later
    public static System.Random comabtRNG = new();
    public static System.Random AIRNG = new();

    public static readonly (Type, Type)[] oppositeTypes = new (Type, Type)[] {
        (Type.Fire, Type.Ice),
        (Type.Earth, Type.Elec),
        (Type.Light, Type.Dark)
    };

    // calculate basic damage based on damage type, base damage, and defender type
    public static int CalculateBasicDamage(int baseDamage, Type damageType, Type defenderType, Type casterType)
    {
        if (damageType == Type.None)
        {
            // todo: This might be the the wrong error type
            throw new System.ArgumentException("Effect for damage cannot be none");
        }

        if (damageType == casterType)
        {
            baseDamage = (int) (baseDamage * casterBonusMult);
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

        // if weakness pair is in opposite types
        if (oppositeTypes.Contains((defenderType, damageType)) || oppositeTypes.Contains((damageType, defenderType)))
        {
            return (int)(baseDamage * weakMultipler);
        }

        // no type weaknesses so return base damage
        return baseDamage; 
    }
    
}
