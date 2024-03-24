using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public enum Type
{
    None,
    Fire,
    Ice,
    Earth,
    Elec,
    Light,
    Dark,
    Absolute,
}

public enum TargetSpace
{
    Any,
    AnyAlly,
    AnyEnemy,
    All,
    AllAlly,
    AllEnemy,
    Self
}

[System.Serializable]
public struct CardInfo
{
    public string name;
    public Type type;
    public int mana;
    public TargetSpace targetSpace;
    public EffectInfo[] effects; // maybe make into List<Effect>() instead
}

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardInfo cardInfo;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _cardDescText;
    [SerializeField] private Transform _effectParent;
    [SerializeField] private GameObject _effectPrefab;

    private CardManager _cardManager;

    public void Init(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;
        _cardManager = transform.parent.GetComponent<CardManager>();

        _nameText.text = cardInfo.name;
        transform.name = cardInfo.name + " - Card";
        _cardDescText.text = $"Type: {cardInfo.type}\nMana: {cardInfo.mana}\nTS: {cardInfo.targetSpace}";

        foreach(EffectInfo effectInfo in cardInfo.effects)
        {
            GameObject effectObj = Instantiate(_effectPrefab, _effectParent);
            effectObj.GetComponent<Effect>().Init(effectInfo);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
