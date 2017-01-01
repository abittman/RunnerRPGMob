using UnityEngine;
using System.Collections;

/* This script manages the variations of enemy behaviours in the game.
 * 
 * The logic for an enemy typically runs in the following way:
 *      1   -   Enemy is spawned
 *      2   -   Enemy detects player (cannot think of any passive effects currently)
 *      3   -   Enemy begins to move with player
 *          a   -   A standard form of movement may be for the enemy to follow alongside, above or just in front of the player. Here the enemies run at the player's speed.
 *          b   -   Static: The enemy does not move, but rather the player approaches them. (typically for projectile enemies).
 *          c   -   Similar to a, except when hit they will be "bounced ahead" where the player cannot hit them. After a moment of recovery, they return to their original position. (or if it's b, a position ahead)
 *          d   -   May use a or c, but will also swap lanes to stick to the player.
 *          e   -   May be pushed out of a lane.
 *          f   -   Similar to b, but the enemy does move away from the player slightly.
 *          g   -   A system where the enemy turns and the player has to follow. Better with ranged.
 *          g   -   A more elaborate system, where the enemy actually "turns" with the player. Bit more difficult to do, but haven't figured out drawn out fights yet. Might work ok with melee enemies.
 *                      (Game could detect if enemy is a bit outside of a turn area, and have them run ahead and turn)
 *          h   -   Combination of any of the above (enemy states)
 *      4   -   Enemy attacks player
 *          a   -   Standard melee attack. Must be "with" player in the lane next to, or same lane but above, or just ahead.
 *          b   -   Standard ranged attack. Standard ranged attacks move in the same lane as the enemy.
 *          c   -   Varied ranged attack. These attacks may be fired in lanes that the enemy is not currently in.
 *      5   -   A destroyed enemy is removed and drops items
 *      6   -   An enemy may or may not be respawned if the piece is respawned.
 * */
public class EnemyBehaviour : MonoBehaviour {

    [Header("Self references")]
    public EnemyStatus eStatus;
    public EnemyMovement eMover;
    public EnemyCombat eCombat;
    public EnemyAnimator eAnimatorController;

    [Header("External References")]
    public PlayerEventHandler pEventHandler;
    public PlayerRunner pRunner;
    public PlayerCombat pCombat;

    [Header("Behaviour State")]
    public bool enemyIsActive = false;
    public int startingAIState = 0;
    public int currentAIState = 0;
    public bool respawnable = true;
    public bool hasDied = false;

    [Header("Behaviour variables")]
    public float distanceForDetection = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(enemyIsActive)
        {
            float distToPlayer = Vector3.Distance(transform.position, pRunner.transform.position);

            if(distToPlayer <= distanceForDetection)
            {
                PlayerDetected();
            }
        }
	}

    public void EnemySpawned(BuiltPathPiece currentBPP)
    {
        if (respawnable
            || (!respawnable && !hasDied))
        {
            //Reset enemy
            gameObject.SetActive(true);

            eStatus.ResetEnemyStatus();
            eCombat.ResetCombat();
            eMover.ResetEnemyMovement();

            //May want this to go elsewhere in this script
            eMover.SetupEnemy(currentBPP);

            currentAIState = startingAIState;
            pEventHandler.listeningEnemies.Add(this);
        }
    }

    public void EnemyActivated()
    {
        enemyIsActive = true;
    }

    public void PlayerDetected()
    {
        eAnimatorController.DoRunningAnimation();
        eMover.StartMovingEnemy();
        eCombat.PrepareEnemyCombat();
    }

    public void PlayerChangesLanes(RunningLane newLane)
    {
        Debug.Log("Player changes lane - enemy react");
        if(eMover.laneChangeWithPlayer)
        {
            eMover.EnemySwapLanes(newLane);
        }
    }

    public void PlayerInMeleeRange()
    {
        pCombat.EnemyEngaged(this);
    }

    public void PlayerPassesEnemy()
    {
        pCombat.ClearEnemy(this);
    }

    public void EnemyDespawned()
    {
        enemyIsActive = false;
        pEventHandler.listeningEnemies.Remove(this);
        pCombat.ClearEnemy(this);

        gameObject.SetActive(false);
    }
}
