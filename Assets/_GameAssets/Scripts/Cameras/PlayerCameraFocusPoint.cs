using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent object of main camera
public class PlayerCameraFocusPoint : MonoBehaviour {

    public Transform playerObj;
    public bool doFollow = true;
	
	// Update is called once per frame
	void Update ()
    {
        if (doFollow)
        {
            transform.position = playerObj.position;
            transform.rotation = playerObj.rotation;
        }
	}
}
