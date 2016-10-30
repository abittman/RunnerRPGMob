using UnityEngine;
using System.Collections.Generic;

public enum MoveDirection
{
    None,
    North,
    East,
    South,
    West
}

public class PathBuilder : MonoBehaviour
{
    public GameObject sectionObj;

    public MoveDirection currentMoveDirection;

    public int currentPathProgress = 0;

    public GameObject lastPath;
    public GameObject lastPath_LRAlternative;
    public GameObject currentPath;
    public GameObject nextLeftPath;
    public GameObject nextRightPath;

    public Vector3 objectScale = new Vector3(8f, 0f, 50f);
    //Vector3 halvedScale;
    Vector3 northAdd;
    Vector3 southAdd;
    Vector3 eastAdd;
    Vector3 westAdd;

    public GameObject townObject;
    public List<GameObject> objectsToNotDestroy;
    public Vector3 townScale;

	// Use this for initialization
	void Start ()
    {
        northAdd = new Vector3(objectScale.x / 2f, 0f, objectScale.z / 2f);
        southAdd = new Vector3(-objectScale.x / 2f, 0f, -objectScale.z / 2f);
        eastAdd = new Vector3(objectScale.z / 2f, 0f, objectScale.x / 2f);
        westAdd = new Vector3(-objectScale.z / 2f, 0f, -objectScale.x / 2f);

        currentPathProgress = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateNextPath(GameObject currentPosObjRef)
    {
        bool tookLeft = false;
        bool tookRight = false;
        
        Vector3 currentPos = Vector3.zero;
        Vector3 endLocation = Vector3.zero;
        Vector3 nextRightLocation = Vector3.zero;
        Vector3 nextRightEulerRotation = Vector3.zero;
        Vector3 nextLeftLocation = Vector3.zero;
        Vector3 nextLeftEulerRotation = Vector3.zero;

        if (currentPathProgress > 0)
        {
            Debug.Log("Next on path");

            lastPath = currentPath;

            currentPath = currentPosObjRef;

            if (currentPath == nextLeftPath)
            {
                lastPath_LRAlternative = nextRightPath;
                tookLeft = true;
            }
            else if (currentPath == nextRightPath)
            {
                lastPath_LRAlternative = nextLeftPath;
                tookRight = true;
            }
            
            currentPos = currentPath.transform.position;
            endLocation = currentPos;

            switch (currentMoveDirection)
            {
                case MoveDirection.North:
                    if (tookRight)
                    {
                        //Direction is east
                        currentMoveDirection = MoveDirection.East;
                        endLocation += eastAdd;
                        nextRightLocation = endLocation + southAdd;
                        nextRightEulerRotation = new Vector3(0f, 180f, 0f);
                        nextLeftLocation = endLocation + northAdd;
                        nextLeftEulerRotation = new Vector3(0f, 0f, 0f);
                    }
                    else if (tookLeft)
                    {
                        //Direction is west
                        currentMoveDirection = MoveDirection.West;
                        endLocation += westAdd;
                        nextRightLocation = endLocation + northAdd;
                        nextRightEulerRotation = new Vector3(0f, 0f, 0f);
                        nextLeftLocation = endLocation + southAdd;
                        nextLeftEulerRotation = new Vector3(0f, 180f, 0f);
                    }
                    break;
                case MoveDirection.East:
                    if (tookRight)
                    {
                        //Direction is east
                        currentMoveDirection = MoveDirection.South;
                        endLocation += southAdd;
                        nextRightLocation = endLocation + westAdd;
                        nextRightEulerRotation = new Vector3(0f, 270f, 0f);
                        nextLeftLocation = endLocation + eastAdd;
                        nextLeftEulerRotation = new Vector3(0f, 90f, 0f);
                    }
                    else if (tookLeft)
                    {
                        //Direction is west
                        currentMoveDirection = MoveDirection.North;
                        endLocation += northAdd;
                        nextRightLocation = endLocation + eastAdd;
                        nextRightEulerRotation = new Vector3(0f, 90f, 0f);
                        nextLeftLocation = endLocation + westAdd;
                        nextLeftEulerRotation = new Vector3(0f, 270f, 0f);
                    }
                    break;
                case MoveDirection.South:
                    if (tookRight)
                    {
                        //Direction is east
                        currentMoveDirection = MoveDirection.West;
                        endLocation += westAdd;
                        nextRightLocation = endLocation + northAdd;
                        nextRightEulerRotation = new Vector3(0f, 0f, 0f);
                        nextLeftLocation = endLocation + southAdd;
                        nextLeftEulerRotation = new Vector3(0f, 180f, 0f);
                    }
                    else if (tookLeft)
                    {
                        //Direction is west
                        currentMoveDirection = MoveDirection.East;
                        endLocation += eastAdd;
                        nextRightLocation = endLocation + southAdd;
                        nextRightEulerRotation = new Vector3(0f, 180f, 0f);
                        nextLeftLocation = endLocation + northAdd;
                        nextLeftEulerRotation = new Vector3(0f, 0f, 0f);
                    }
                    break;
                case MoveDirection.West:
                    if (tookRight)
                    {
                        //Direction is east
                        currentMoveDirection = MoveDirection.North;
                        endLocation += northAdd;
                        nextRightLocation = endLocation + eastAdd;
                        nextRightEulerRotation = new Vector3(0f, 90f, 0f);
                        nextLeftLocation = endLocation + westAdd;
                        nextLeftEulerRotation = new Vector3(0f, 270f, 0f);
                    }
                    else if (tookLeft)
                    {
                        //Direction is west
                        currentMoveDirection = MoveDirection.South;
                        endLocation += southAdd;
                        nextRightLocation = endLocation + westAdd;
                        nextRightEulerRotation = new Vector3(0f, 270f, 0f);
                        nextLeftLocation = endLocation + eastAdd;
                        nextLeftEulerRotation = new Vector3(0f, 90f, 0f);
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("First path");
            currentPos = currentPath.transform.position;
            endLocation = currentPos;

            switch (currentMoveDirection)
            {
                case MoveDirection.North:
                        //Direction is north
                        endLocation += northAdd;
                        nextRightLocation = endLocation + eastAdd;
                        nextRightEulerRotation = new Vector3(0f, 90f, 0f);
                        nextLeftLocation = endLocation + westAdd;
                        nextLeftEulerRotation = new Vector3(0f, 270f, 0f);
                    break;

                case MoveDirection.East:
                    //Direction is north
                    endLocation += eastAdd;
                    nextRightLocation = endLocation + southAdd;
                    nextRightEulerRotation = new Vector3(0f, 180f, 0f);
                    nextLeftLocation = endLocation + northAdd;
                    nextLeftEulerRotation = new Vector3(0f, 0f, 0f);
                    break;

                case MoveDirection.South:
                    //Direction is north
                    endLocation += southAdd;
                    nextRightLocation = endLocation + westAdd;
                    nextRightEulerRotation = new Vector3(0f, 270f, 0f);
                    nextLeftLocation = endLocation + eastAdd;
                    nextLeftEulerRotation = new Vector3(0f, 90f, 0f);
                    break;

                case MoveDirection.West:
                    //Direction is north
                    endLocation.x += westAdd.x;
                    nextRightLocation = endLocation + northAdd;
                    nextRightEulerRotation = new Vector3(0f, 0f, 0f);
                    nextLeftLocation = endLocation + southAdd;
                    nextLeftEulerRotation = new Vector3(0f, 180f, 0f);
                    break;
            }
        }
        //Spawn left path
        GameObject leftG = Instantiate(sectionObj, transform) as GameObject;
        Debug.Log("Left path at " + nextLeftLocation + " | " + nextLeftEulerRotation);
        leftG.transform.position = nextLeftLocation;
        leftG.transform.eulerAngles = nextLeftEulerRotation;
        leftG.GetComponentInChildren<PathTriggerArea>().pathBuilder = this;
        nextLeftPath = leftG;

        //Spawn right path
        GameObject rightG = Instantiate(sectionObj, transform) as GameObject;
        Debug.Log("Right path at " + nextRightLocation + " | " + nextRightEulerRotation);

        rightG.transform.position = nextRightLocation;
        rightG.transform.eulerAngles = nextRightEulerRotation;
        rightG.GetComponentInChildren<PathTriggerArea>().pathBuilder = this;
        nextRightPath = rightG;

        currentPathProgress++;
    }

    public void PathStarted(GameObject startPathRef, MoveDirection moveDirection)
    {
        currentPath = startPathRef;
        currentMoveDirection = moveDirection;
        Debug.Log(currentPath);
    }
}
