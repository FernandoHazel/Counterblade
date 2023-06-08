using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [Header("Enemy navigation")]
    public Enemy_SO enemyData;
    [SerializeField] LevelWaypoints waypointsData;
    
    [Tooltip("if the enemy reach this distance to the waypoint it go to the next one")]
    [SerializeField] float maxProximity;
    public int currentWp = 0;
    public float distanceToWP;
    [Header("Enemy HP information")]
    public float hp;
    public GameObject HPBar;
    public GameObject currentHPBar;
    [SerializeField] Animator animator;
    GameManager gm;

    private void Start() {
        GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
        hp = enemyData.MaxHP;
        currentHPBar.GetComponent<RectTransform>().sizeDelta = HPBar.GetComponent<RectTransform>().sizeDelta;
        animator = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();

        //Find the way to follow
        int randomWay = UnityEngine.Random.Range(0,2);
        waypointsData = transform.parent.gameObject.transform.GetChild(randomWay).gameObject.GetComponent<LevelWaypoints>(); //this will depend on the enemy generator
        //Debug.Log($"following way {waypointsData.wayID}");
    }
    void Update()
    {
        Move();
    }
    private void Move()
    {
        if (currentWp != waypointsData.wpList.Count)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, waypointsData.wpList[currentWp].transform.position, enemyData.speed * Time.deltaTime);
            distanceToWP = Vector2.Distance(this.transform.position, waypointsData.wpList[currentWp].transform.position);
            
            if (Vector2.Distance(this.transform.position, waypointsData.wpList[currentWp].transform.position) < maxProximity)
            {
                currentWp++;
            }
        }
    }

    public void getDamaged(float minDamage, float maxDamage)
    {
        hp -= Random.Range(minDamage, maxDamage);
        UpdateHPBar(hp);
        if (hp <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    public void Die()
    {
        //Change this for an object pooling
        Destroy(gameObject);
        GameManager.enemyCount--;
    }

    void UpdateHPBar(float hp)
    {
        float percentage = hp/enemyData.MaxHP;
        currentHPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(HPBar.GetComponent<RectTransform>().sizeDelta.x * percentage, HPBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("GameController"))
        {
            Destroy(gameObject);
        }
    }
}
