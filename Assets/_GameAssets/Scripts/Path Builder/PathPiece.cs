using UnityEngine;
using System.Collections.Generic;

public class PathPiece : MonoBehaviour {
    
    //This matters not for town pieces
    public MoveDirection pieceFacingDirection;
    public MoveDirection alternateFacingDirection;

    public bool isTownPiece = false;
    public Transform centrePositionObject;

    public List<ItemPickup> allItems = new List<ItemPickup>();

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void PrepareArea(Vector3 currentPlayerPos)
    {
        Debug.Log("Prepare area " + gameObject.name);
        //Determine direction without much work on my part in other scripts
        Vector3 directionFromAreaToPlayer = transform.position - currentPlayerPos;

        //If more X (east-west) than Z(north-south)
        if(Mathf.Abs(directionFromAreaToPlayer.x) > Mathf.Abs(directionFromAreaToPlayer.z))
        {
            if(directionFromAreaToPlayer.x < 0f)
            {
                pieceFacingDirection = MoveDirection.West;
            }
            else if(directionFromAreaToPlayer.x > 0f)
            {
                pieceFacingDirection = MoveDirection.East;
            }
            else
            {
                Debug.Log("Perfectly aligned???");
            }
        }
        else
        {
            if (directionFromAreaToPlayer.z < 0f)
            {
                pieceFacingDirection = MoveDirection.South;
            }
            else if (directionFromAreaToPlayer.z > 0f)
            {
                pieceFacingDirection = MoveDirection.North;
            }
        }
        
        if(isTownPiece)
        {
            switch (pieceFacingDirection)
            {
                case MoveDirection.East:
                    alternateFacingDirection = MoveDirection.West;
                    break;
                case MoveDirection.West:
                    alternateFacingDirection = MoveDirection.East;
                    break;
                case MoveDirection.South:
                    alternateFacingDirection = MoveDirection.North;
                    break;
                case MoveDirection.North:
                    alternateFacingDirection = MoveDirection.South;
                    break;
            }
        }

        for(int i = 0; i<allItems.Count; i++)
        {
            allItems[i].ActivatePickup();
        }
    }

}
