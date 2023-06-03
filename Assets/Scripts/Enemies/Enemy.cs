using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private EnemyStats myStats;

    private NavMeshAgent navMeshAgent;
    private Behaviours[] baseBehavioursArray;
    [SerializeField]
    private Behaviours StartingAbilityOnAwake;
    [SerializeField]
    private Behaviours currentAbilityPlaying;
    public void DisableNavMeshMovement() => navMeshAgent.isStopped = true;
    public void EnableNavMeshMovement() => navMeshAgent.isStopped = false;
    public void SetTargetToMoveTo(Vector2 target) => navMeshAgent.SetDestination(target);
    public Vector2 EnemyPosition { get { return transform.position; } }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        baseBehavioursArray = GetComponents<Behaviours>();
        Behaviours.OnAnyAbilityCompleted += OnBehaviourCompleted;
    }

    private void Start()
    {
        EnterBehaviour(StartingAbilityOnAwake);
    }


    private void Update()
    {
        if (currentAbilityPlaying == null)
            return;

        currentAbilityPlaying.Tick();
    }

    private void EnterBehaviour(Behaviours e)
    {
        currentAbilityPlaying = null;
        currentAbilityPlaying = e;
        //currentAction = e.abilityEnum;
        currentAbilityPlaying.OnEnterBehaviour();
    }

    private void OnBehaviourCompleted(object sender, Behaviours e)
    {
        EnterBehaviour(e);
    }

    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    public T GetAbility<T>() where T : Behaviours
    {
        foreach (Behaviours baseAction in baseBehavioursArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }

        return null;
    }
}


public enum BehavioursEnum
{
    Idle, 
    Patrol,
    Chase,
    Shooting,
    Special
}