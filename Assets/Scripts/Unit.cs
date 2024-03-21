using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private int health = 10;

    [SerializeField]
    private int baseMana = 10;

    [SerializeField]
    private Type type = Type.None;
    // NOTES:
    //  deck is also just used as draw
    //  by convention, end of deck is TOP
    //  front of deck is BOTTOM
          
    [SerializeField]
    private List<Card> deck;

    private List<Card> hand;
    private List<Card> discard;

    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
    }

    
    // moves discard to deck, then shuffles
    void Shuffle()
    {
        deck.AddRange(discard);
        discard.Clear();

        // todo: check this lol
        var rng = new System.Random();
        deck = deck.OrderBy(_ => rng.Next()).ToList();
    }

    public void DrawCards(int n = 1)
    {
        for (int i = 0; i < n; i ++)
        {
            if (deck.Count == 0)
            {
                Shuffle();
                // if no more cards to draw stop
                if (deck.Count == 0)
                {
                    break;
                }
            }
            hand.Add(deck[deck.Count - 1]);
            deck.RemoveAt(deck.Count - 1);
        }
    }

    // get hand
    public List<Card> getHand()
    {
        return hand;
    }

    // moves a card in a spot to discard, and then return it
    // we might not need to return the card
    public Card moveToDiscard(int n)
    {
        if (n >= hand.Count)
        {
            throw new System.ArgumentException(string.Format("Invalid card number: {0} max is: {1}", n, hand.Count), "n");
        }
        var playedCard = hand[n];
        hand.RemoveAt(n);
        return playedCard;
    }




    // resolve effect
    public void resolveEffect(EffectInfo effect, Unit caster)
    {

    }


    // INDIVIDUAL EFFECTS:

    private void Damage()
    {

    }


}
