using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyBug : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;

    [Header("Shoot")]
    public GameObject spitPoint;
    public GameObject salivaProjectilePrefab;
    public float timeBetweenShoots = 2f;

    [Header("Waypoints")]
    public Transform[] waypoints;
    [HideInInspector] public int waypointIndex = 0;

    [HideInInspector] public GameObject player;

    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState currentState;

    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        patrolState = new PatrolState(this);
        attackState = new AttackState(this);
        currentState = patrolState;
    }

    private void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();

        
    }

    public void ShootSaliva()
    {
        spitPoint.transform.right = player.transform.position - spitPoint.transform.position;
        Instantiate(salivaProjectilePrefab, spitPoint.transform.position, spitPoint.transform.rotation, null);
        AudioManager.instance.Play("BugShot");
    }

    public void SetPatrolState()
    {
        currentState = patrolState;
    }

    public void SetAttackState()
    {
        currentState = attackState;
    }
}
