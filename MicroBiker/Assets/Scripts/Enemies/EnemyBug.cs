using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyBug : MonoBehaviour
{

    [Header("Shoot")]
    public GameObject spitPoint;
    public GameObject salivaProjectilePrefab;
    public float timeBetweenShoots = 2f;
    
    float shootTimer = 0;
    bool shooting;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shootTimer < timeBetweenShoots && Time.timeScale != 0)
        {
            shootTimer += Time.fixedDeltaTime;
        }
        if (shootTimer >= timeBetweenShoots && !shooting)
        {
            //shoot
            shooting = true;
            ShootSaliva();
            shootTimer = 0;
        }
    }

    void ShootSaliva()
    {
        Instantiate(salivaProjectilePrefab, spitPoint.transform.position, spitPoint.transform.rotation, null);
        shooting = false;
    }
    
}
