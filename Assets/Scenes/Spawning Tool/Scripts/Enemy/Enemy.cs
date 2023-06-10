using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EventBus;

public class Enemy : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] private Vector2 targetPosition;
    public EnemyStats myStats;
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

    //Variables para movimiento temporal de fuerza
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 100;
    [SerializeField] private float maxVelocity = 10f;
    [Range(0f, 1f)]
    [SerializeField] private float slowdownFactor = 0.5f;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private bool canBeParried;
    public float distanceFromPlayer;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        gm = FindObjectOfType<GameManager>();
        /* baseBehavioursArray = GetComponents<Behaviours>();
        Behaviours.OnAnyAbilityCompleted += OnBehaviourCompleted; */
    }

    private void OnEnable() 
    {
        GameEventBus.Subscribe(GameEventType.PARRY, OnParry);
    }

    private void OnDisable() 
    {
        GameEventBus.Unsubscribe(GameEventType.PARRY, OnParry);
    }

    private void Start()
    {
        //EnterBehaviour(StartingAbilityOnAwake);

        //Cuando este enemigo aparece agrega uno al contador
        GameManager.enemyCount++;
        gm.UpdateUI();
        canBeParried = true;
    }

    private void Update()
    {
        //Medimos la distancia entre este enemigo y el jugador
        distanceFromPlayer = Vector2.Distance(transform.position, PlayerMovement.playerTransform.position);

        /* if (currentAbilityPlaying == null)
            return;

        currentAbilityPlaying.Tick(); */
    }

    private void FixedUpdate()
    {  
        ForceMovement();
    }

    /* private void EnterBehaviour(Behaviours e)
    {
        currentAbilityPlaying = null;
        currentAbilityPlaying = e;
        //currentAction = e.abilityEnum;
        currentAbilityPlaying.OnEnterBehaviour();
    } */

    /* private void OnBehaviourCompleted(object sender, Behaviours e)
    {
        EnterBehaviour(e);
    }

    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }
    */

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

    //Este es un movimiento temporal usando fuerza
    private void ForceMovement()
    {
        //Nos movemos hacia la posición del jugador
        Vector2 movement = PlayerMovement.playerTransform.position - transform.position;

        if (movement != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000*Time.deltaTime);
        }

        //slowdownFactor = (moveSpeed / 100);

        if ( movement.magnitude > 0)
        {
            rb.AddForce(movement.normalized * moveSpeed);
            currentVelocity = rb.velocity;
        }
        else
        {
            rb.velocity = currentVelocity * (1 - slowdownFactor);
            currentVelocity = rb.velocity;
        }

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        float scale = Mathf.Lerp(1f, 0.80f, rb.velocity.magnitude / maxVelocity);
        transform.localScale = new Vector3(scale, 1f, 1f);
    }

    //Este es el método que corre cada que el enemigo recibe un parry
    private void OnParry()
    {
        if (distanceFromPlayer <= Parry.parryRange && canBeParried)
        {
            Debug.Log(gameObject.name + " was parried");
            Die();
        }
    }

    public void Die ()
    {
        //Al morir destruimos el objeto y restamos uno al contador
        GameManager.enemyCount--;

        //Intentamos pasar a la siguiente oleada
        //El gm verifica primero que todos los enemigos hayan sido eliminados para hacerlo
        gm.NextWave();
        gm.UpdateUI();
        Destroy(gameObject);
    }

    //Si el enemigo collisiona con el jugador ese le hace daño 
    //y se anula la posibilidad de hacer parry
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canBeParried = false;

            Debug.Log("Player was damaged, cannot parry" + gameObject.name);

            //Añadimos el código para dañar al jugador
            //...
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canBeParried = true;
        }
    }
}