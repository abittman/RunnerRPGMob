using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour {

    public PlayerStatus pStatus;
    public PlayerRunner pRunner;
    public PlayerAnimationController pAniController;
    public PlayerFXController pFXController;

    public bool inCombat = false;

    public List<EnemyBehaviour> engagedEnemies = new List<EnemyBehaviour>();

    public EnemyBehaviour frontEnemy;
    //public EnemyCombat leftEnemy;
    //public EnemyCombat rightEnemy;
    //public EnemyCombat jumpEnemy;
    //public EnemyCombat slideEnemy;
    
    public float attackStrength = 25f;

    //Same as player runner timers
    float horizontalAttackDelay;
    float jumpAttackDelay;
    float slideAttackDelay;
    float frontAttackDelay = .25f;
    float delayTime = 0f;
    float currentAttackDelayTimer = 0f;

    public bool isAttacking = false;

    void Start()
    {
        horizontalAttackDelay = pRunner.laneChangeTransitionTime;
    }

    void Update()
    {
        if(isAttacking)
        {
            currentAttackDelayTimer += Time.deltaTime;
            if(currentAttackDelayTimer >= delayTime)
            {
                currentAttackDelayTimer = 0f;
                StopAttack();
            }
        }
    }

    public void EnemyEngaged(EnemyBehaviour enemyRef)
    {
        if(engagedEnemies.Exists(x => x == enemyRef) == false)
        {
            engagedEnemies.Add(enemyRef);
        }
        inCombat = true;
    }

    public void ClearEnemy(EnemyBehaviour enemyRef)
    {
        engagedEnemies.Remove(enemyRef);

        if(enemyRef == frontEnemy)
        {
            frontEnemy = null;
        }

        if(engagedEnemies.Count == 0)
        {
            inCombat = false;
        }
    }

    public void StopAttack()
    {
        pAniController.StraightRunAnimation();
        pFXController.StopAttackFX();
    }

    public bool CanDoFrontAttack()
    {
        bool enemyInFront = false;

        if(inCombat)
        {
            for(int i = 0; i < engagedEnemies.Count; i++)
            {
                //If in same lane
                if(engagedEnemies[i].eMover.thisEnemyRunningLane == pRunner.currentLane)
                {
                    //And if distance is close enough
                    float distToEnemy = Vector3.Distance(transform.position, engagedEnemies[i].transform.position);
                    if (distToEnemy <= 3f)
                    {
                        frontEnemy = engagedEnemies[i];     //Save enemy ref temporarily. Need to clear
                        enemyInFront = true;
                    }
                }
            }
        }
        return enemyInFront;
    }

    public void DoFrontAttack()
    {
        Debug.Log("Do front attack");
        isAttacking = true;
        delayTime = frontAttackDelay;
        frontEnemy.eStatus.DamageEnemy(attackStrength);

        pAniController.FrontAttackAnimation();
        pFXController.DoAttackFX();
    }

    public void DoLeftAttack()
    {
        /*if(leftEnemy != null)
        {
            isAttacking = true;
            delayTime = horizontalAttackDelay;

            leftEnemy.eBehaviour.eStatus.DamageEnemy(attackStrength);
            
            pAniController.LeftAttackAnimation();
            pFXController.DoAttackFX();
        }*/
    }

    public void DoRightAttack()
    {
        /*
        if (rightEnemy != null)
        {
            isAttacking = true;
            delayTime = horizontalAttackDelay;

            rightEnemy.eBehaviour.eStatus.DamageEnemy(attackStrength);

            pAniController.RightAttackAnimation();
            pFXController.DoAttackFX();
        }
        */
    }

    public void DoJumpAttack()
    {
        /*
        if (jumpEnemy != null)
        {
            isAttacking = true;
            delayTime = jumpAttackDelay;

            jumpEnemy.eBehaviour.eStatus.DamageEnemy(attackStrength);

            pAniController.JumpAttackAnimation();
            pFXController.DoAttackFX();
        }
        */
    }

    public void DoSlideAttack()
    {
        /*
        if (slideEnemy != null)
        {
            isAttacking = true;
            delayTime = slideAttackDelay;

            slideEnemy.eBehaviour.eStatus.DamageEnemy(attackStrength);

            pAniController.SlideAttackAnimation();
            pFXController.DoAttackFX();
        }
        */
    }
}
