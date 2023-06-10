using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class Parry : MonoBehaviour
{
    //En esta lista los enemigos se registrarán si entran en un rango
    public static List<GameObject> enemiesInRange = new List<GameObject>();
    public static float parryRange = 2f;

    [Header("Parry Variables")]
    private bool isParryng;
    [SerializeField] private float parryForce = 2f;

    [Header("Effects")]
    [SerializeField] ParticleSystem Parryeffect;
    private Animator anim;
    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        isParryng = false;
    }

    private void Update()
    {
       if (Input.GetButtonDown("Fire1") && !isParryng)
        {
            ParryAction();         
        }
    }
    private void ParryAction()
    {
        isParryng = true;
        anim.SetTrigger("Parry");
    }
    public void ParryFinish()
    {
        isParryng = false;
    }
    public void ParryEffect()
    {
        Parryeffect.Play();
        GameEventBus.Publish(GameEventType.PARRY);

        
        //Ataque usando el gameObject enemigo
        foreach (GameObject enemy in enemiesInRange)
        {
            //Actualmente hacemos que el enemigo se muera
            enemy.GetComponent<Enemy>().Die();
        }



        ///Ataque usando físicas
        /* // Get all colliders within a specified range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, parryRange);

        // Apply a force to each rigidbody2D component attached to the colliders
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                Vector2 direction = (rb.transform.position - transform.position).normalized;
                rb.AddForce(direction * parryForce, ForceMode2D.Impulse);
            }
        } */
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, parryRange);
    }
}
