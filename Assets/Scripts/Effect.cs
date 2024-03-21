
using UnityEngine;

public enum EffectType
{
    Damage,
    Heal,
    Draw
}

public enum EffectTarget
{
    Random,
    All
}

[System.Serializable]
public struct EffectInfo
{
    public EffectType effect;
    public int intensity;
    public Type type;
    public EffectTarget targetType; // choose target out of Card.TargetSpace
    public int innerMult; // # of times effect will be applied per "round"
    public int outerMult; // # of "rounds" this effect will be applied
}

public class Effect : MonoBehaviour
{
    // handels UI stuff
}