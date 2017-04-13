using UnityEngine;
using System.Collections.Generic;

public class BuiltPathPiece : MonoBehaviour {

    public InteractionTrigger killTriggerArea;
    public PathTriggerArea pathTriggerArea;

    //public PathPiece connectedPathPiece;

    [Header("Path Building Variables")]
    [Tooltip("Must include turn area also")]
    //public Vector3 objectScale = new Vector3(8f, 0f, 80f);        [TODO] use path turn conditions instead

    //Showonly
    public MoveDirection intendedMoveDirection;

    public MoveDirection pieceFacingDirection;
    //public MoveDirection alternateFacingDirection;

    public bool isTownPiece = false;

    public List<PathTurnConditions> exitLocations = new List<PathTurnConditions>();

    [Header("Linked elements")]
    public List<ResourceNode> resourceNodes = new List<ResourceNode>();

    public bool isActive = false;

    public void ActivateFromPool()
    {
        gameObject.SetActive(true);
        isActive = true;
    }

    public void ActivateFromPool(Vector3 spawnLocation, Vector3 rotationEuler)
    {
        transform.eulerAngles = rotationEuler;
        transform.position = spawnLocation;
        gameObject.SetActive(true);
        isActive = true;
        //Node activation
        
    }

    public void PlayerOnThisPiece()
    {
        for(int i = 0; i < resourceNodes.Count; i++)
        {
            resourceNodes[i].SetupNode();
        }
    }

    public void DeactivateToPool()
    {
        if (isTownPiece == false)
        {
            gameObject.SetActive(false);
            for (int i = 0; i < exitLocations.Count; i++)
            {
                exitLocations[i].connectedTurnTriggerArea.proceduralConnectedPieces.Clear();
            }
        }
        else
        {
            Debug.Log("Cannot deactivate town piece by itself");
        }

        isActive = false;
    }


    public void PrepareArea(Vector3 currentPlayerPos)
    {
        Debug.Log("Prepare area " + gameObject.name);
        //Determine direction without much work on my part in other scripts
        Vector3 directionFromAreaToPlayer = transform.position - currentPlayerPos;

        //If more X (east-west) than Z(north-south)
        if (Mathf.Abs(directionFromAreaToPlayer.x) > Mathf.Abs(directionFromAreaToPlayer.z))
        {
            if (directionFromAreaToPlayer.x < 0f)
            {
                pieceFacingDirection = MoveDirection.West;
            }
            else if (directionFromAreaToPlayer.x > 0f)
            {
                pieceFacingDirection = MoveDirection.East;
            }
            else
            {
                Debug.Log("Perfectly aligned???");
            }
        }
        else
        {
            if (directionFromAreaToPlayer.z < 0f)
            {
                pieceFacingDirection = MoveDirection.South;
            }
            else if (directionFromAreaToPlayer.z > 0f)
            {
                pieceFacingDirection = MoveDirection.North;
            }
        }

        /*
        if (isTownPiece)
        {
            switch (pieceFacingDirection)
            {
                case MoveDirection.East:
                    alternateFacingDirection = MoveDirection.West;
                    break;
                case MoveDirection.West:
                    alternateFacingDirection = MoveDirection.East;
                    break;
                case MoveDirection.South:
                    alternateFacingDirection = MoveDirection.North;
                    break;
                case MoveDirection.North:
                    alternateFacingDirection = MoveDirection.South;
                    break;
            }
        }
        */

        //Activate nodes?
    }

}
