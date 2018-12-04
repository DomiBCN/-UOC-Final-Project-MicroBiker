using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class CharacterHealth : MonoBehaviour {

    Animator characterAnimator;
    SpriteRenderer spriteRenderer;
    public Color hurtColor;
    public float health = 150f;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Start()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        StartCoroutine(DamafeFlashEffect(spriteRenderer, hurtColor));
        health -= damage;
        if(health <= 0)
        {
            Die(characterAnimator);
        }
    }

    public virtual void Die(Animator characterAnimator)
    {
        characterAnimator.SetTrigger("Die");
    }
    
    public virtual IEnumerator DamafeFlashEffect(SpriteRenderer spriteRenderer, Color hurtColor)
    {
        yield return null;
    }
}
