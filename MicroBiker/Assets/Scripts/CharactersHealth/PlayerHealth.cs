using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharacterHealth {

    public override IEnumerator DamafeFlashEffect(SpriteRenderer motorbikeRenderer, Color hurtColor)
    {
        motorbikeRenderer.color = hurtColor;
        yield return new WaitForSeconds(0.05f);
        motorbikeRenderer.color = Color.white;
    }

    public override void Die(Animator motorbikeAnimator)
    {
        base.Die(motorbikeAnimator);
    }

    public void DestroyMotorbike()
    {
        Destroy(gameObject);
    }
}
