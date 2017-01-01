using UnityEngine;
using System.Collections.Generic;

public class BuiltPathPiece : MonoBehaviour {

    public InteractionTrigger killTriggerArea;

    public TurnTriggerArea connectedTriggerArea;

    public PathPiece connectedPathPiece;


    [Tooltip("Must include turn area also")]
    public Vector3 objectScale = new Vector3(8f, 0f, 80f);

    //Showonly
    public MoveDirection intendedMoveDirection;

    [Header("Linked elements")]
    //public EnemyMovement linkedEnemy;
    public List<EnemyBehaviour> linkedEnemies = new List<EnemyBehaviour>();
    public List<ItemPickup> itemPickups = new List<ItemPickup>();

    public void ActivateFromPool(Vector3 spawnLocation, Vector3 rotationEuler)
    {
        transform.eulerAngles = rotationEuler;
        transform.position = spawnLocation;
        gameObject.SetActive(true);

        for(int i = 0; i < itemPickups.Count; i++)
        {
            itemPickups[i].ActivatePickup();
        }

        for(int i = 0; i < linkedEnemies.Count; i++)
        {
            linkedEnemies[i].EnemySpawned(this);
        }
    }

    public void PlayerOnThisPiece()
    {
        for(int i = 0; i < linkedEnemies.Count; i++)
        {
            linkedEnemies[i].EnemyActivated();
        }
    }

    public void DeactivateToPool()
    {
        if (connectedPathPiece.isTownPiece == false)
        {
            gameObject.SetActive(false);
            connectedTriggerArea.proceduralConnectedPieces.Clear();
        }
        else
        {
            Debug.Log("Cannot deactivate town piece by itself");
        }
    }
}
