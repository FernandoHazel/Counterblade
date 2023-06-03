using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : Behaviours
{
    [Header("Customization")]
    
    [SerializeField]
    private int maxRange;
    [SerializeField]
    private int minRange;
    [SerializeField, Min(1)] private int patrollingAmount;
    [SerializeField] private bool ChasePlayerOnSeen;


    //private
    private Vector2 targetPosition;
    private int currentPatrolLap;
    public override string GetAbilityName()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDamaged()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDead()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnterBehaviour()
    {
        currentPatrolLap = 0;
        targetPosition = ReturnRandomPointInScenario(minRange, maxRange, _Enemy.EnemyPosition);
        _Enemy.EnableNavMeshMovement();

        _Enemy.SetTargetToMoveTo(targetPosition);
    }

    public override void OnExitBehaviour(Behaviours behaviourToGo)
    {
        targetPosition = Vector2.zero;
        ActionComplete(behaviourToGo, "Patrol");
    }

    public override void OnHealed()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        if (targetPosition == Vector2.zero)
            return;

        if (Vector2.Distance(_Enemy.EnemyPosition, targetPosition) > 1)
        {
            Debug.Log("DISTANCE: "+ Vector2.Distance(_Enemy.EnemyPosition, targetPosition));
            return;
        }
            

        currentPatrolLap++;
        if (currentPatrolLap >= patrollingAmount)
        {
            OnExitBehaviour(_Enemy.GetAbility<IdleBehaviour>());
        }else
        {
            targetPosition = ReturnRandomPointInScenario(minRange, maxRange, _Enemy.EnemyPosition);
            _Enemy.SetTargetToMoveTo(targetPosition);
        }
    }
}
