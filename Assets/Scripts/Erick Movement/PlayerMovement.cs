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

    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

   
}
