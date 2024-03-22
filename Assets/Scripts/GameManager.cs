using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private List<PartyMember> players;

    [SerializeField]
    private List<EnemyUnit> enemies;

    // turn info
    private bool playerTurn = true;
    private int playerTurnNum = 0;
    private int enemyTurnNum = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // probably helpful for UI
    public List<Unit> GetTargetsFromTargetSpace(TargetSpace targetSpace, Unit target)
    {
        List<Unit> targets = new() { };

        switch (targetSpace)
        {
            case TargetSpace.All:
                targets.AddRange(players);
                targets.AddRange(enemies);
                break;
            case TargetSpace.AllAlly:
                targets.AddRange(players);
                break;
            case TargetSpace.AllEnemy:
                targets.AddRange(enemies);
                break;
            case TargetSpace.AnyAlly:
                if (target is PartyMember)
                {
                    targets.Add(target);
                }
                break;
            case TargetSpace.AnyEnemy:
                if (target is EnemyUnit)
                {
                    targets.Add(target);
                }
                break;
            case TargetSpace.Any:
                targets.Add(target);
                break;
        }
        return targets;
    }

    // returns if card could be resolved or not
    private bool ResolveCard(Card card, Unit caster, Unit intendedTarget)
    {
        TargetSpace targetSpace = card._card.targetSpace; 

        List<Unit> targets = GetTargetsFromTargetSpace(targetSpace, intendedTarget);

        if (targets.Count == 0)
        {
            return false;
        }
        EffectInfo[] effectInfos = card._card.effects;

        foreach (EffectInfo effect in effectInfos)
        {
            foreach (Unit target in targets)
            {
                for (int i = 0; i < effect.outerMult; i++)
                {
                    ResolveInner(effect, target, caster);
                }
            }
        }
        return true;
    }
    private void ResolveInner(EffectInfo effect, Unit target, Unit caster)
    {
    
        switch (effect.effect)
        {
            // add any special cases here (things like random, self damage/draw, etc)

            // damage (basic), heal, draw.
            default: 
                for (int i = 0; i < effect.innerMult; i++) {
                    target.ResolveEffect(effect, caster);
                }
                break;
        }
    }

    // turn management

    private Unit GetCurrentUnit()
    {
        if (playerTurn)
        {
            return players[playerTurnNum];
        }
        return enemies[enemyTurnNum];
    }

    // increments turn counter information
    private void IncrementTurn()
    {
        if (playerTurn)
        {
            playerTurnNum = (playerTurnNum + 1) % players.Count;
            if (playerTurnNum == 0)
            {
                playerTurn = false;
            }
        } else
        {
            enemyTurnNum = (enemyTurnNum + 1) % enemies.Count;
            if (enemyTurnNum == 0)
            {
                playerTurn = true;
            }
        }
    }

    // changes turn, checks for any dead players / enemies and updates
    // turn num accordingly by skipping units that cannot play
    public void EndTurn()
    {
        bool currentPlayerTurn = playerTurn;
        int currentNum = currentPlayerTurn ? enemyTurnNum : playerTurnNum;

        GetCurrentUnit().EndTurn();
        IncrementTurn();

        // this is horrible and might be wrong
        while (currentPlayerTurn != playerTurn || currentNum != (playerTurn ? enemyTurnNum : playerTurnNum))
        {
            if (GetCurrentUnit().CanPlay())
            {
                break;
            }
            IncrementTurn();
        }
        GetCurrentUnit().StartTurn();
    }


    // playing cards
    // attempts to play a card returns true if succesful
    public bool PlayCard(int n, Unit intendedTarget)
    {
        Unit currentUnit = GetCurrentUnit();

        if (!currentUnit.CanPlayCard(n))
        {
            return false;
        }
        Card card = currentUnit.GetHand()[n];
        return ResolveCard(card, currentUnit, intendedTarget);
    }


}
