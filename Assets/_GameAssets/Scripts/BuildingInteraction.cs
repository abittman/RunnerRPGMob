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

    public void PrepareInteraction()
    {
        if(pRunner.doMove)
            pRunner.CanTurnIntoBuildingFromLane(this);
    }

    public void MovePlayerToBuilding()
    {
        if(pRunner.doMove)
        {
            pRunner.MovePlayerToLocation(playerInteractionLocation.position);
        }
    }

    public void EnterInteraction()
    {
        if (pRunner.moveIntoBuilding
            || pRunner.movingToPoint)
        {
            pRunner.BuildingEntered(this);
            screenMan.ActivateUIScreen(linkedUIID);
        }
    }

    public void ExitInteraction()
    {
        if (pRunner.doMove)
        {
            pRunner.CanNoLongerTurnIntoBuildingFromLane(this);
        }
    }
}
