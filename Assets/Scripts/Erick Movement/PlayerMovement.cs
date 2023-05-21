using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float initialMoveSpeed = 10f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxVelocity = 5f;
    [Range(0f, 1f)]
    [SerializeField] private float slowdownFactor = 0.5f;
    [Range(0f, 100f)]
    [SerializeField] private float willPower;

    public static Transform playerTransform;

    [Header("Parry Variables")]
    private bool isParryng;
    [SerializeField] private float parryRange = 2f;
    [SerializeField] private float parryForce = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 currentVelocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    [Header("Effects")]
    [SerializeField] ParticleSystem Parryeffect;

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
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if (movement != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,3000*Time.deltaTime);
        }

        moveSpeed = willPower;
        slowdownFactor = (willPower / 100);

        if (willPower <= 20)
        {
            moveSpeed = initialMoveSpeed;
        }

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

        //Actualizamos la variable estática con la posición del jugador
        playerTransform = transform;
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
        // Get all colliders within a specified range
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
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, parryRange);
    }
}
