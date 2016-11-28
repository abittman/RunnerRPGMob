using UnityEngine;
using System.Collections;

public class PlayerCollisionHandler : MonoBehaviour {

    public PlayerStatus pStatus;
    public PlayerRunner pRunner;

    public PlayerCollisionDetection leftCollision;
    public PlayerCollisionDetection rightCollision;
    public PlayerCollisionDetection frontCollision;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReceiveCollision(PlayerCollisionDetection colRef)
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
            //Kill player
            //pStatus.PlayerRunFails();
        }
    }
}
