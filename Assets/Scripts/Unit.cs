using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 10;
    protected int health;

    [SerializeField]
    protected int startingHandSize = 5;

    [SerializeField]
    protected int baseMana = 10;
    protected int mana;

    [SerializeField]
    protected Type type = Type.None;
    // NOTES:
    //  deck is also just used as draw
    //  by convention, end of deck is TOP
    //  front of deck is BOTTOM
          
    [SerializeField]
    protected List<CardInfo> deck;
    protected List<CardInfo> hand = new();
    protected List<CardInfo> discard = new();

    [Space]

    [SerializeField] private TextMeshProUGUI healthUI;

    [SerializeField] private CardManager cardManager; // TODO: Fix this

    void Start()
    {
        Shuffle();
        health = maxHealth;
        mana = baseMana;

        DrawCards(1);
        cardManager.UpdateHand(hand);
    }

    private void Update()
    {
        // currently update UI every frame, will probably change this in the future
        healthUI.text = $"{health}";
    }


    // moves discard to deck, then shuffles
    protected void Shuffle()
    {
        deck.AddRange(discard);
        discard.Clear();

        // todo: check this lol
        deck = deck.OrderBy(_ => NumberHelper.comabtRNG.Next()).ToList();
    }

    // checks if this unit can play its turn at all (dead or other statuses)
    public bool CanAct()
    {
        return health > 0;
    }

    private void DrawCards(int n = 1)
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
    public List<CardInfo> GetHand()
    {
        return hand;
    }
    public int GetDiscardSize()
    {
        return discard.Count();
    }
    public int GetDrawSize()
    {
        return deck.Count();
    }

    // get unit type
    public Type GetUnitType()
    {
        return type;
    }

    // moves a card in a spot to discard, and then return it
    private CardInfo MoveToDiscard(int n)
    {
        // pre: assume n is valid
        var playedCard = hand[n];
        hand.RemoveAt(n);
        discard.Add(playedCard);
        return playedCard;
    }

    public bool CanPlayCard(int n)
    {
        if (n >= hand.Count)
        {
            throw new System.ArgumentException(string.Format("Invalid card number: {0} max is: {1}", n, hand.Count), "n");
        }
        return hand[n].mana <= mana;
    }

    // moves a card to discard, subtracts mana cost, and returns the card.
    public CardInfo PlayCard(int n)
    {
        // pre: checks for can play should have been made
        if (n >= hand.Count)
        {
            throw new System.ArgumentException(string.Format("Invalid card number: {0} max is: {1}", n, hand.Count), "n");
        }
        if (!CanPlayCard(n))
        {
            //todo this is probably the wrong error look this up later
            throw new System.ArgumentException(string.Format("Cannot play card {0}", n), "n");
        }
        
        mana -= hand[n].mana;
        return MoveToDiscard(n);
    }

    // starts turn by drawing required cards
    public void StartTurn()
    {
        DrawCards(startingHandSize);
    }

    // ends turn by discarding all cards from this players hand
    public void EndTurn()
    {
        while (hand.Count > 0)
        {
            MoveToDiscard(0);
        }
    }

    // raw effects
    // deals damage straight to player
    protected void RawTakeDamage(int rawDamage)
    {
        health = System.Math.Max(health - rawDamage, 0);
    }

    // heals directly
    protected void RawHealHealth(int rawHeal)
    {
        health = System.Math.Min(health + rawHeal, maxHealth);
    }



    // resolve effect

    // we assume the gamemanager has passed a single instance of an effect to
    // the Unit. This means we dont care about information such as inner mult
    // outermult, as the gamemanager deals with those.

    // we pass the caster of the spell incase they have some extra information
    // when calculating damage
    // attached to them
    public void ResolveEffect(EffectInfo effect, Unit caster)
    {
        switch (effect.effect)
        {
            case EffectType.Damage:
                Damage(effect, caster);
                break;
            case EffectType.Heal:
                Heal(effect, caster);
                break;
            case EffectType.Draw:
                DrawCardEffect(effect, caster);
                break;
        }
    }


    // INDIVIDUAL EFFECTS:
    private void Damage(EffectInfo effect, Unit caster)
    {
        var damageTaken = NumberHelper.CalculateBasicDamage(effect.intensity, effect.type, type, caster.GetUnitType());
        // we can call animations or effects here?
        RawTakeDamage(damageTaken);
    }

    private void Heal(EffectInfo effect, Unit caster)
    {
        var amountHealed = effect.intensity;
        // anim
        RawHealHealth(amountHealed);
    }

    private void DrawCardEffect(EffectInfo effect, Unit caster)
    {
        var n = effect.intensity;
        // anim
        DrawCards(n);
    }
}
