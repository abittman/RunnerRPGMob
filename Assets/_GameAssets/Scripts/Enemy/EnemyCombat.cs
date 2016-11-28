﻿using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour {

    public EnemyStatus eStatus;
    public EnemyMovement thisEnemyMover;

    public PlayerCombat pCombat;

    public float damageAmount = 5f;
    public float delayBetweenAttacks;
    float nextAttackTimer = 0f;

    bool combatStarted = false;
    bool canAttack = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(combatStarted)
        {
            CheckPlayerProximity();

            nextAttackTimer += Time.deltaTime;
            if(nextAttackTimer >= delayBetweenAttacks)
            {
                EnemyAttacks();
                nextAttackTimer = 0f;
            }
        }
	}

    void CheckPlayerProximity()
    {
        switch(thisEnemyMover.thisEnemyRunningLane)
        {
            case RunningLane.Left:
                if(pCombat.pRunner.currentLane == RunningLane.Mid)
                {
                    EnemyInCombat();
                    pCombat.leftEnemy = this;
                }
                else
                {
                    EnemyOutOfCombat();
                }
                break;
            case RunningLane.Mid:
                if (pCombat.pRunner.currentLane == RunningLane.Left)
                {
                    EnemyInCombat();
                    pCombat.rightEnemy = this;
                }
                else if (pCombat.pRunner.currentLane == RunningLane.Right)
                {
                    EnemyInCombat();
                    pCombat.leftEnemy = this;
                }
                else
                {
                    EnemyOutOfCombat();
                }
                break;
            case RunningLane.Right:
                if (pCombat.pRunner.currentLane == RunningLane.Mid)
                {
                    EnemyInCombat();
                    pCombat.rightEnemy = this;
                }
                else
                {
                    EnemyOutOfCombat();
                }
                break;
        }
    }

    public void PrepareEnemyCombat()
    {
        combatStarted = true;
    }

    public void EnemyInCombat()
    {
        canAttack = true;
    }

    public void EnemyOutOfCombat()
    {
        canAttack = false;

        if(pCombat.leftEnemy == this)
        {
            pCombat.leftEnemy = null;
        }
        else if(pCombat.rightEnemy == this)
        {
            pCombat.rightEnemy = null;
        }
    }

    void EnemyAttacks()
    {
        if (canAttack)
        {
            pCombat.pStatus.DamageHealth(damageAmount);
        }
        else
        {
            //miss
        }
    }
}