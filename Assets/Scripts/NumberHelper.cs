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
    public static System.Random comabtRNG = new System.Random();

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

        float casterBonus = 1;
        if (damageType == casterType)
        {
            casterBonus = casterBonusMult;
        }

        if (damageType == Type.Absolute)
        {
            return baseDamage; // * casterBonus?
        }

        if (defenderType == damageType)
        {
            // we assume baseDamage is positive
            return (int)(baseDamage * strongMultiplier * casterBonus);
        }

        // if weakness pair is in opposite types
        if (oppositeTypes.Contains((defenderType, damageType)) || oppositeTypes.Contains((damageType, defenderType)))
        {
            return (int)(baseDamage * weakMultipler * casterBonus);
        }

        // no type weaknesses so return base damage
        return baseDamage; // * casterBonus?
    }
    
}
