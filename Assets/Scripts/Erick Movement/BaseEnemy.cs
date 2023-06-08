using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour
{
    public Enemy_SO enemyData;
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 100;
    [SerializeField] private float maxVelocity = 10f;
    [Range(0f, 1f)]
    [SerializeField] private float slowdownFactor = 0.5f;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    private void OnEnable() 
    {
        //Cuando este enemigo aparece agrega uno al contador
        GameManager.enemyCount++;
    }
    private void FixedUpdate()
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

    private void Die ()
    {
        //Al morir destruimos el objeto y restamos uno al contador
        GameManager.enemyCount--;
        Destroy(gameObject);
    }
}
