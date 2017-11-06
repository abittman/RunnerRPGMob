using UnityEngine;
using System.Collections;

public class WorldObstacles : MonoBehaviour {

    public PlayerStatus pStatus;

    public bool obstacleIsDestructible;

    public float obstacleHitDamage = 25f;

    public bool obstacleForcesTurn = false;

    public GameObject activeObject;
    public GameObject destroyedObject;

	void SetupObstacle()
    {
    }

    void OnTriggerEnter(Collider cold)
    {
        if(cold.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            //[TODO] Temp disable - pStatus.DamageHealth(obstacleHitDamage);

            if(obstacleForcesTurn)
            {
                pStatus.pRunner.Bump_ForcePlayerToTurn();
            }
            if(obstacleIsDestructible)
            {
                activeObject.SetActive(false);
                destroyedObject.SetActive(true);
            }
        }
    }
}
