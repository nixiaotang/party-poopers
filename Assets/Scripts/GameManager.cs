using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private List<PartyMember> players;

    [SerializeField]
    private List<EnemyUnit> enemies;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // checks if the intended target is valid
    private bool CheckValidIntendedTarget(TargetSpace targetSpace, Unit intendedTarget)
    {
        if (targetSpace == TargetSpace.Any || targetSpace == TargetSpace.All)
        {
            return true;
        }

        bool isPartyMember = intendedTarget is PartyMember;

        if (isPartyMember)
        {
            return targetSpace == TargetSpace.AllAlly || targetSpace == TargetSpace.AnyAlly;
        }

        return targetSpace == TargetSpace.AllEnemy || targetSpace == TargetSpace.AnyEnemy;
    }


    private List<Unit> GetTargetsFromTargetSpace(TargetSpace targetSpace, Unit target)
    {
        // pre: assume target is valid
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
            default:
                targets.Add(target); // valid target so any
                break;
        }
        return targets;
    }

    // returns if card could be resolved or not
    public bool ResolveCard(Card card, Unit caster, Unit intendedTarget)
    {
        TargetSpace targetSpace = card.GetComponent<CardInfo>().targetSpace; 
        if (!CheckValidIntendedTarget(targetSpace, intendedTarget))
        {
            return false;
        }
        List<Unit> targets = GetTargetsFromTargetSpace(targetSpace, intendedTarget);




        return true;
    }

    private void ResolveOuter(EffectInfo effect, Unit caster)
    {

    }
}
