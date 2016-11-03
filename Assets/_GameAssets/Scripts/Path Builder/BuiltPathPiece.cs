using UnityEngine;
using System.Collections;

public class BuiltPathPiece : MonoBehaviour {

    public InteractionTrigger killTriggerArea;
    
    public void ActivateFromPool(Vector3 spawnLocation, Vector3 rotationEuler)
    {
        transform.eulerAngles = rotationEuler;
        transform.position = spawnLocation;
        gameObject.SetActive(true);
    }

    public void DeactivateToPool()
    {
        gameObject.SetActive(false);
    }
}
