using UnityEngine;
using System.Collections;

public class BuildingInteraction : MonoBehaviour {

    public Transform playerInteractionLocation;
    public RunningLane canTurnFromLane;

    public PlayerRunner pRunner;
    
    public Transform cameraHoldPosition;
    public Transform cameraNewLookAt;

    public Transform exitLocation;

    public UIScreenManager screenMan;
    public string linkedUIID;

    public bool turnPlayerAround = false;

    public void MovePlayerToBuilding()
    {
        if(pRunner.doMove)
        {
            pRunner.pEventHandler.MovePlayerToLocation(playerInteractionLocation.position);
        }
    }
    
    //[TODO] all the below

    public void PlayerInBuilding()
    {

    }

    public void MovePlayerOutOfBuilding()
    {

    }

    public void EnterInteraction()
    {
        //[TODO] Fix again
    }

    public void ExitInteraction()
    {

    }
}
