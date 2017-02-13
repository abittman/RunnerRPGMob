using UnityEngine;
using System.Collections;

public class BuildingInteraction : MonoBehaviour {

    public Transform playerInteractionLocation;
    public RunningLane canTurnFromLane;

    public PlayerRunner pRunner;
    
    public Transform cameraHoldPosition;
    public Transform cameraNewLookAt;

    public Transform exitLocation;

    public GameObject linkedUIRef;

    public bool turnPlayerAround = false;

    public void PrepareInteraction()
    {
        if(pRunner.doMove)
            pRunner.CanTurnIntoBuildingFromLane(this);
    }

    public void EnterInteraction()
    {
        if (pRunner.doMove)
        {
            pRunner.BuildingEntered(this);
            linkedUIRef.SetActive(true);
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
