﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class SalivaProjectile : MonoBehaviour {

    public ParticleSystem salivaImpact;
    public float damage = 20f;
    public float speed = 20f;

    Rigidbody2D salivaBody;

    private void Awake()
    {
        salivaBody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        salivaBody.velocity = transform.right * speed;
    }

    void SelfDestruction()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(salivaImpact, transform.position, transform.rotation, null);
        if (collision.tag == "Player")
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(45);
            }
        }
        Destroy(gameObject);
    }
}
