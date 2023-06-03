using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "new boss", menuName = "Bosses/NewStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField]
    private int baseMovementSpeed;


    public float GetMovementSpeed { get { return baseMovementSpeed; } }
}
