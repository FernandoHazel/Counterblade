using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    public static event EventHandler<float> OnPlayerHitGround;
    [SerializeField]
    private Configuration _PrototypeConfig;
    private HealthSystem _HealthSystem;
    [SerializeField]
    private ParticleSystem OnLaunchedParticle;
    [SerializeField]
    private ParticleSystem OnHitGroundParticle;
    [SerializeField]
    private GameObject _HPBar;
    [SerializeField]
    private SpriteRenderer _PlayerVisual;



    private bool m_wantsRight = false;
    private bool m_wantsLeft = false;
    private bool m_wantsUp = false;
    private bool m_wantsDown = false;

    public float MOVE_ACCEL = (0.12f * 60.0f);
    public float MAX_TIME_SPEEDCAP;
    public float GROUND_FRICTION = 0.85f;
   // public float GRAVITY = (-0.05f * 60.0f);
   /* public float JUMP_VEL = 0.75f;
    public float JUMP_MIN_TIME = 0.06f;
    public float JUMP_MAX_TIME = 0.20f;
    public float AIR_FALL_FRICTION = 0.975f;
    public float AIR_MOVE_FRICTION = 0.85f;*/
    private float SHAKE_FORCE = 0;
    private Animator m_animator;
    private Rigidbody2D m_rigidBody = null;
    private bool m_shouldSlow = false;
    private bool m_canBeDamaged = true;
    private float m_canBeDamagedTimer = 0.0f;
    private float m_stateTimer = 0.0f;
    private float m_blinkTimer = 0.0f;
    private int m_bounceCounter = 1;
    private Vector2 m_vel = new Vector2(0, 0);
    private List<GameObject> m_groundObjects = new List<GameObject>();
    private Color m_hpBarColor;
    private Vector2 m_startPosition;
    private enum PlayerState
    {
        PS_IDLE = 0,
        //PS_FALLING,
        //PS_JUMPING,
        PS_WALKING,
        //PS_LAUNCHING,
        //PS_BOUNCING,
        PS_DYING
    };
    [SerializeField]
    private PlayerState m_state = PlayerState.PS_IDLE;
    
  
    void Update()
	{
        UpdateInput();
        //CheckAnimation();
        CheckIfCanBeDamaged();
    }
	
    void FixedUpdate()
    {
        switch (m_state)
        {
            case PlayerState.PS_IDLE:
                Idle();
                break;
            /*case PlayerState.PS_FALLING:
                Falling();
                break;
            case PlayerState.PS_JUMPING:
                Jumping();
                break;*/
            case PlayerState.PS_WALKING:
                Walking();
                break;
            /*case PlayerState.PS_LAUNCHING:
                Launching();
                break;
            case PlayerState.PS_BOUNCING:
                Bouncing();
                break;*/
            default:
                break;
        }
    }

    void Idle()
    {
        m_vel = Vector2.zero;
        //Check to see whether to go into movement of some sort

        //Test for input to move
        if (m_wantsLeft || m_wantsRight || m_wantsDown || m_wantsUp)
        {
            m_state = PlayerState.PS_WALKING;
            return;
        }
    }

    void Walking()
    {
        if (m_wantsLeft)
        {
            m_vel.x -= MOVE_ACCEL * Time.fixedDeltaTime;
        }
        else if (m_wantsRight)
        {
            m_vel.x += MOVE_ACCEL * Time.fixedDeltaTime;
        }


        if (m_wantsUp)
        {
            m_vel.y += MOVE_ACCEL * Time.fixedDeltaTime;
        }
        else if (m_wantsDown)
        {
            m_vel.y -= MOVE_ACCEL * Time.fixedDeltaTime;
        }

        bool ismovingY = false;
        bool ismovingX = false;

        if (m_vel.y >= -0.05f && m_vel.y <= 0.05 )
        {
            ismovingY = true;
            m_vel.y = 0;
        }
        if (m_vel.x >= -0.05f && m_vel.x <= 0.05)
        {
            ismovingX = true;
            m_vel.x = 0;
        }

        if(ismovingY && ismovingX)
            m_state = PlayerState.PS_IDLE;

        m_vel.x *= GROUND_FRICTION;
        m_vel.y *= GROUND_FRICTION;
        if (m_shouldSlow)
            m_vel.x *= _PrototypeConfig.SpeedPercentDebuff;

        ApplyVelocity();

      
    }


    void CheckIfCanBeDamaged()
    {
        //can be damaged so no reason to go further
        if (m_canBeDamaged)
            return;

        //simple timer for damage application
        m_canBeDamagedTimer -= Time.deltaTime;
        if(m_canBeDamagedTimer <= 0)
        {
            _PlayerVisual.enabled = true;
            m_canBeDamaged = true;
            m_canBeDamagedTimer = _PrototypeConfig.MaxInvulnerableTime;
            return;
        }

        m_blinkTimer += Time.deltaTime;
        if(m_blinkTimer >= _PrototypeConfig.BlinkRateAfterDamage)
        {
            _PlayerVisual.enabled = !_PlayerVisual.enabled;
            m_blinkTimer = 0;
        }
    }

    void ApplyVelocity()
    {
        /*Vector3 pos = m_rigidBody.transform.position;
        pos.x += m_vel.x;
        pos.y += m_vel.y;
        m_rigidBody.transform.position = pos;
        */

        Vector3 pos = transform.position;
        pos.x += m_vel.x;
        pos.y += m_vel.y;

        transform.position = pos;
    }

    void UpdateInput()
    {
        m_wantsLeft = Input.GetKey(KeyCode.LeftArrow);
        m_wantsRight = Input.GetKey(KeyCode.RightArrow);
        m_wantsUp = Input.GetKey(KeyCode.UpArrow);
        m_wantsDown = Input.GetKey(KeyCode.DownArrow);    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ProcessCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_groundObjects.Remove(collision.gameObject);
    }

    private void ProcessCollision(Collision2D collision)
    {
        m_groundObjects.Remove(collision.gameObject);
        Vector3 pos = m_rigidBody.transform.position;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            //Push back out
            Vector2 impulse = contact.normal * (contact.normalImpulse / Time.fixedDeltaTime);
            pos.x += impulse.x;
            pos.y += impulse.y;

            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                //Hit ground
              
            }
            else
            {
                if ((contact.normal.x > 0 && m_vel.x < 0) || (contact.normal.x < 0 && m_vel.x > 0))
                {
                    m_vel.x = 0;
                }
            }
        }
        m_rigidBody.transform.position = pos;
    }


    public void Damage(float amount)
    {
        if(m_canBeDamaged)
        {
            m_canBeDamaged = false;
            m_canBeDamagedTimer = _PrototypeConfig.MaxInvulnerableTime;
            _HealthSystem.Damage(amount);
        }
        
    }

    public void SlowPlayer(bool shouldBeSlowed)
    {
        m_shouldSlow = shouldBeSlowed;
    }
    private void OnPlayerHealed(object sender, EventArgs e)
    {
        _HPBar.SetActive(true);
        Image fillBar = _HPBar.transform.GetChild(0).GetComponent<Image>();

        fillBar.DOFillAmount(1.0f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => {
            fillBar.DOFade(0.0f, 0.3f);

            _HPBar.GetComponent<Image>().DOFade(0.0f, _PrototypeConfig.MaxInvulnerableTime - 0.5f).OnComplete(() =>
            {
                fillBar.color = m_hpBarColor;
                _HPBar.SetActive(false);
                m_state = PlayerState.PS_IDLE;
            });


        }
        );
    }

    private void OnPlayerDead(object sender, EventArgs e)
    {
        m_canBeDamaged = true;
        m_canBeDamagedTimer = _PrototypeConfig.MaxInvulnerableTime;
        transform.position = m_startPosition;
        _HealthSystem.Heal(_PrototypeConfig.MaxHP);
        m_state = PlayerState.PS_DYING;

    }

    private void OnPlayerDamaged(object sender, EventArgs e)
    {
        _HPBar.SetActive(true);
        _HPBar.GetComponent<Image>().color = Color.gray;
        Image fillBar = _HPBar.transform.GetChild(0).GetComponent<Image>();
        fillBar.color = Color.red;
        
        fillBar.DOFillAmount(_HealthSystem.GetHealthPercent(), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
            fillBar.DOFade(0.0f, _PrototypeConfig.MaxInvulnerableTime - 0.5f);

            _HPBar.GetComponent<Image>().DOFade(0.0f, _PrototypeConfig.MaxInvulnerableTime - 0.5f).OnComplete(() =>
            {
                fillBar.color = m_hpBarColor;
                _HPBar.SetActive(false);
            });

            
        }           
        );


    }

}
