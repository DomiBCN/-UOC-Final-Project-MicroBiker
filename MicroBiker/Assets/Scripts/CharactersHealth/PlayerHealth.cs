using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : CharacterHealth {

    public Slider healthSlider;
    public float maxHealth;

    public override void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthSlider.value = health;

    }

    public override IEnumerator DamafeFlashEffect(SpriteRenderer motorbikeRenderer, Color hurtColor)
    {
        motorbikeRenderer.color = hurtColor;
        yield return new WaitForSeconds(0.05f);
        motorbikeRenderer.color = Color.white;
    }

    public override void Die(Animator motorbikeAnimator)
    {
        base.Die(motorbikeAnimator);
        AudioManager.instance.Play("MotorbikeExplosion");
    }

    public void DestroyMotorbike()
    {
        GameManager.instance.RestartLevel();
        //Destroy(gameObject);
    }
}
