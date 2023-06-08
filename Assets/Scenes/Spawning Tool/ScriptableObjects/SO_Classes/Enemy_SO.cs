using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/New Enemy", order = 1)]

public class Enemy_SO : ScriptableObject
{
    //some characteristics only apply for shielders and regenerators
    [Header("General enemy stats")]
    public enemyType myEnemyType;
    public Sprite sprite; //eventually change this for an Animator
    public float MaxHP;
    public float speed;
    [Tooltip("Lives taken from player")]
    public byte damage;

}

public enum enemyType
{
    Deambulante,
    Atacante, 
    Distancia, 
    Pasivo, 
    Especial
}
