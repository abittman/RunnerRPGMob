using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

    public Animator enemyAnimator;

    void Start()
    {
        enemyAnimator.SetBool("doRunning", false);
        enemyAnimator.SetBool("doPreAttack", false);
        enemyAnimator.SetBool("doAttack", false);
    }

    public void DoRunningAnimation()
    {
        enemyAnimator.SetBool("doRunning", true);
        enemyAnimator.SetBool("doPreAttack", false);
        enemyAnimator.SetBool("doAttack", false);
    }

    public void DoAttackPrepAnimation()
    {

        enemyAnimator.SetBool("doRunning", false);
        enemyAnimator.SetBool("doPreAttack", true);
        enemyAnimator.SetBool("doAttack", false);
    }

    public void DoAttackAnimation()
    {

        enemyAnimator.SetBool("doRunning", false);
        enemyAnimator.SetBool("doAttackPrep", false);
        enemyAnimator.SetBool("doAttack", true);
    }
}
