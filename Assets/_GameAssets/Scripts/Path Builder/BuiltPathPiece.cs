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


    public void PrepareArea(Vector3 turnAreaPos, MoveDirection playerMoveDir)
    {
        Debug.Log("Prepare area " + gameObject.name);
        //Determine direction without much work on my part in other scripts
        Vector3 directionFromTurnAreaToPiece = transform.position - turnAreaPos;
        MoveDirection directionAssumingNorth = MoveDirection.North;
        //If more X (east-west) than Z(north-south)
        if (Mathf.Abs(directionFromTurnAreaToPiece.x) > Mathf.Abs(directionFromTurnAreaToPiece.z))
        {
            if (directionFromTurnAreaToPiece.x < 0f)
            {
                directionAssumingNorth = MoveDirection.West;
            }
            else if (directionFromTurnAreaToPiece.x > 0f)
            {
                directionAssumingNorth = MoveDirection.East;
            }
            else
            {
                Debug.Log("Perfectly aligned???");
            }
        }
        else
        {
            if (directionFromTurnAreaToPiece.z < 0f)
            {
                directionAssumingNorth = MoveDirection.South;
            }
            else if (directionFromTurnAreaToPiece.z > 0f)
            {
                directionAssumingNorth = MoveDirection.North;
            }
        }

        pieceFacingDirection = directionAssumingNorth;
    }

}
