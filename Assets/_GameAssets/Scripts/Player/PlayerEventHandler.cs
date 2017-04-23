using UnityEngine;
using System.Collections.Generic;

public class PlayerEventHandler : MonoBehaviour {

    [Header("References")]
    public PlayerRunner pRunner;

    [Header("Event Details")]
    //Building travel
    BuildingInteraction currentBuildingRef;

    public bool movingToPoint = false;
    Vector3 goalLocation;

    public void MovePlayerToLocation(Vector3 waitPoint)
    {
        movingToPoint = true;
        pRunner.StopRunner();
        goalLocation = waitPoint;
    }

    public void MovePlayerToLocation(Vector3 waitPoint, BuildingInteraction bInteraction)
    {
        movingToPoint = true;
        pRunner.StopRunner();
        goalLocation = waitPoint;
        currentBuildingRef = bInteraction;
    }

    void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, goalLocation, Time.deltaTime);
        if (Vector3.Distance(transform.position, goalLocation) < 1f)
        {
            movingToPoint = false;
        }
    }

    public void ReturnPlayerControl()
    {
        //May need to handle "turning around" or something of the like
        pRunner.StartRunner();
    }
}
