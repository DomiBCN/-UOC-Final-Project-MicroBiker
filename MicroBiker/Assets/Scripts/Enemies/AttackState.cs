using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyBug bug;
    float shootTimer = 0;
    bool shooting;

    public AttackState(EnemyBug enemy)
    {
        bug = enemy;
    }
    
    public void UpdateState()
    {
        if (bug.player.transform.position.x < bug.transform.position.x)
        {
            bug.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (bug.player.transform.position.x > bug.transform.position.x)
        {
            bug.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (shootTimer < bug.timeBetweenShoots && Time.timeScale != 0)
        {
            shootTimer += Time.fixedDeltaTime;
        }
        if (shootTimer >= bug.timeBetweenShoots)
        {
            //shoot
            bug.ShootSaliva();
            shootTimer = 0;
        }
    }
}
