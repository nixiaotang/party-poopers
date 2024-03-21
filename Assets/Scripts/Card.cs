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
    AllEnemy
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
    [SerializeField] private CardInfo _card;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _descText;

    private CardManager _cardManager;

    public void Init(CardInfo cardInfo)
    {
        _card = cardInfo;
        _cardManager = transform.parent.GetComponent<CardManager>();

        _nameText.text = _card.name;
        transform.name = _card.name + " - Card";
        _costText.text = $"Mana: {_card.mana}";
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
