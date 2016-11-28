using UnityEngine;
using System.Collections;

public class EnemyRangeTrigger : MonoBehaviour {

    public EnemyMovement enemyMover;

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            enemyMover.StartMovingEnemy();
        }
    }
}
