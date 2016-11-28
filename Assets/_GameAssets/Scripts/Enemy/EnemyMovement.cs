using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public PlayerRunner pRunner;
    public EnemyCombat eCombat;

    float moveSpeed;
    MoveDirection enemyMoveDirection;
    public RunningLane thisEnemyRunningLane;

    public bool canMove = false;

	// Use this for initialization
	void Start ()
    {
        moveSpeed = pRunner.moveSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(canMove)
        {
            MoveEnemy();
        }
	}

    void MoveEnemy()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }

    public void SetupEnemy(PathPiece startingPathPiece)
    {
        enemyMoveDirection = startingPathPiece.pieceFacingDirection;

        switch(enemyMoveDirection)
        {
            case MoveDirection.North:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case MoveDirection.East:
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
                break;
            case MoveDirection.South:
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                break;
            case MoveDirection.West:
                transform.eulerAngles = new Vector3(0f, 270f, 0f);
                break;
        }
    }

    public void StartMovingEnemy()
    {
        canMove = true;

        eCombat.PrepareEnemyCombat();
    }

    public void StopMovingEnemy()
    {
        canMove = false;
    }
}
