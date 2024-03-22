using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 10;
    protected int health;

    [SerializeField]
    protected int baseMana = 10;

    [SerializeField]
    protected Type type = Type.None;
    // NOTES:
    //  deck is also just used as draw
    //  by convention, end of deck is TOP
    //  front of deck is BOTTOM
          
    [SerializeField]
    protected List<Card> deck;

    protected List<Card> hand;
    protected List<Card> discard;

    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
        health = maxHealth;
    }

    
    // moves discard to deck, then shuffles
    protected void Shuffle()
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
    public List<Card> GetHand()
    {
        return hand;
    }
    // get unit type
    public Type GetUnitType()
    {
        return type;
    }

    // moves a card in a spot to discard, and then return it
    // we might not need to return the card
    public Card MoveToDiscard(int n)
    {
        if (n >= hand.Count)
        {
            throw new System.ArgumentException(string.Format("Invalid card number: {0} max is: {1}", n, hand.Count), "n");
        }
        var playedCard = hand[n];
        hand.RemoveAt(n);
        return playedCard;
    }


    // raw effects
    // deals damage straight to player
    protected void RawTakeDamage(int rawDamage)
    {
        health = System.Math.Max(health + rawDamage, 0);
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
        var damageTaken = NumberHelper.CalculateBasicDamage(effect.intensity, effect.type, GetUnitType(), caster.GetUnitType());
        // we can call animations or effects here?
        RawTakeDamage(damageTaken);
    }

    private void Heal(EffectInfo effect, Unit caster)
    {
        var amountHealed = effect.intensity;

        RawHealHealth(amountHealed);
    }

    private void DrawCardEffect(EffectInfo effect, Unit caster)
    {
        var n = effect.intensity;
        DrawCards(n);
    }
}
