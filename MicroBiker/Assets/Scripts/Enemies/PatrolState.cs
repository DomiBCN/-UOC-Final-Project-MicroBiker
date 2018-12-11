using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    EnemyBug bug;

    public PatrolState(EnemyBug enemy)
    {
        bug = enemy;
    }
    
    public void UpdateState()
    {
        if (bug.waypoints[bug.waypointIndex].transform.position.x < bug.transform.position.x)
        {
            bug.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (bug.waypoints[bug.waypointIndex].transform.position.x > bug.transform.position.x)
        {
            bug.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        bug.transform.position = Vector2.MoveTowards(bug.transform.position, bug.waypoints[bug.waypointIndex].transform.position, bug.speed * Time.deltaTime);
        
        if (Vector2.Distance(bug.transform.position, bug.waypoints[bug.waypointIndex].transform.position) < 0.1f)
        {
            bug.waypointIndex += 1;

            
        }

        if (bug.waypointIndex == bug.waypoints.Length)
        {
            bug.waypointIndex = 0;
        }
    }
}
