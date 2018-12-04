using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Laser : MonoBehaviour
{

    public ParticleSystem laserImpact;
    public float damage = 20f;
    public float speed = 20f;

    Rigidbody2D laserBody;

    private void Awake()
    {
        laserBody = GetComponent<Rigidbody2D>();
        SelfDestruction();
    }

    // Use this for initialization
    void Start()
    {
        laserBody.velocity = transform.right * speed;
    }

    void SelfDestruction()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(laserImpact, transform.position, transform.rotation, null);
        if(collision.tag == "Enemy")
        {
            EnemyHealth bug = collision.GetComponent<EnemyHealth>();
            if(bug != null)
            {
                bug.TakeDamage(45);
            }
        }
        Destroy(gameObject);
    }
}
