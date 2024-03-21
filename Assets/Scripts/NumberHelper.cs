using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class NumberHelper
{
    // defender is strong to type
    const float strongMultiplier = 0.5f;
    // weak to type
    const float weakMultipler = 2f;

    public static readonly Type[][] oppositeTypes = new Type[][] {
        new Type[] {Type.Fire, Type.Ice },
        new Type[] {Type.Earth, Type.Elec },
        new Type[] {Type.Light, Type.Dark },
    };
  
    public static int calculateDamage(EffectInfo effect, Unit defender)
    {
        var baseDamage = effect.intensity;
        var damageType = effect.type;
        
        


        return 0;
    }
    
}
