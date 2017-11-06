using UnityEngine;
using System.Collections;

public class PlayerCollisionHandler : MonoBehaviour {

    public PlayerStatus pStatus;
    public PlayerRunner pRunner;

    public PlayerCollisionDetection leftCollision;
    public PlayerCollisionDetection rightCollision;
    public PlayerCollisionDetection frontCollision;
    
    public void ReceiveBlockerCollision(PlayerCollisionDetection colRef)
    {
        if (colRef == leftCollision)
        {
            pRunner.DoBumpLeft();
        }
        else if (colRef == rightCollision)
        {
            pRunner.DoBumpRight();
        }
        else if (colRef == frontCollision)
        {
            pRunner.PlayerHit();
        }
    }
}
