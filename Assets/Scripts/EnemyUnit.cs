using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



// might be useful in player actions as well
// these can probably should be moved to another file
public enum ActionType
{
    EndTurn,
    PlayCard,
}

// one packet of action information
public struct ActionInfo
{
    public ActionType actionType;
    public int actionNum;
    public Unit target;
}

public abstract class EnemyAI
{
    // children will pass in a game state to this function, give it as much info
    // as possible
    public abstract ActionInfo ChooseAction(List<PartyMember> partyMembers, List<EnemyUnit> enemyUnits, Unit caster, List<ActionInfo> allPossibleActions);
    public abstract ActionInfo ChooseAction(List<ActionInfo> allPossibleActions);
}



public class EnemyUnit : Unit
{
    [SerializeField]
    private EnemyAI enemyAI;

    private ActionInfo PlayCardAction(int n, Unit target)
    {
        return new ActionInfo
        {
            actionType = ActionType.PlayCard,
            actionNum = n,
            target = target

        };
    }

    private ActionInfo EndTurnAction()
    {
        return new ActionInfo
        {
            actionType = ActionType.EndTurn,
            actionNum = 0,
            target = null
        };
    }

    // from a list of playableCards, get all possible enemy actions
    // except end turn
    public List<ActionInfo> GetPossibleAIActions(List<(int, Unit)> allCardUnitPairs)
    {
        List<ActionInfo> actions = new() { EndTurnAction() };
        foreach ((int, Unit) pair in allCardUnitPairs)
        {
            actions.Add(PlayCardAction(pair.Item1, pair.Item2));
        }
        return actions;
    }


    // temporary
    public ActionInfo GiveAction(List<(int, Unit)> allPossibleCardUnitPairs)
    {
        return enemyAI.ChooseAction(GetPossibleAIActions(allPossibleCardUnitPairs));
    }

    public ActionInfo GiveAction(List<PartyMember> partyMembers, List<EnemyUnit> enemyUnits, Unit caster, List<(int, Unit)> allPossibleCardUnitPairs)
    {
        if (caster != this)
        {
            throw new System.ArgumentException("Casting Unit is not same as current unit");
        }
        return enemyAI.ChooseAction(partyMembers, enemyUnits, caster, GetPossibleAIActions(allPossibleCardUnitPairs));
    }
}

