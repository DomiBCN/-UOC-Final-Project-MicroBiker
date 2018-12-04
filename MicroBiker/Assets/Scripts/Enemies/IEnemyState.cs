using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState{
    void UpdateState();
    void GotoPatrolState();
    void GoToChaseState();
    void GoToAttackState();
    void OnImpact();
    void OnTriggerEnter(Collider col);
    void OnTriggerStay(Collider col);
    void OnTriggerExit(Collider col);
}
