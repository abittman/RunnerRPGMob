using UnityEngine;
using System.Collections;

public class BuiltPathPiece : MonoBehaviour {

    public InteractionTrigger killTriggerArea;

    public TurnTriggerArea connectedTriggerArea;

    public PathPiece connectedPathPiece;

    public EnemyMovement linkedEnemy;
    
    public void ActivateFromPool(Vector3 spawnLocation, Vector3 rotationEuler)
    {
        transform.eulerAngles = rotationEuler;
        transform.position = spawnLocation;
        gameObject.SetActive(true);
        if(linkedEnemy != null)
        {
            linkedEnemy.SetupEnemy(connectedPathPiece);
        }
    }

    public void DeactivateToPool()
    {
        gameObject.SetActive(false);
        connectedTriggerArea.proceduralConnectedPieces.Clear();
    }
}
