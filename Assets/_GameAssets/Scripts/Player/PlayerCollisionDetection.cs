using UnityEngine;
using System.Collections;

public class PlayerCollisionDetection : MonoBehaviour {

    public PlayerCollisionHandler colHandler;

    void OnTriggerEnter(Collider cols)
    {
        colHandler.ReceiveCollision(this);
    }
}
