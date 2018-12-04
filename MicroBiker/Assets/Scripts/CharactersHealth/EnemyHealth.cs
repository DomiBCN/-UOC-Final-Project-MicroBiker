using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : CharacterHealth {

    [Header("Dead")]
    public Color deadStainColor;
    public ParticleSystem splashParticle;
    public GameObject stain;
    public List<Sprite> stains;
    SpriteRenderer stainRenderer;

    public override IEnumerator DamafeFlashEffect(SpriteRenderer bugRenderer, Color hurtColor)
    {
        bugRenderer.color = hurtColor;
        yield return new WaitForSeconds(0.05f);
        bugRenderer.color = Color.white;
    }

    public override void Die(Animator bugAnimator)
    {
        bugAnimator.SetTrigger("Die");
    }

    public void DestroyBug()
    {
        Destroy(gameObject);
    }

    void DeathEffects()
    {
        //Splash effect
        Instantiate(splashParticle, transform.position, transform.rotation, null);
        //Stain on wall
        GameObject stainSprite = Instantiate(stain, transform.position, transform.rotation, null);
        stainRenderer = stainSprite.GetComponent<SpriteRenderer>();
        stainRenderer.sprite = stains[Random.Range(0, stains.Count - 1)];
        stainRenderer.color = deadStainColor;
    }
}
