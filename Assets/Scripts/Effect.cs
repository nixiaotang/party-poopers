
using UnityEngine;
using TMPro;

public enum EffectType
{
    Damage,
    Heal,
    Draw
}

public enum EffectTarget
{
    Random,
    All,
    Self,
    SelectedTarget,
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
    private EffectInfo _effectInfo;

    [SerializeField] private TextMeshProUGUI _effectText;
    [SerializeField] private TextMeshProUGUI _intensityText;
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _targetTypeText;
    [SerializeField] private TextMeshProUGUI _innerMultText;
    [SerializeField] private TextMeshProUGUI _outerMultText;


    public void Init(EffectInfo effectInfo)
    {
        _effectInfo = effectInfo;

        _effectText.text = _effectInfo.effect.ToString();
        _intensityText.text = _effectInfo.intensity.ToString();
        _typeText.text = _effectInfo.type.ToString();
        _targetTypeText.text = _effectInfo.targetType == EffectTarget.All ? "ALL" : "RAND";
        _innerMultText.text = $"x{_effectInfo.innerMult}";
        _outerMultText.text = $"x{_effectInfo.outerMult}";
    }
}