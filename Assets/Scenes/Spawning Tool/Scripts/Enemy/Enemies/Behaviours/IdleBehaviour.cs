using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : Behaviours
{
    [SerializeField]
    private float IdleStateTime;
    [SerializeField]
    private Behaviours defaultBehaviourOnFinish;

    private float timerIdleState;
    public override string GetAbilityName()
    {
        return "Idle state behaviour";
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
        timerIdleState = 0;
        _Enemy.DisableNavMeshMovement();
    }

    public override void OnExitBehaviour(Behaviours behaviourToGo)
    {
        ActionComplete(behaviourToGo, "Idle");
    }

    public override void OnHealed()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        timerIdleState += Time.deltaTime;
        if(timerIdleState >= IdleStateTime)
        {
            OnExitBehaviour((defaultBehaviourOnFinish));
        }
    }
}
