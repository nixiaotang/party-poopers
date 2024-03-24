using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    // get targets give targetSpace and target, can be empty if target is invalid
    private List<Unit> GetTargetsFromTarget(TargetSpace targetSpace, Unit target, Unit caster)
    {
        List<Unit> targets = new() { };

        switch (targetSpace)
        {
            case TargetSpace.All:
                targets.AddRange(players);
                targets.AddRange(enemies);
                break;

            case TargetSpace.AllAlly:
                if (caster is PartyMember) targets.AddRange(players);
                else targets.AddRange(enemies);
                break;

            case TargetSpace.AllEnemy:
                if (caster is PartyMember) targets.AddRange(enemies);
                else targets.AddRange(players);
                break;

            case TargetSpace.AnyAlly:
                if (target.GetType() == caster.GetType()) targets.Add(target);
                break;

            case TargetSpace.AnyEnemy:
                if (target.GetType() != caster.GetType()) targets.Add(target);
                break;

            case TargetSpace.Any:
                targets.Add(target);
                break;

            case TargetSpace.Self:
                if (caster == target) targets.Add(target);
                break;
        }
        return targets;
    }

    // returns if card could be resolved or not
    private bool ResolveCard(Card card, Unit caster, Unit intendedTarget)
    {
        TargetSpace targetSpace = card.cardInfo.targetSpace; 

        List<Unit> targets = GetTargetsFromTarget(targetSpace, intendedTarget, caster);

        if (targets.Count == 0)
        {
            return false;
        }
        EffectInfo[] effectInfos = card.cardInfo.effects;

        foreach (EffectInfo effect in effectInfos)
        {

            for (int i = 0; i < effect.outerMult; i++)
            {
                // todo refactor this

                if (effect.targetType == EffectTarget.Random)
                {
                    ResolveInner(effect, targets[NumberHelper.comabtRNG.Next(targets.Count)], caster);
                }
                else if (effect.targetType == EffectTarget.All)
                {
                    foreach (Unit target in targets)
                    {
                        ResolveInner(effect, target, caster);
                    }
                }
                else if (effect.targetType == EffectTarget.Self)
                {
                    ResolveInner(effect, caster, caster);
                }
                else if (effect.targetType == EffectTarget.SelectedTarget)
                {
                    ResolveInner(effect, intendedTarget, caster);
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
        bool originalPlayerTurn = playerTurn;
        int originalNum = originalPlayerTurn ? enemyTurnNum : playerTurnNum;

        GetCurrentUnit().EndTurn();
        IncrementTurn();

        // this is horrible and might be wrong
        // todo please fix !
        while (originalPlayerTurn != playerTurn || originalNum != (playerTurn ? enemyTurnNum : playerTurnNum))
        {
            if (GetCurrentUnit().CanAct())
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

    public bool IsPlayerTurn()
    {
        return playerTurn;
    }


    public List<Card> GetCurrentUnitCards()
    {
        return GetCurrentUnit().GetHand();
    }
    public int GetCurrentUnitDrawSize()
    {
        return GetCurrentUnit().GetDrawSize();
    }
    public int GetCurrentUnitDiscard()
    {
        return GetCurrentUnit().GetDiscardSize();
    }


    private List<int> GetPlayableCardPositions()
    {
        return Enumerable.Range(0, GetCurrentUnit().GetHand().Count)
                         .Where(i => GetCurrentUnit().CanPlayCard(i))
                         .ToList();
    }
    // generates a list of int unit pairs
    // each pair is the position of a card, and a target it can hit
    // this should be super helpful for UI as it allows you to find out which
    // cards can target what
    public List<(int, Unit)> GetAllCardTargetPairs()
    {
        Unit currentUnit = GetCurrentUnit();
        List<(int, Unit)> pairs = new();
        foreach (int i in GetPlayableCardPositions())
        {
            TargetSpace targetSpace = currentUnit.GetHand()[i].cardInfo.targetSpace;
            foreach (Unit target in players.Concat<Unit>(enemies))
            {
                if (GetTargetsFromTarget(targetSpace, target, currentUnit).Count > 0) {
                    pairs.Add((i, target));
                }
            }
        }
        return pairs;
    }


    private void DoAction(ActionInfo action)
    {
        if (action.actionType == ActionType.EndTurn)
        {
            EndTurn();
            return;
        }

        if (action.actionType == ActionType.PlayCard)
        {
            PlayCard(action.actionNum, action.target);
            return;
        }
    }
  
    private void TakeEnemyTurn()
    {
        if (playerTurn) {
            throw new System.InvalidOperationException(
                "Trying to take enemy turn on player turn");
        }
        if (GetCurrentUnit() is not EnemyUnit)
        {
            // this should never happen
            throw new System.InvalidOperationException(
                "how (not player turn but unit is player)");
        }
        EnemyUnit enemy = (EnemyUnit)GetCurrentUnit(); // downcast to EnemyUnit
        DoAction(enemy.GiveAction(GetAllCardTargetPairs()));
    }

    // take all enemy turns
    public void TakeEnemyTurns()
    {
        while (!playerTurn)
        {
            TakeEnemyTurn();
        }
    }


    // change void to some struct
    // give information about the game state (win, loss, other flags)
    public void CheckGameState()
    {

    }
}
