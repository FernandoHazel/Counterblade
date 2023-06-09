using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/New Enemy", order = 1)]
public class EnemyStats : ScriptableObject
{
    [SerializeField]
    private int baseMovementSpeed;

    public enemyType myEnemyType;


    public float GetMovementSpeed { get { return baseMovementSpeed; } }
}

public enum enemyType
{
    Idle, 
    Patrol,
    Chase,
    Shooting,
    Special
}