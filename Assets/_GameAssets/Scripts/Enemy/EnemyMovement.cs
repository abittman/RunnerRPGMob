using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public EnemyBehaviour eBehaviour;

    public bool isStationary = false;
    public bool laneChangeWithPlayer = false;

    float moveSpeed;
    MoveDirection enemyMoveDirection;
    public RunningLane thisEnemyRunningLane;
    Vector3 centreLanePos;

    public bool canMove = false;

    public bool isFlying = false;

	// Use this for initialization
	void Start ()
    {
        if (!isStationary)
        {
            moveSpeed = eBehaviour.pRunner.moveSpeed;
        }
        else
        {
            moveSpeed = 0f;
        }
	}

    public void ResetEnemyMovement()
    {
        canMove = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(canMove
            && !isStationary)
        {
            MoveEnemy_Forward();
        }
	}

    void MoveEnemy_Forward()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }

    public void EnemySwapLanes(RunningLane newLane)
    {
        Debug.Log("Enemy Swap Lanes");
        //If same as before
        if(newLane == thisEnemyRunningLane)
        {
            return;
        }
        else
        {
            thisEnemyRunningLane = newLane;
            Vector3 newPosition = transform.position;

            switch(thisEnemyRunningLane)
            {
                case RunningLane.Left:
                    switch(enemyMoveDirection)
                    {
                        case MoveDirection.North:
                            newPosition.x = centreLanePos.x - 2f;
                            break;
                        case MoveDirection.East:
                            newPosition.z = centreLanePos.z + 2f;
                            break;
                        case MoveDirection.South:
                            newPosition.x = centreLanePos.x + 2f;
                            break;
                        case MoveDirection.West:
                            newPosition.z = centreLanePos.z - 2f;
                            break;
                    }
                    break;
                case RunningLane.Mid:
                    switch (enemyMoveDirection)
                    {
                        case MoveDirection.North:
                            newPosition.x = centreLanePos.x;
                            break;
                        case MoveDirection.East:
                            newPosition.z = centreLanePos.z;
                            break;
                        case MoveDirection.South:
                            newPosition.x = centreLanePos.x;
                            break;
                        case MoveDirection.West:
                            newPosition.z = centreLanePos.z;
                            break;
                    }
                    break;
                case RunningLane.Right:
                    switch (enemyMoveDirection)
                    {
                        case MoveDirection.North:
                            newPosition.x = centreLanePos.x + 2f;
                            break;
                        case MoveDirection.East:
                            newPosition.z = centreLanePos.z - 2f;
                            break;
                        case MoveDirection.South:
                            newPosition.x = centreLanePos.x - 2f;
                            break;
                        case MoveDirection.West:
                            newPosition.z = centreLanePos.z + 2f;
                            break;
                    }
                    break;
            }

            transform.position = newPosition;
        }
    }

    public void SetupEnemy(BuiltPathPiece startingPathPiece)
    {
        enemyMoveDirection = startingPathPiece.intendedMoveDirection;
        centreLanePos = startingPathPiece.transform.position;

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
    }

    public void StopMovingEnemy()
    {
        canMove = false;
    }
}
