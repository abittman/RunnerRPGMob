using UnityEngine;
using System.Collections;

public class PlayerCollisionDetection : MonoBehaviour {

    public PlayerCollisionHandler colHandler;

    void OnTriggerEnter(Collider cols)
    {
        if(cols.gameObject.CompareTag("Blocker"))
            colHandler.ReceiveBlockerCollision(this);
    }
}
