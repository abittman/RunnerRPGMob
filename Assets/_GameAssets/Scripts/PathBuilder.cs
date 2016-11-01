using UnityEngine;
using System.Collections;
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

    public int minPathLength = 4;
    public int maxPathLength = 7;
    int pathLength = 5;

    bool lastIsTownPart = false;
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
    public Transform townReturnPoint;
    public List<GameObject> objectsToNotDestroy;
    //public Vector3 townScale;
    public MoveDirection exitOrientation = MoveDirection.South;

    public List<PathTriggerArea> entrancePathAreas = new List<PathTriggerArea>();

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
        Debug.Log("Create next path at " + currentPosObjRef.name);
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

            StartCoroutine(DestroyPastPathsOnDelay());

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

        if (currentPathProgress < pathLength)
        {
            //Spawn left path
            GameObject leftG = Instantiate(sectionObj, transform) as GameObject;
            //Debug.Log("Left path at " + nextLeftLocation + " | " + nextLeftEulerRotation);
            leftG.transform.position = nextLeftLocation;
            leftG.transform.eulerAngles = nextLeftEulerRotation;
            leftG.GetComponentInChildren<PathTriggerArea>().pathBuilder = this;
            nextLeftPath = leftG;

            //Spawn right path
            GameObject rightG = Instantiate(sectionObj, transform) as GameObject;
            //Debug.Log("Right path at " + nextRightLocation + " | " + nextRightEulerRotation);

            rightG.transform.position = nextRightLocation;
            rightG.transform.eulerAngles = nextRightEulerRotation;
            rightG.GetComponentInChildren<PathTriggerArea>().pathBuilder = this;
            nextRightPath = rightG;

            currentPathProgress++;
        }
        else
        {
            //Move Town - need to figure out the maths here
            ResetTown(endLocation);
        }
    }

    public void PathStarted(GameObject startPathRef, MoveDirection moveDirection)
    {
        Debug.Log("Start path at direction " + moveDirection);
        lastIsTownPart = true;
        currentPath = startPathRef;
        currentMoveDirection = moveDirection;

        int randomLength = Random.Range(minPathLength, maxPathLength + 1);
        pathLength = randomLength;
    }

    IEnumerator DestroyPastPathsOnDelay()
    {
        yield return new WaitForSeconds(1f);

        if (!lastIsTownPart && currentPathProgress > 0)
        {
            Destroy(lastPath);
            townObject.SetActive(false);
        }
        else
        {
            lastIsTownPart = false;
        }

        Destroy(lastPath_LRAlternative);
    }

    void ResetTown(Vector3 pathEndLocation)
    {
        //Debug.Log("Reset town");
        //Current move direction
        Vector3 townToExitOffset = Vector3.zero;

        float rotAmount = 0f;
        switch(exitOrientation)
        {
            case MoveDirection.North:
                rotAmount = 0;
                break;
            case MoveDirection.East:
                rotAmount = 90;
                break;
            case MoveDirection.South:
                rotAmount = 180f;
                break;
            case MoveDirection.West:
                rotAmount = 270;
                break;
        }
        switch(currentMoveDirection)
        {
            case MoveDirection.North:
                townObject.transform.eulerAngles = new Vector3(0f, 0f + rotAmount, 0f);
                break;
            case MoveDirection.East:
                townObject.transform.eulerAngles = new Vector3(0f, 90f + rotAmount, 0f);
                break;
            case MoveDirection.South:
                townObject.transform.eulerAngles = new Vector3(0f, 180f + rotAmount, 0f);
                break;
            case MoveDirection.West:
                townObject.transform.eulerAngles = new Vector3(0f, 270f + rotAmount, 0f);
                break;
        }

        for(int i = 0; i<entrancePathAreas.Count; i++)
        {
            int moveEnumToInt = 0;

            switch (exitOrientation)
            {
                case MoveDirection.North:
                    //Do nothing
                    break;
                case MoveDirection.East:
                    moveEnumToInt = (int)entrancePathAreas[i].thisMoveDirection + 1;
                    if(moveEnumToInt > 4)
                    {
                        moveEnumToInt -= 3;
                    }
                    entrancePathAreas[i].thisMoveDirection = (MoveDirection)moveEnumToInt;
                    break;
                case MoveDirection.South:
                    moveEnumToInt = (int)entrancePathAreas[i].thisMoveDirection + 2;
                    if (moveEnumToInt > 4)
                    {
                        moveEnumToInt -= 3;
                    }
                    entrancePathAreas[i].thisMoveDirection = (MoveDirection)moveEnumToInt;
                    break;

                case MoveDirection.West:
                    moveEnumToInt = (int)entrancePathAreas[i].thisMoveDirection + 3;
                    if (moveEnumToInt > 4)
                    {
                        moveEnumToInt -= 3;
                    }
                    entrancePathAreas[i].thisMoveDirection = (MoveDirection)moveEnumToInt;
                    break;
            }
        }

        townToExitOffset = townReturnPoint.position - townObject.transform.position;

        //Move Town Position
        Debug.Log("Path end location : " + pathEndLocation + "Exit offset : " + townToExitOffset);
        townObject.transform.position = pathEndLocation - townToExitOffset;
        townObject.SetActive(true);

        //Reset path
        currentPathProgress = 0;
    }
}
