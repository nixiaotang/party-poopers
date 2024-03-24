using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public ActionInfo GiveAction(List<PartyMember> partyMembers, List<EnemyUnit> enemyUnits, Unit caster, List<(int, Unit)> allPossibleCardUnitPairs)
    {
        if (caster != this)
        {
            throw new System.ArgumentException("Casting Unit is not same as current unit");
        }
        return enemyAI.ChooseAction(partyMembers, enemyUnits, caster, GetPossibleAIActions(allPossibleCardUnitPairs));
    }
    
}

