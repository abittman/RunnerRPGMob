using UnityEngine;
using System.Collections;

/* Trigger to check for "melee range"
 * Typically for the player to attack
 * */
public class EnemyRangeTrigger : MonoBehaviour {

    public EnemyBehaviour eBehaviour;

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            eBehaviour.PlayerInMeleeRange();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            eBehaviour.PlayerPassesEnemy();
        }
    }
}
