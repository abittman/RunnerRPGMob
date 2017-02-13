using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[TODO] I probably want this camera to be a child of another camera like object.
//This parent object is what the child looks at.
//The parent object moves accordingly, to either be with the player or on the object to be looked at.
public class MainGameplayCamera : MonoBehaviour {

    public PlayerCameraFocusPoint parentObj;

    public Transform currentLookAt;

    public Vector3 playerFollowPos;

    public Vector3 currentWaitPosition;

    public Vector3 playerStartRotEul;

    bool followPlayer = true;
    bool lookAtObject = false;

	// Use this for initialization
	void Start ()
    {
        playerStartRotEul = transform.localEulerAngles;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (followPlayer)
        {
        }
        else if(lookAtObject)
        {
            transform.position = currentWaitPosition;
            transform.LookAt(currentLookAt);
        }
    }

    public void WatchPlayer()
    {
        followPlayer = true;
        lookAtObject = false;
        
        transform.localPosition = playerFollowPos;
        transform.localEulerAngles = playerStartRotEul;

        parentObj.doFollow = true;
    }

    public void WatchObject(Vector3 waitPos, Transform lookTarget)
    {
        followPlayer = false;
        lookAtObject = true;

        currentWaitPosition = waitPos;
        currentLookAt = lookTarget;

        parentObj.doFollow = false;
    }
}
