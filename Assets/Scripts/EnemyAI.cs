using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
