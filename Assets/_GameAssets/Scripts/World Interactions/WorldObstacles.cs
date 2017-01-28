﻿using UnityEngine;
using System.Collections;

public class WorldObstacles : MonoBehaviour {

    public PlayerStatus pStatus;

    public bool obstacleIsDestructible;

    public float obstacleHitDamage = 25f;

	void SetupObstacle()
    {
    }

    void OnTriggerEnter(Collider cold)
    {
        if(cold.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            pStatus.DamageHealth(obstacleHitDamage);

            if(obstacleIsDestructible)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
