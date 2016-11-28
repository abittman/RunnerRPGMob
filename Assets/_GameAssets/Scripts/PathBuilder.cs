using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MoveDirection
{
    North = 1,
    East = 2,
    South = 3,
    West = 4
}

public class PathBuilder : MonoBehaviour
{
    public GameObject sectionObj;
    public List<BuiltPathPiece> forestSectionPoolObjects = new List<BuiltPathPiece>();

    public MoveDirection currentMoveDirection;

    public int currentPathProgress = 0;

    public int minPathLength = 4;
    public int maxPathLength = 7;
    int pathLength = 5;

    bool lastIsTownPart = false;
    /*
    public GameObject lastPath;
    public GameObject lastPath_LRAlternative;
    public GameObject currentPath;
    public GameObject nextLeftPath;
    public GameObject nextRightPath;
    */

    public BuiltPathPiece lastPathPiece;
    public BuiltPathPiece lastPathPiece_LRAlternative;
    public BuiltPathPiece currentPathPiece;
    public BuiltPathPiece nextLeftPathPiece;
    public BuiltPathPiece nextRightPathPiece;

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

    MoveDirection lastTownDirection = MoveDirection.North;

	// Use this for initialization
	void Start ()
    {
        northAdd = new Vector3(objectScale.x / 2f, 0f, objectScale.z / 2f);
        southAdd = new Vector3(-objectScale.x / 2f, 0f, -objectScale.z / 2f);
        eastAdd = new Vector3(objectScale.z / 2f, 0f, objectScale.x / 2f);
        westAdd = new Vector3(-objectScale.z / 2f, 0f, -objectScale.x / 2f);

        currentPathProgress = 0;

        DeactivateAllPoolPieces();
    }
	
	void DeactivateAllPoolPieces()
    {
        for(int i = 0; i<forestSectionPoolObjects.Count; i++)
        {
            forestSectionPoolObjects[i].DeactivateToPool();
        }
    }

    public void CreateNextPath(BuiltPathPiece currentPathPieceRef)
    {
        Debug.Log("Create next path at " + currentPathPieceRef.gameObject.name);
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

            lastPathPiece = currentPathPiece;

            currentPathPiece = currentPathPieceRef;

            if (currentPathPiece == nextLeftPathPiece)
            {
                lastPathPiece_LRAlternative = nextRightPathPiece;
                nextRightPathPiece = null;
                tookLeft = true;
            }
            else if (currentPathPiece == nextRightPathPiece)
            {
                lastPathPiece_LRAlternative = nextLeftPathPiece;
                nextLeftPathPiece = null;
                tookRight = true;
            }

            StartCoroutine(DeactivatePastPathsOnDelay());

            currentPos = currentPathPiece.transform.position;
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
            currentPos = currentPathPiece.transform.position;
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
            //Randomise selection of path piece
            nextLeftPathPiece = RandomiseNextPathPiece();
            nextLeftPathPiece.ActivateFromPool(nextLeftLocation, nextLeftEulerRotation);
            currentPathPiece.connectedTriggerArea.proceduralConnectedPieces.Add(nextLeftPathPiece.connectedPathPiece);

            nextRightPathPiece = RandomiseNextPathPiece();
            nextRightPathPiece.ActivateFromPool(nextRightLocation, nextRightEulerRotation);
            currentPathPiece.connectedTriggerArea.proceduralConnectedPieces.Add(nextRightPathPiece.connectedPathPiece);

            currentPathProgress++;
        }
        else
        {
            //Move Town - need to figure out the maths here
            ResetTown(endLocation);
        }
    }

    public void PathStarted(BuiltPathPiece startPathPieceRef, MoveDirection moveDirection)
    {
        Debug.Log("Start path at direction " + moveDirection);
        lastIsTownPart = true;
        currentPathPiece = startPathPieceRef;
        currentMoveDirection = moveDirection;

        int randomLength = Random.Range(minPathLength, maxPathLength + 1);
        pathLength = randomLength;
    }

    IEnumerator DeactivatePastPathsOnDelay()
    {
        yield return new WaitForSeconds(1f);

        if (!lastIsTownPart && currentPathProgress > 0)
        {
            lastPathPiece.DeactivateToPool();
        }
        else if(lastIsTownPart)
        {
            townObject.SetActive(false);
            lastIsTownPart = false;
        }

        lastPathPiece_LRAlternative.DeactivateToPool();
    }

    BuiltPathPiece RandomiseNextPathPiece()
    {
        BuiltPathPiece nextPathPiece = null;

        bool newPiece = false;
        do
        {
            //Randomise number - more to do with this calculation some other time
            int randomR = Random.Range(0, forestSectionPoolObjects.Count);

            //Check it was not a piece previously/recently used
            if(forestSectionPoolObjects[randomR] != currentPathPiece
                && forestSectionPoolObjects[randomR] != lastPathPiece
                && forestSectionPoolObjects[randomR] != lastPathPiece_LRAlternative
                && forestSectionPoolObjects[randomR] != nextRightPathPiece
                && forestSectionPoolObjects[randomR] != nextLeftPathPiece)
            {
                newPiece = true;
                nextPathPiece = forestSectionPoolObjects[randomR];
            }

        } while (newPiece == false);

        return nextPathPiece;
    }

    void ResetTown(Vector3 pathEndLocation)
    {
        Debug.Log("Reset town coming in from " + exitOrientation);
        //Current move direction
        Vector3 townToExitOffset = Vector3.zero;

        MoveDirection townOrientation = MoveDirection.North;

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

        if(townObject.transform.eulerAngles.y == 0f)
        {
            townOrientation = MoveDirection.North;
        }
        else if(townObject.transform.eulerAngles.y == 90f)
        {
            townOrientation = MoveDirection.East;
        }
        else if(townObject.transform.eulerAngles.y == 180f)
        {
            townOrientation = MoveDirection.South;
        }
        else if(townObject.transform.eulerAngles.y == 270f)
        {
            townOrientation = MoveDirection.West;
        }

        int townRotationDiff = (int)townOrientation - (int)lastTownDirection;
        if (townRotationDiff < 1)
        {
            townRotationDiff += 4;
        }

        for (int i = 0; i<entrancePathAreas.Count; i++)
        {
            int moveEnumToInt = 0;

            moveEnumToInt = (int)entrancePathAreas[i].thisMoveDirection + townRotationDiff;
            if(moveEnumToInt > 4)
            {
                moveEnumToInt -= 4;
            }
            else if(moveEnumToInt < 1)
            {
                moveEnumToInt += 4;
            }
            entrancePathAreas[i].thisMoveDirection = (MoveDirection)moveEnumToInt;

            Debug.Log("Exit " + entrancePathAreas[i].parentPathPiece.name + " becomes " + entrancePathAreas[i].thisMoveDirection);
        }

        lastTownDirection = townOrientation;

        townToExitOffset = townReturnPoint.position - townObject.transform.position;

        //Move Town Position
        Debug.Log("Path end location : " + pathEndLocation + "Exit offset : " + townToExitOffset);
        townObject.transform.position = pathEndLocation - townToExitOffset;
        townObject.SetActive(true);

        //Reset path
        currentPathProgress = 0;
    }
}
