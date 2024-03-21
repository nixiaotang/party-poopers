using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardInfo> cardInfos;
    [SerializeField] private GameObject cardPrefab;

    private List<Card> cards = new();

    private void Start()
    {
        // instantiate every card specified by cardInfos list
        for(int i = 0; i < cardInfos.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, this.transform, false);
            cardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-((cardInfos.Count-1) * 110 / 2) + i * 110, 140);

            Card card = cardObj.GetComponent<Card>();
            card.Init(cardInfos[i]);
            cards.Add(card);
        }
    }

}
