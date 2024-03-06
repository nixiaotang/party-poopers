using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Type
    {
        Fire,
        Ice,
        Earth,
        Elec,
        Light,
        Dark
    }


    [System.Serializable]
    public class CardInfo
    {
        public string name;
        public Type type;
        public int damage;
        public int cost;
        public string description;
    }

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
        _damageText.text = $"Damage: {_card.damage}";
        _costText.text = $"Cost: {_card.cost}";
        _descText.text = _card.description;
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
