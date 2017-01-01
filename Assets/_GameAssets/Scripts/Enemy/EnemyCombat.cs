using UnityEngine;
using System.Collections.Generic;

public class EnemyCombat : MonoBehaviour {

    public EnemyBehaviour eBehaviour;

    public float damageAmount = 5f;
    public float preAttackDelay = 0.5f;
    bool inPreAttackDelay = false;
    public float delayBetweenAttacks;
    float nextAttackTimer = 0f;
    public float attackAnimationTime = 0.25f;
    bool inAttackAnimation = false;

    bool combatStarted = false;
    bool canAttack = false;

    [Space]
    public bool attacksOnSides = true;
    public bool attacksOnSameLane = false;
    public bool attacksOnMultipleLanes = false;
    public List<RunningLane> multipleLaneAttackOptions = new List<RunningLane>();
    [Space]
    public bool doesRangedAttack = false;
    public bool doesMeleeAttack = false;
    [Space]
    public GameObject projectilePool;
    public GameObject rangedProjectilePrefab;

    public void ResetCombat()
    {
        combatStarted = false;
        canAttack = false;
        nextAttackTimer = 0f;
    }

	// Update is called once per frame
	void Update ()
    {
	    if(combatStarted)
        {
            nextAttackTimer += Time.deltaTime;

            if (inPreAttackDelay == false)
            {
                //Pre attack delay
                if (nextAttackTimer >= delayBetweenAttacks - preAttackDelay)
                {
                    //Pre attack animation
                    eBehaviour.eAnimatorController.DoAttackPrepAnimation();
                    inPreAttackDelay = true;
                }
            }

            if (inPreAttackDelay)
            {
                if (doesMeleeAttack)
                {
                    //CheckPlayerProximity();
                    if (nextAttackTimer >= delayBetweenAttacks)
                    {
                        EnemyAttacks_Melee();
                    }
                }
                else if (doesRangedAttack)
                {
                    if (nextAttackTimer >= delayBetweenAttacks)
                    {
                        EnemyAttacks_Ranged();
                    }
                }
            }

            if(inAttackAnimation)
            {
                if(nextAttackTimer >= delayBetweenAttacks + attackAnimationTime)
                {
                    nextAttackTimer = 0f;
                    inAttackAnimation = false;
                }
            }
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

        eBehaviour.pCombat.ClearEnemy(eBehaviour);
    }

    void EnemyAttacks_Melee()
    {
        bool attackSuccess = false;
        bool rightAttack = false;
        bool leftAttack = false;
        //Check player location
        switch(eBehaviour.eMover.thisEnemyRunningLane)
        {
            case RunningLane.Left:
                //Player cannot be left

                //Player may be right
                if(eBehaviour.pRunner.currentLane == RunningLane.Mid)
                {
                    attackSuccess = true;
                    rightAttack = true;
                }
                break;
            case RunningLane.Mid:
                //Player may be left
                if (eBehaviour.pRunner.currentLane == RunningLane.Left)
                {
                    attackSuccess = true;
                    leftAttack = true;
                }

                //Player may be right
                if (eBehaviour.pRunner.currentLane == RunningLane.Right)
                {
                    attackSuccess = true;
                    rightAttack = true;
                }
                break;
            case RunningLane.Right:
                //Player may be left
                if (eBehaviour.pRunner.currentLane == RunningLane.Mid)
                {
                    attackSuccess = true;
                    leftAttack = true;
                }

                //Player cannot be right
                break;
        }

        //Do animation
        eBehaviour.eAnimatorController.DoAttackAnimation();

        //Do damage if applicable
        if (attackSuccess)
        {
            eBehaviour.pCombat.pStatus.DamageHealth(damageAmount);
        }

        inAttackAnimation = true;
        inPreAttackDelay = false;
    }

    void EnemyAttacks_Ranged()
    {
        GameObject g = Instantiate(rangedProjectilePrefab, projectilePool.transform) as GameObject;
        g.transform.position = transform.position;
        g.transform.eulerAngles = rangedProjectilePrefab.transform.eulerAngles + transform.eulerAngles;
        g.GetComponent<ProjectileMovement>().pStatus = eBehaviour.pCombat.pStatus;

        inAttackAnimation = true;
        inPreAttackDelay = false;
    }
}
