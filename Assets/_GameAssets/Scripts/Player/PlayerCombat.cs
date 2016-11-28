using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour {

    public PlayerStatus pStatus;
    public PlayerRunner pRunner;
    public PlayerAnimationController pAniController;

    public EnemyCombat leftEnemy;
    public EnemyCombat rightEnemy;
    public EnemyCombat jumpEnemy;
    public EnemyCombat slideEnemy;

    public float attackStrength = 25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoLeftAttack()
    {
        if(leftEnemy != null)
        {
            leftEnemy.eStatus.DamageEnemy(attackStrength);
        }
    }

    public void DoRightAttack()
    {
        if (rightEnemy != null)
        {
            rightEnemy.eStatus.DamageEnemy(attackStrength);
        }

    }

    public void DoJumpAttack()
    {
        if (jumpEnemy != null)
        {
            jumpEnemy.eStatus.DamageEnemy(attackStrength);
        }
    }

    public void DoSlideAttack()
    {
        if (slideEnemy != null)
        {
            slideEnemy.eStatus.DamageEnemy(attackStrength);
        }

    }
}
