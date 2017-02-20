using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MoveDirection
{
    North = 1,
    East = 2,
    South = 3,
    West = 4,
    None = 50
}

public enum AreaTypes
{
    None,
    Town,
    Forest,
    Deep_Forest,
    Cave,
    Cave_Dungeon,
    Coast,
    Forest_Camp,
    Autumn_Grove
}

public class PathBuilder : MonoBehaviour
{
    /*
    [Header("References")]
    public PathPoolManager poolMan;
    //public List<PathedArea> allPathedAreas = new List<PathedArea>();
    //public List<BuiltPathPiece> forestSectionPoolObjects = new List<BuiltPathPiece>();
    //PathedArea currentPathedArea;
    public AreaTypes startingAreaType;

    [Space]
    public MoveDirection currentMoveDirection;

    public int currentPathProgress = 0;
    int pathLength = 5;
    
    bool lastIsTownPart = false;

    //Last pieces, to clear
    public BuiltPathPiece lastPathPiece;

    //[TO-DO] Need to make this a list of alternatives, given exit location plurals
    //MAYBE have an exit location disable itself after passed?
    public BuiltPathPiece lastPathPiece_LRAlternative;
    //Current piece that player is on
    public BuiltPathPiece currentPathPiece;

    Vector3 northAdd;
    Vector3 southAdd;
    Vector3 eastAdd;
    Vector3 westAdd;

    public GameObject townObject;
    public List<GameObject> objectsToNotDestroy;

    MoveDirection lastTownDirection = MoveDirection.North;

    bool hasReturnedToTown = true;

    //public List<BuiltPathPiece> allBuiltPoolObjects = new List<BuiltPathPiece>();

	// Use this for initialization
	void Start ()
    {
        currentPathProgress = 0;
        
        poolMan.DeactivateAllPoolPieces();

        CreateFirstPathConnections();
    }

    void CreateFirstPathConnections()
    {
        //Create first path connections based on current area...
        poolMan.SetStartingArea(startingAreaType);

        //Spawn starting pieces for each connected path area
        for(int i = 0; i < poolMan.currentPathPoolGroup.thisPathedArea.allTurnChangeConditions.Count; i++)
        {
            //For a connected area
            AreaTypes connectedAreaType = poolMan.currentPathPoolGroup.thisPathedArea.allTurnChangeConditions[i].areaThisLeadsTo;

            //Select a left - if applicable
            BuiltPathPiece firstLeftBPP = poolMan.GetValidPathPieceForArea(connectedAreaType);

            //Set intended move direction
            firstLeftBPP.intendedMoveDirection = GetLeftMoveDirection(allPathedAreas[i].entranceDirection);

            //Move to the entrance location
            Vector3 nextLeftPos = GetNextPlacementPosition(firstLeftBPP.intendedMoveDirection,
                                                            firstLeftBPP[i].pathTownEntrancePiece.exitLocations[0].pathTurnLocation.position,
                                                            firstLeftBPP);
            //Rotate accordingly
            //Activate piece
            //Connect piece to others

            //Select a right - if applicable
        }

        for (int i = 0; i < allPathedAreas.Count; i++)
        {
            Debug.Log("Build start of " + allPathedAreas[i].thisAreaType + "area");
            currentPathedArea = allPathedAreas[i];

            //Left Piece
            BuiltPathPiece leftPiece = RandomiseNextPathPiece(currentPathedArea);
            leftPiece.intendedMoveDirection = GetLeftMoveDirection(allPathedAreas[i].entranceDirection);

            Vector3 nextLeftPos = GetNextPlacementPosition(leftPiece.intendedMoveDirection,
                                                            allPathedAreas[i].pathTownEntrancePiece.exitLocations[0].pathTurnLocation.position,
                                                            leftPiece);

            //Rotation
            Vector3 leftEulRot = new Vector3(0f, ((int)leftPiece.intendedMoveDirection - 1) * 90f, 0f);

            //Creation
            leftPiece.ActivateFromPool(nextLeftPos, leftEulRot);
            allPathedAreas[i].pathFirstLeftPiece = leftPiece;

            switch(leftPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.northPiece = leftPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.eastPiece = leftPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.southPiece = leftPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.westPiece = leftPiece.connectedPathPiece;
                    break;
            }

            //Right Piece
            BuiltPathPiece rightPiece = RandomiseNextPathPiece(currentPathedArea);
            rightPiece.intendedMoveDirection = GetRightMoveDirection(allPathedAreas[i].entranceDirection);

            Vector3 nextRightPos = GetNextPlacementPosition(rightPiece.intendedMoveDirection,
                                                            allPathedAreas[i].pathTownEntrancePiece.exitLocations[0].pathTurnLocation.position,
                                                            rightPiece);
            //Rotation
            Vector3 rightEulRot = new Vector3(0f, ((int)rightPiece.intendedMoveDirection - 1) * 90f, 0f);

            //Creation
            rightPiece.ActivateFromPool(nextRightPos, rightEulRot);
            allPathedAreas[i].pathFirstRightPiece = rightPiece;

            switch (rightPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.northPiece = rightPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.eastPiece = rightPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.southPiece = rightPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    currentPathedArea.pathTownEntrancePiece.exitLocations[0].connectedTurnTriggerArea.westPiece = rightPiece.connectedPathPiece;
                    break;
            }
        }

        currentPathedArea = null;
    }

    public void CreateNextPath(BuiltPathPiece currentPathPieceRef)
    {
        Debug.Log("Checking " + currentPathPieceRef);
        if(currentPathProgress == 0)
        {
            PathStarted(currentPathPieceRef);
        }

        if (currentPathedArea != null)
        {
            //Check if player has "exited"
            for (int i = 0; i < currentPathedArea.allTurnChangeConditions.Count; i++)
            {
                if (currentPathPieceRef == currentPathedArea.allTurnChangeConditions[i].connectorPieceToNextArea)
                {
                    //Check if it's the town area first
                    if (currentPathedArea.allTurnChangeConditions[i].areaThisLeadsTo == AreaTypes.Town)
                    {
                        Debug.Log("Exiting to town");
                        currentPathedArea = null;
                        currentPathProgress = 0;
                        //CreateFirstPathConnections();
                        break;
                    }
                    else
                    {
                        Debug.Log("New area");
                        //Player has moved to new area
                        currentPathProgress = 1;
                        currentPathedArea = allPathedAreas.Find(x => x.thisAreaType == currentPathedArea.allTurnChangeConditions[i].areaThisLeadsTo);
                        //Would need to send area also? -> PathStarted(currentPathPieceRef);
                        break;
                    }
                }
            }
        }

        //If current area is still not null (i.e. not back in town)
        if(currentPathedArea != null)
        { 
            Debug.Log("Create next path from " + currentPathPieceRef.gameObject.name);

            Vector3 currentPos = Vector3.zero;
            Vector3 endLocation = Vector3.zero;

            
            //MoveDirection nextRightMoveDir = MoveDirection.None;
            //MoveDirection nextLeftMoveDir = MoveDirection.None;

            if (currentPathProgress == 0)
            {
                Debug.Log("First path");
                //Set left and right to left and right
                currentPathPieceRef.exitLocations[0].nextLeftPathPiece = currentPathedArea.pathFirstLeftPiece;
                currentPathPieceRef.exitLocations[0].nextRightPathPiece = currentPathedArea.pathFirstRightPiece;
            }
            else
            {
                Debug.Log("Follow on path");
                //lastPathPiece = currentPathPiece;
                
                //Deactivate extensions based on current path
                for (int i = 0; i < currentPathPiece.exitLocations.Count; i++)
                {
                    Debug.Log("Check currentpath exit locations");
                    if(currentPathPieceRef != currentPathPiece.exitLocations[i].nextLeftPathPiece)
                    {
                        //lastPathPiece_LRAlternative = currentPathPiece.exitLocations[i].nextLeftPathPiece;
                        allBuiltPoolObjects.Remove(currentPathPiece.exitLocations[i].nextLeftPathPiece);
                        for (int j = 0; j < currentPathPiece.exitLocations[i].nextLeftPathPiece.exitLocations.Count; j++)
                        {
                            Debug.Log("Remove extra extensions");
                            allBuiltPoolObjects.Remove(currentPathPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextLeftPathPiece);

                            allBuiltPoolObjects.Remove(currentPathPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextRightPathPiece);
                        }

                        //currentPathPiece.exitLocations[i].nextLeftPathPiece.DeactivateToPool(true);
                        //currentPathPiece.exitLocations[i].nextLeftPathPiece = null;
                    }
                    else
                    {
                        Debug.Log("LR Alt is right");
                        lastPathPiece_LRAlternative = currentPathPiece.exitLocations[i].nextRightPathPiece;
                    }

                    if (currentPathPieceRef != currentPathPiece.exitLocations[i].nextRightPathPiece)
                    {
                        //lastPathPiece_LRAlternative = currentPathPiece.exitLocations[i].nextRightPathPiece;
                        allBuiltPoolObjects.Remove(currentPathPiece.exitLocations[i].nextRightPathPiece);
                        for (int j = 0; j < currentPathPiece.exitLocations[i].nextRightPathPiece.exitLocations.Count; j++)
                        {
                            Debug.Log("Remove extra extensions");

                            allBuiltPoolObjects.Remove(currentPathPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextLeftPathPiece);

                            allBuiltPoolObjects.Remove(currentPathPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextLeftPathPiece);
                        }

                        //currentPathPiece.exitLocations[i].nextLeftPathPiece.DeactivateToPool(true);
                        //currentPathPiece.exitLocations[i].nextRightPathPiece = null;
                    }
                    else
                    {
                        Debug.Log("LR Alt is left");
                        lastPathPiece_LRAlternative = currentPathPiece.exitLocations[i].nextLeftPathPiece;
                    }
                }

                //If the current path piece ref was a left turn
                if (currentPathPiece.exitLocations.Exists(x => x.nextLeftPathPiece == currentPathPieceRef))
                {
                    Debug.Log("Went left");
                    currentMoveDirection = GetLeftMoveDirection(currentMoveDirection);
                    lastPathPiece = currentPathPiece;
                    currentPathPiece = currentPathPieceRef;
                }
                //else if it was a right turn
                else if (currentPathPiece.exitLocations.Exists(x => x.nextRightPathPiece == currentPathPieceRef))
                {
                    Debug.Log("Went right");
                    currentMoveDirection = GetRightMoveDirection(currentMoveDirection);
                    lastPathPiece = currentPathPiece;
                    currentPathPiece = currentPathPieceRef;
                }
                //else if it isn't valid
                else
                {
                    Debug.Log("Invalid path taken");
                }
            }
            
            SpawnExtensions();
            currentPathPiece.PlayerOnThisPiece();

            if (lastPathPiece != null)
            {
                if ((lastPathPiece.connectedPathPiece.isTownPiece == true)
                    && currentPathPiece.connectedPathPiece.isTownPiece == false)
                {
                    lastIsTownPart = true;
                }
            }
            
            currentPathProgress++;
        }

        StartCoroutine(DeactivatePastPathsOnDelay());
    }

    public void PathStarted(BuiltPathPiece startPathPieceRef)
    {
        Debug.Log("Start path at direction " + startPathPieceRef + " going " + startPathPieceRef.connectedPathPiece.pieceFacingDirection);

        if (hasReturnedToTown == false)
        {
            for (int i = 0; i < allPathedAreas.Count; i++)
            {
                if (allPathedAreas[i].pathTownExitPiece == startPathPieceRef)
                {
                    hasReturnedToTown = true;
                    Debug.Log("Returned to town");
                    DeactivateAllPoolPieces();
                    CreateFirstPathConnections();
                }
            }
        }
        else
        {
            Debug.Log("Check if start piece");
            for (int i = 0; i < allPathedAreas.Count; i++)
            {
                if (allPathedAreas[i].pathTownEntrancePiece == startPathPieceRef)
                {
                    currentPathedArea = allPathedAreas[i];
                    break;
                }
            }

            if (currentPathedArea != null)
            {
                Debug.Log("Start path");
                hasReturnedToTown = false;
                currentPathPiece = startPathPieceRef;
                currentMoveDirection = startPathPieceRef.connectedPathPiece.pieceFacingDirection;
                currentPathProgress = 0;

                int randomLength = Random.Range(currentPathedArea.minPathLength, currentPathedArea.maxPathLength + 1);
                pathLength = randomLength;
            }
        }
    }

    Vector3 GetNextPlacementPosition(MoveDirection mDir, Vector3 displacePoint, BuiltPathPiece nextPathPiece)
    {
        Vector3 nextPos = Vector3.zero;

        Vector3 objectScale = nextPathPiece.objectScale;
        northAdd = new Vector3(0f, objectScale.y / 2f, objectScale.x / 2f + objectScale.z / 2f);
        southAdd = new Vector3(0f, objectScale.y / 2f, -(objectScale.x / 2f + objectScale.z / 2f));
        eastAdd = new Vector3(objectScale.x / 2f + objectScale.z / 2f, objectScale.y / 2f, 0f);
        westAdd = new Vector3(-(objectScale.x / 2f + objectScale.z / 2f), objectScale.y / 2f, 0f);

        switch (mDir)
        {
            case MoveDirection.North:
                nextPos = displacePoint + northAdd;
                break;
            case MoveDirection.East:
                nextPos = displacePoint + eastAdd;
                break;
            case MoveDirection.South:
                nextPos = displacePoint + southAdd;
                break;
            case MoveDirection.West:
                nextPos = displacePoint + westAdd;
                break;
            default:
                Debug.Log("???");
                break;
        }
        return nextPos;
    }

    void SpawnExtensions()
    {
        //Begin by assuming that the next location most likely begins in the same "area"/"pool"
        PathedArea nextLeftPathArea = currentPathedArea;
        PathedArea nextRightPathArea = currentPathedArea;
        
        //For each exit location's path piece, which should already exist by here...
        for (int i = 0; i < currentPathPiece.exitLocations.Count; i++)
        {
            //First check if we're moving to a new area
            for (int k = 0; k < currentPathedArea.allTurnChangeConditions.Count; k++)
            {
                if(currentPathedArea.allTurnChangeConditions[k].connectorPieceToNextArea 
                        == currentPathPiece.exitLocations[i].nextLeftPathPiece)
                {
                    //If it's the town, do different logic
                    if (currentPathedArea.allTurnChangeConditions[k].areaThisLeadsTo == AreaTypes.Town)
                    {
                        nextLeftPathArea = null;
                        //ResetTown function?
                    }
                    else
                    {
                        nextLeftPathArea = allPathedAreas.Find(x => x.thisAreaType == currentPathedArea.allTurnChangeConditions[k].areaThisLeadsTo);
                    }
                }
                else if (currentPathedArea.allTurnChangeConditions[k].connectorPieceToNextArea
                        == currentPathPiece.exitLocations[i].nextRightPathPiece)
                {
                    //If it's the town, do different logic
                    if (currentPathedArea.allTurnChangeConditions[k].areaThisLeadsTo == AreaTypes.Town)
                    {
                        nextRightPathArea = null;
                        //ResetTown function?
                    }
                    else
                    {
                        nextRightPathArea = allPathedAreas.Find(x => x.thisAreaType == currentPathedArea.allTurnChangeConditions[k].areaThisLeadsTo);
                    }
                }
            }

            //Need an else check here - if town area, don't do the below?

            //Spawn an extension for each exit location of this left "exit" path piece
            if (currentPathPiece.exitLocations[i].nextLeftPathPiece != null)
            {
                Debug.Log("Spawn left locations");
                BuiltPathPiece leftPieceRef = currentPathPiece.exitLocations[i].nextLeftPathPiece;

                for (int j = 0; j < leftPieceRef.exitLocations.Count; j++)
                {
                    if (leftPieceRef.exitLocations[j].canDoLeft)
                    {
                        //Spawn a left
                        BuiltPathPiece leftBPPExtension = RandomiseNextPathPiece(currentPathedArea);
                        //Determine move direction
                        leftBPPExtension.intendedMoveDirection = GetLeftMoveDirection(leftPieceRef.intendedMoveDirection);
                        //Get exit position for exit location J left piece
                        Vector3 nextLeftEndPos = GetNextPlacementPosition(leftBPPExtension.intendedMoveDirection,
                                                                        leftPieceRef.exitLocations[j].pathTurnLocation.position,
                                                                        leftBPPExtension);

                        Vector3 nextLeftExtensionRot = new Vector3(0f, ((int)leftBPPExtension.intendedMoveDirection - 1) * 90f, 0f);

                        leftBPPExtension.ActivateFromPool(nextLeftEndPos, nextLeftExtensionRot);

                        leftPieceRef.exitLocations[j].nextLeftPathPiece = leftBPPExtension;

                        switch (leftBPPExtension.intendedMoveDirection)
                        {
                            case MoveDirection.North:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.northPiece = leftBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.East:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.eastPiece = leftBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.South:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.southPiece = leftBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.West:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.westPiece = leftBPPExtension.connectedPathPiece;
                                break;
                        }
                    }

                    if (leftPieceRef.exitLocations[j].canDoRight)
                    {
                        //Spawn a right
                        BuiltPathPiece rightBPPExtension = RandomiseNextPathPiece(currentPathedArea);
                        //Determine move direction
                        rightBPPExtension.intendedMoveDirection = GetRightMoveDirection(leftPieceRef.intendedMoveDirection);
                        //Get exit position for exit location J left piece
                        Vector3 nextRightEndPos = GetNextPlacementPosition(rightBPPExtension.intendedMoveDirection,
                                                                        leftPieceRef.exitLocations[j].pathTurnLocation.position,
                                                                        rightBPPExtension);


                        Vector3 nextRightExtensionRot = new Vector3(0f, ((int)rightBPPExtension.intendedMoveDirection - 1) * 90f, 0f);

                        rightBPPExtension.ActivateFromPool(nextRightEndPos, nextRightExtensionRot);

                        leftPieceRef.exitLocations[j].nextRightPathPiece = rightBPPExtension;

                        switch (rightBPPExtension.intendedMoveDirection)
                        {
                            case MoveDirection.North:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.northPiece = rightBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.East:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.eastPiece = rightBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.South:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.southPiece = rightBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.West:
                                leftPieceRef.exitLocations[j].connectedTurnTriggerArea.westPiece = rightBPPExtension.connectedPathPiece;
                                break;
                        }
                    }

                }
            }

            //Spawn an extension for each exit location of this right "exit" path piece
            if (currentPathPiece.exitLocations[i].nextRightPathPiece != null)
            {
                Debug.Log("Spawn right locations");

                BuiltPathPiece rightPieceRef = currentPathPiece.exitLocations[i].nextRightPathPiece;

                for (int j = 0; j < rightPieceRef.exitLocations.Count; j++)
                {
                    if (rightPieceRef.exitLocations[j].canDoLeft)
                    {
                        //Spawn a left
                        BuiltPathPiece leftBPPExtension = RandomiseNextPathPiece(currentPathedArea);
                        //Determine move direction
                        leftBPPExtension.intendedMoveDirection = GetLeftMoveDirection(rightPieceRef.intendedMoveDirection);
                        //Get exit position for exit location J left piece
                        Vector3 nextLeftEndPos = GetNextPlacementPosition(leftBPPExtension.intendedMoveDirection,
                                                                        rightPieceRef.exitLocations[j].pathTurnLocation.position,
                                                                        leftBPPExtension);

                        Vector3 nextLeftExtensionRot = new Vector3(0f, ((int)leftBPPExtension.intendedMoveDirection - 1) * 90f, 0f);

                        leftBPPExtension.ActivateFromPool(nextLeftEndPos, nextLeftExtensionRot);

                        rightPieceRef.exitLocations[j].nextLeftPathPiece = leftBPPExtension;

                        switch (leftBPPExtension.intendedMoveDirection)
                        {
                            case MoveDirection.North:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.northPiece = leftBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.East:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.eastPiece = leftBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.South:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.southPiece = leftBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.West:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.westPiece = leftBPPExtension.connectedPathPiece;
                                break;
                        }
                    }

                    if (rightPieceRef.exitLocations[j].canDoRight)
                    {
                        //Spawn a right
                        BuiltPathPiece rightBPPExtension = RandomiseNextPathPiece(currentPathedArea);
                        //Determine move direction
                        rightBPPExtension.intendedMoveDirection = GetRightMoveDirection(rightPieceRef.intendedMoveDirection);
                        //Get exit position for exit location J left piece
                        Vector3 nextRightEndPos = GetNextPlacementPosition(rightBPPExtension.intendedMoveDirection,
                                                                        rightPieceRef.exitLocations[j].pathTurnLocation.position,
                                                                        rightBPPExtension);


                        Vector3 nextRightExtensionRot = new Vector3(0f, ((int)rightBPPExtension.intendedMoveDirection - 1) * 90f, 0f);

                        rightBPPExtension.ActivateFromPool(nextRightEndPos, nextRightExtensionRot);

                        rightPieceRef.exitLocations[j].nextRightPathPiece = rightBPPExtension;

                        switch (rightBPPExtension.intendedMoveDirection)
                        {
                            case MoveDirection.North:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.northPiece = rightBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.East:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.eastPiece = rightBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.South:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.southPiece = rightBPPExtension.connectedPathPiece;
                                break;
                            case MoveDirection.West:
                                rightPieceRef.exitLocations[j].connectedTurnTriggerArea.westPiece = rightBPPExtension.connectedPathPiece;
                                break;
                        }
                    }

                }
            }
        }
    }

    IEnumerator DeactivatePastPathsOnDelay()
    {
        yield return new WaitForSeconds(1f);

        //Due to delay, progress would have been 0, but will have since updated to ++
        if(currentPathProgress == 1)
        {
            //Clear original spawns in town area
            for(int i = 0; i < allPathedAreas.Count; i++)
            {
                if(allPathedAreas[i] != currentPathedArea)
                {
                    allBuiltPoolObjects.Remove(allPathedAreas[i].pathFirstLeftPiece);
                    allPathedAreas[i].pathFirstLeftPiece.DeactivateToPool(false);
                    allBuiltPoolObjects.Remove(allPathedAreas[i].pathFirstRightPiece);
                    allPathedAreas[i].pathFirstRightPiece.DeactivateToPool(false);
                }
            }
        }
        if (lastPathPiece != null)
        {
            allBuiltPoolObjects.Remove(lastPathPiece);
            lastPathPiece.DeactivateToPool(false);
            Debug.Log("Deactivate last pieces");
        }
        if(lastPathPiece_LRAlternative != null)
        {
            allBuiltPoolObjects.Remove(lastPathPiece_LRAlternative);
            lastPathPiece_LRAlternative.DeactivateToPool(true);
            Debug.Log("Deactivate last LR piece");
        }

        if (lastIsTownPart)
        {
            Debug.Log("Clear town");
            townObject.SetActive(false);
            lastIsTownPart = false;
        }
    }

    //[TODO] Separate builder into separate pool managers (for each area) to hold, get and set objects
    //          and the builder that requests and sets them up
    BuiltPathPiece RandomiseNextPathPiece(PathedArea pathArea)
    {
        BuiltPathPiece nextPathPiece = null;

        bool newPiece = false;

        bool inactivePieceExists = false;
        List<BuiltPathPiece> availableBPP = new List<BuiltPathPiece>();
        //Do a check to see if pieces are available, else exit
        for(int i = 0; i < pathArea.thisAreaPoolObjects.Count; i++)
        {
            if (pathArea.thisAreaPoolObjects[i].gameObject.activeInHierarchy == false)
            {
                inactivePieceExists = true;
                availableBPP.Add(pathArea.thisAreaPoolObjects[i]);
            }
        }

        Debug.Log("Available BPP length = " + availableBPP.Count);

        if (inactivePieceExists == true)
        {
            do
            {
                //Randomise number - more to do with this calculation some other time
                int randomR = Random.Range(0, availableBPP.Count);

                //Check it was not a piece previously/recently used
                if (allBuiltPoolObjects.Exists(x => x == availableBPP[randomR]) == false)
                {
                    bool wasChangePiece = false;
                    for (int i = 0; i < pathArea.allTurnChangeConditions.Count; i++)
                    {
                        if (availableBPP[randomR] == pathArea.allTurnChangeConditions[i].connectorPieceToNextArea)
                        {
                            wasChangePiece = true;

                            if (pathArea.allTurnChangeConditions[i].minPosToOffer <= currentPathProgress)
                            {
                                newPiece = true;
                                nextPathPiece = availableBPP[randomR];
                            }
                        }
                    }
                    if (wasChangePiece == false)
                    {
                        newPiece = true;
                        nextPathPiece = availableBPP[randomR];
                    }
                }

            } while (newPiece == false);
        }
        else
        {
            Debug.Log("No available pieces");
        }
        allBuiltPoolObjects.Add(nextPathPiece);

        return nextPathPiece;
    }

    MoveDirection GetLeftMoveDirection(MoveDirection moveDir)
    {
        MoveDirection leftMoveDir;

        int val = (int)moveDir;

        val--;

        if(val < 1)
        {
            val = 4;
        }

        leftMoveDir = (MoveDirection)val;

        return leftMoveDir;
    }

    MoveDirection GetRightMoveDirection(MoveDirection moveDir)
    {
        MoveDirection rightMoveDir;

        int val = (int)moveDir;

        val++;

        if (val > 4)
        {
            val = 1;
        }

        rightMoveDir = (MoveDirection)val;

        return rightMoveDir;
    }

    void ResetTown(Vector3 pathEndLocation, MoveDirection playerExitMoveDir)
    {
        Debug.Log("Reset town coming in from " + currentPathedArea.exitDirection);
        //Current move direction
        Vector3 townToExitOffset = Vector3.zero;

        MoveDirection townOrientation = MoveDirection.North;
        int townOrientationNum = (int)townOrientation;

        int oppositeOrientationNum = townOrientationNum + 2;

        //South exit + south movement + north town = 0 rotation
        //South exit + north movement + north town = 180 rotation
        
        //North - north = 0, north - east = -1, north - south = -2, north - west = -3
        //West - north = 3, west - east = 2, west - south = 1, west - west = 0
        int exitToMoveDiffNum = (int)currentPathedArea.exitDirection - (int)playerExitMoveDir;

        float rotAmount = 0f;
        switch(exitToMoveDiffNum)
        {
            case 3:
            case -1:
                rotAmount = 90f;
                break;
            case 2:
            case -2:
                rotAmount = 180f;
                break;
            case 1:
            case -3:
                rotAmount = 270f;
                break;
            //exit and move are same
            case 0:
                rotAmount = 0f;
                break;
        }

        townObject.transform.eulerAngles = new Vector3(0f, rotAmount, 0f);

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

        for (int i = 0; i<allPathedAreas.Count; i++)
        {
            int moveEnumToInt = 0;

            moveEnumToInt = (int)allPathedAreas[i].entranceDirection + townRotationDiff;
            if(moveEnumToInt > 4)
            {
                moveEnumToInt -= 4;
            }
            else if(moveEnumToInt < 1)
            {
                moveEnumToInt += 4;
            }
            allPathedAreas[i].entranceDirection = (MoveDirection)moveEnumToInt;
            allPathedAreas[i].pathTownEntrancePiece.connectedPathPiece.pieceFacingDirection = (MoveDirection)moveEnumToInt;

           // Debug.Log("Exit " + entrancePathAreas[i].parentPathPiece.name + " becomes " + entrancePathAreas[i].thisMoveDirection);
        }

        lastTownDirection = townOrientation;

        townToExitOffset = currentPathedArea.townReEntrancePoint.position - townObject.transform.position;

        //Move Town Position
        Debug.Log("Path end location : " + pathEndLocation + "Exit offset : " + townToExitOffset);
        townObject.transform.position = pathEndLocation - townToExitOffset;
        townObject.SetActive(true);

        //Reset path
        //currentPathProgress = 0;
        //currentPathedArea = null;
    }*/
}
