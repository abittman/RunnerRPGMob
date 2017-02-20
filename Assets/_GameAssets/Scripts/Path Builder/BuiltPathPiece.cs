using UnityEngine;
using System.Collections.Generic;

public class BuiltPathPiece : MonoBehaviour {

    public InteractionTrigger killTriggerArea;

    //public TurnTriggerArea connectedTurnTriggerArea;

    public PathPiece connectedPathPiece;

    [Header("Path Building Variables")]
    [Tooltip("Must include turn area also")]
    public Vector3 objectScale = new Vector3(8f, 0f, 80f);

    //Showonly
    public MoveDirection intendedMoveDirection;

    public List<PathTurnConditions> exitLocations = new List<PathTurnConditions>();

    [Header("Linked elements")]
    //public EnemyMovement linkedEnemy;
    public List<EnemyBehaviour> linkedEnemies = new List<EnemyBehaviour>();
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
        if (connectedPathPiece.isTownPiece == false)
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
}
