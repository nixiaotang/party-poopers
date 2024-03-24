using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DumbAI : EnemyAI
{
    public override ActionInfo ChooseAction(List<PartyMember> partyMembers, List<EnemyUnit> enemyUnits, Unit caster, List<ActionInfo> allPossibleActions)
    {
        return ChooseAction(allPossibleActions);
    }
    public override ActionInfo ChooseAction(List<ActionInfo> allPossibleActions)
    {
        return allPossibleActions[NumberHelper.AIRNG.Next(allPossibleActions.Count)];
    }
}
