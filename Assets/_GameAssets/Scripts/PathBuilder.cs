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
    Coast
}

[System.Serializable]
public class TurnChangeConditions
{
    public int minPosToOffer;
    public AreaTypes areaThisLeadsTo;
    public BuiltPathPiece connectorPieceToNextArea;
}

[System.Serializable]
public class PathedArea
{
    public AreaTypes thisAreaType;
    public BuiltPathPiece pathTownEntrancePiece;
    public MoveDirection entranceDirection;
    public BuiltPathPiece pathTownExitPiece;
    public MoveDirection exitDirection;
    [Tooltip("Minimum 9 pieces required")]
    public List<BuiltPathPiece> thisAreaPoolObjects = new List<BuiltPathPiece>();
    public Transform townReEntrancePoint;

    public int minPathLength = 5;
    public int maxPathLength = 10;
    //Want something about connected areas...
    public List<TurnChangeConditions> allTurnChangeConditions = new List<TurnChangeConditions>();

    //Show only
    public BuiltPathPiece pathFirstLeftPiece;
    public BuiltPathPiece pathFirstRightPiece;

}

public class PathBuilder : MonoBehaviour
{
    public List<PathedArea> allPathedAreas = new List<PathedArea>();
    //public List<BuiltPathPiece> forestSectionPoolObjects = new List<BuiltPathPiece>();
    PathedArea currentPathedArea;

    public MoveDirection currentMoveDirection;

    public int currentPathProgress = 0;
    /*
    public int minPathLength = 4;
    public int maxPathLength = 7;*/
    int pathLength = 5;
    
    bool lastIsTownPart = false;

    //Last pieces, to clear
    public BuiltPathPiece lastPathPiece;
    public BuiltPathPiece lastPathPiece_LRAlternative;
    //Current piece that player is on
    public BuiltPathPiece currentPathPiece;
    //Next left right from current piece
    public BuiltPathPiece nextLeftPathPiece;
    public BuiltPathPiece nextRightPathPiece;
    //Next left right from next left piece (2 ahead)
    public BuiltPathPiece nextLeft_NextLeftPathPiece;
    public BuiltPathPiece nextLeft_NextRightPathPiece;
    //Next left right from next right piece (2 ahead)
    public BuiltPathPiece nextRight_NextLeftPathPiece;
    public BuiltPathPiece nextRight_NextRightPathPiece;

    Vector3 northAdd;
    Vector3 southAdd;
    Vector3 eastAdd;
    Vector3 westAdd;

    public GameObject townObject;
    //public Transform townReturnPoint;
    public List<GameObject> objectsToNotDestroy;
    //public Vector3 townScale;
    //public MoveDirection exitOrientation = MoveDirection.South;

    //public List<PathTriggerArea> entrancePathAreas = new List<PathTriggerArea>();

    MoveDirection lastTownDirection = MoveDirection.North;

    bool hasReturnedToTown = true;

    public List<BuiltPathPiece> allBuiltPoolObjects = new List<BuiltPathPiece>();

	// Use this for initialization
	void Start ()
    {
        /*
        northAdd = new Vector3(objectScale.x / 2f, 0f, objectScale.z / 2f);
        southAdd = new Vector3(-objectScale.x / 2f, 0f, -objectScale.z / 2f);
        eastAdd = new Vector3(objectScale.z / 2f, 0f, objectScale.x / 2f);
        westAdd = new Vector3(-objectScale.z / 2f, 0f, -objectScale.x / 2f);
        */
        currentPathProgress = 0;
        
        DeactivateAllPoolPieces();

        CreateFirstPathConnections();
    }
	
	void DeactivateAllPoolPieces()
    {
        for(int i = 0; i<allPathedAreas.Count; i++)
        {
            for (int j = 0; j < allPathedAreas[i].thisAreaPoolObjects.Count; j++)
            {
                allPathedAreas[i].thisAreaPoolObjects[j].DeactivateToPool();
            }
        }

        allBuiltPoolObjects.Clear();

        lastPathPiece = null;
        lastPathPiece_LRAlternative = null;
        nextRightPathPiece = null;
        nextRight_NextLeftPathPiece = null;
        nextRight_NextRightPathPiece = null;
        nextLeftPathPiece = null;
        nextLeft_NextLeftPathPiece = null;
        nextLeft_NextRightPathPiece = null;
    }

    void CreateFirstPathConnections()
    {
        for(int i = 0; i < allPathedAreas.Count; i++)
        {
            Debug.Log("Build start of " + allPathedAreas[i].thisAreaType + "area");
            currentPathedArea = allPathedAreas[i];
            Vector3 entranceEndPoint = GetNextPlacementPosition(allPathedAreas[i].entranceDirection,
                                                                allPathedAreas[i].pathTownEntrancePiece.transform.position,
                                                                allPathedAreas[i].pathTownEntrancePiece);

            //Left Piece
            BuiltPathPiece leftPiece = RandomiseNextPathPiece(currentPathedArea);
            leftPiece.intendedMoveDirection = GetLeftMoveDirection(allPathedAreas[i].entranceDirection);

            Vector3 nextLeftPos = GetNextPlacementPosition(leftPiece.intendedMoveDirection,
                                                            entranceEndPoint,
                                                            leftPiece);

            //Rotation
            Vector3 leftEulRot = new Vector3(0f, ((int)leftPiece.intendedMoveDirection - 1) * 90f, 0f);

            //Creation
            leftPiece.ActivateFromPool(nextLeftPos, leftEulRot);
            allPathedAreas[i].pathFirstLeftPiece = leftPiece;

            switch(leftPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.northPiece = leftPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.eastPiece = leftPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.southPiece = leftPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.westPiece = leftPiece.connectedPathPiece;
                    break;
            }

            //Right Piece
            BuiltPathPiece rightPiece = RandomiseNextPathPiece(currentPathedArea);
            rightPiece.intendedMoveDirection = GetRightMoveDirection(allPathedAreas[i].entranceDirection);

            Vector3 nextRightPos = GetNextPlacementPosition(rightPiece.intendedMoveDirection,
                                                            entranceEndPoint,
                                                            rightPiece);
            //Rotation
            Vector3 rightEulRot = new Vector3(0f, ((int)rightPiece.intendedMoveDirection - 1) * 90f, 0f);

            //Creation
            rightPiece.ActivateFromPool(nextRightPos, rightEulRot);
            allPathedAreas[i].pathFirstRightPiece = rightPiece;

            switch (rightPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.northPiece = rightPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.eastPiece = rightPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.southPiece = rightPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    currentPathedArea.pathTownEntrancePiece.connectedTriggerArea.westPiece = rightPiece.connectedPathPiece;
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
                if ((nextLeftPathPiece == currentPathPieceRef || nextRightPathPiece == currentPathPieceRef)
                    && currentPathPieceRef == currentPathedArea.allTurnChangeConditions[i].connectorPieceToNextArea)
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
            Debug.Log("Create next path at " + currentPathPieceRef.gameObject.name);

            Vector3 currentPos = Vector3.zero;
            Vector3 endLocation = Vector3.zero;

            MoveDirection nextRightMoveDir = MoveDirection.None;
            MoveDirection nextLeftMoveDir = MoveDirection.None;

            if(currentPathProgress == 0)
            {
                Debug.Log("First path");
                //Set left and right to left and right
                nextLeftPathPiece = currentPathedArea.pathFirstLeftPiece;
                nextRightPathPiece = currentPathedArea.pathFirstRightPiece;
            }
            else
            {
                Debug.Log("Follow on path");
                lastPathPiece = currentPathPiece;
                //Deactivate extensions based on current path
                if(currentPathPieceRef == nextLeftPathPiece)
                {
                    currentMoveDirection = GetLeftMoveDirection(currentMoveDirection);

                    Debug.Log("Went left");
                    allBuiltPoolObjects.Remove(nextRight_NextLeftPathPiece);
                    nextRight_NextLeftPathPiece.DeactivateToPool();
                    nextRight_NextLeftPathPiece = null;
                    allBuiltPoolObjects.Remove(nextRight_NextRightPathPiece);
                    nextRight_NextRightPathPiece.DeactivateToPool();
                    nextRight_NextRightPathPiece = null;

                    currentPathPiece = nextLeftPathPiece;
                    lastPathPiece_LRAlternative = nextRightPathPiece;
                    nextLeftPathPiece = nextLeft_NextLeftPathPiece;
                    nextRightPathPiece = nextLeft_NextRightPathPiece;
                }
                else if(currentPathPieceRef == nextRightPathPiece)
                {
                    currentMoveDirection = GetRightMoveDirection(currentMoveDirection);

                    Debug.Log("Went right");
                    allBuiltPoolObjects.Remove(nextLeft_NextLeftPathPiece);
                    nextLeft_NextLeftPathPiece.DeactivateToPool();
                    nextLeft_NextLeftPathPiece = null;
                    allBuiltPoolObjects.Remove(nextLeft_NextRightPathPiece);
                    nextLeft_NextRightPathPiece.DeactivateToPool();
                    nextLeft_NextRightPathPiece = null;

                    currentPathPiece = nextRightPathPiece;
                    lastPathPiece_LRAlternative = nextLeftPathPiece;
                    nextLeftPathPiece = nextRight_NextLeftPathPiece;
                    nextRightPathPiece = nextRight_NextRightPathPiece;
                }
                else
                {
                    Debug.Log("!?!?!");
                }
            }

            SpawnExtensions();
            currentPathPiece.PlayerOnThisPiece();

            if (lastPathPiece != null)
            {
                if ((lastPathPiece.connectedPathPiece.isTownPiece == true || lastPathPiece_LRAlternative.connectedPathPiece.isTownPiece)
                    && currentPathPiece.connectedPathPiece.isTownPiece == false)
                {
                    lastIsTownPart = true;
                }
            }

            /* DO NEXT
             * Have to handle town reset
             * And options of progress
             * */
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
        northAdd = new Vector3(0f, 0f, objectScale.x / 2f + objectScale.z / 2f);
        southAdd = new Vector3(0f, 0f, -(objectScale.x / 2f + objectScale.z / 2f));
        eastAdd = new Vector3(objectScale.x / 2f + objectScale.z / 2f, 0f, 0f);
        westAdd = new Vector3(-(objectScale.x / 2f + objectScale.z / 2f), 0f, 0f);

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
        PathedArea nextLeftPathArea = currentPathedArea;
        PathedArea nextRightPathArea = currentPathedArea;

        //Check if path leads to new area
        for (int i = 0; i < currentPathedArea.allTurnChangeConditions.Count; i++)
        {
            if (currentPathedArea.allTurnChangeConditions[i].connectorPieceToNextArea == nextLeftPathPiece)
            {
                //If it's the town, do different logic
                if (currentPathedArea.allTurnChangeConditions[i].areaThisLeadsTo == AreaTypes.Town)
                {
                    nextLeftPathArea = null;
                }
                else
                {
                    nextLeftPathArea = allPathedAreas.Find(x => x.thisAreaType == currentPathedArea.allTurnChangeConditions[i].areaThisLeadsTo);
                }
            }
            else if (currentPathedArea.allTurnChangeConditions[i].connectorPieceToNextArea == nextRightPathPiece)
            {//If it's the town, do different logic
                if (currentPathedArea.allTurnChangeConditions[i].areaThisLeadsTo == AreaTypes.Town)
                {
                    nextRightPathArea = null;
                }
                else
                {
                    nextRightPathArea = allPathedAreas.Find(x => x.thisAreaType == currentPathedArea.allTurnChangeConditions[i].areaThisLeadsTo);
                }
            }

        }

        //Right end pos
        Vector3 nextRightEndPos = GetNextPlacementPosition(nextRightPathPiece.intendedMoveDirection,
                                                        nextRightPathPiece.transform.position,
                                                        nextRightPathPiece);

        if (nextRightPathArea == null)
        {
            ResetTown(nextRightEndPos, GetRightMoveDirection(currentMoveDirection));
        }
        else
        {
            //Spawn next right NL
            BuiltPathPiece nRight_nLeftPiece = RandomiseNextPathPiece(nextRightPathArea);
            nRight_nLeftPiece.intendedMoveDirection = GetLeftMoveDirection(nextRightPathPiece.intendedMoveDirection);
            Vector3 nRight_nLeftPos = GetNextPlacementPosition(nRight_nLeftPiece.intendedMoveDirection,
                                                            nextRightEndPos,
                                                            nRight_nLeftPiece);
            Vector3 nRight_nLeftRot = new Vector3(0f, ((int)nRight_nLeftPiece.intendedMoveDirection - 1) * 90f, 0f);
            nRight_nLeftPiece.ActivateFromPool(nRight_nLeftPos, nRight_nLeftRot);
            nextRight_NextLeftPathPiece = nRight_nLeftPiece;
            switch (nRight_nLeftPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    nextRightPathPiece.connectedTriggerArea.northPiece = nRight_nLeftPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    nextRightPathPiece.connectedTriggerArea.eastPiece = nRight_nLeftPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    nextRightPathPiece.connectedTriggerArea.southPiece = nRight_nLeftPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    nextRightPathPiece.connectedTriggerArea.westPiece = nRight_nLeftPiece.connectedPathPiece;
                    break;
            }
            //Spawn next left NR
            BuiltPathPiece nRight_nRightPiece = RandomiseNextPathPiece(nextRightPathArea);
            nRight_nRightPiece.intendedMoveDirection = GetRightMoveDirection(nextRightPathPiece.intendedMoveDirection);
            Vector3 nRight_nRightPos = GetNextPlacementPosition(nRight_nRightPiece.intendedMoveDirection,
                                                            nextRightEndPos,
                                                            nRight_nRightPiece);
            Vector3 nRight_nRightRot = new Vector3(0f, ((int)nRight_nRightPiece.intendedMoveDirection - 1) * 90f, 0f);
            nRight_nRightPiece.ActivateFromPool(nRight_nRightPos, nRight_nRightRot);
            nextRight_NextRightPathPiece = nRight_nRightPiece;
            switch (nRight_nRightPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    nextRightPathPiece.connectedTriggerArea.northPiece = nRight_nRightPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    nextRightPathPiece.connectedTriggerArea.eastPiece = nRight_nRightPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    nextRightPathPiece.connectedTriggerArea.southPiece = nRight_nRightPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    nextRightPathPiece.connectedTriggerArea.westPiece = nRight_nRightPiece.connectedPathPiece;
                    break;
            }
        }
        //Spawn next next left
        //Left end pos
        Vector3 nextLeftEndPos = GetNextPlacementPosition(nextLeftPathPiece.intendedMoveDirection,
                                                        nextLeftPathPiece.transform.position,
                                                        nextLeftPathPiece);
        if (nextLeftPathArea == null)
        {
            ResetTown(nextLeftEndPos, GetLeftMoveDirection(currentMoveDirection));
        }
        else
        {
            //Spawn next right NL
            BuiltPathPiece nLeft_nLeftPiece = RandomiseNextPathPiece(nextLeftPathArea);
            nLeft_nLeftPiece.intendedMoveDirection = GetLeftMoveDirection(nextLeftPathPiece.intendedMoveDirection);
            Vector3 nLeft_nLeftPos = GetNextPlacementPosition(nLeft_nLeftPiece.intendedMoveDirection,
                                                            nextLeftEndPos,
                                                            nLeft_nLeftPiece);
            Vector3 nLeft_nLeftRot = new Vector3(0f, ((int)nLeft_nLeftPiece.intendedMoveDirection - 1) * 90f, 0f);
            nLeft_nLeftPiece.ActivateFromPool(nLeft_nLeftPos, nLeft_nLeftRot);
            nextLeft_NextLeftPathPiece = nLeft_nLeftPiece;
            switch (nLeft_nLeftPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    nextLeftPathPiece.connectedTriggerArea.northPiece = nLeft_nLeftPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    nextLeftPathPiece.connectedTriggerArea.eastPiece = nLeft_nLeftPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    nextLeftPathPiece.connectedTriggerArea.southPiece = nLeft_nLeftPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    nextLeftPathPiece.connectedTriggerArea.westPiece = nLeft_nLeftPiece.connectedPathPiece;
                    break;
            }
            //Spawn next left NR
            BuiltPathPiece nLeft_nRightPiece = RandomiseNextPathPiece(nextLeftPathArea);
            nLeft_nRightPiece.intendedMoveDirection = GetRightMoveDirection(nextLeftPathPiece.intendedMoveDirection);
            Vector3 nLeft_nRightPos = GetNextPlacementPosition(nLeft_nRightPiece.intendedMoveDirection,
                                                            nextLeftEndPos,
                                                            nLeft_nRightPiece);
            Vector3 nLeft_nRightRot = new Vector3(0f, ((int)nLeft_nRightPiece.intendedMoveDirection - 1) * 90f, 0f);
            nLeft_nRightPiece.ActivateFromPool(nLeft_nRightPos, nLeft_nRightRot);
            nextLeft_NextRightPathPiece = nLeft_nRightPiece;
            switch (nLeft_nRightPiece.intendedMoveDirection)
            {
                case MoveDirection.North:
                    nextLeftPathPiece.connectedTriggerArea.northPiece = nLeft_nRightPiece.connectedPathPiece;
                    break;
                case MoveDirection.East:
                    nextLeftPathPiece.connectedTriggerArea.eastPiece = nLeft_nRightPiece.connectedPathPiece;
                    break;
                case MoveDirection.South:
                    nextLeftPathPiece.connectedTriggerArea.southPiece = nLeft_nRightPiece.connectedPathPiece;
                    break;
                case MoveDirection.West:
                    nextLeftPathPiece.connectedTriggerArea.westPiece = nLeft_nRightPiece.connectedPathPiece;
                    break;
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
                    allPathedAreas[i].pathFirstLeftPiece.DeactivateToPool();
                    allBuiltPoolObjects.Remove(allPathedAreas[i].pathFirstRightPiece);
                    allPathedAreas[i].pathFirstRightPiece.DeactivateToPool();
                }
            }
        }
        if (lastPathPiece != null)
        {
            allBuiltPoolObjects.Remove(lastPathPiece);
            lastPathPiece.DeactivateToPool();
            allBuiltPoolObjects.Remove(lastPathPiece_LRAlternative);
            lastPathPiece_LRAlternative.DeactivateToPool();
            Debug.Log("Deactivate last pieces");
        }

        if (lastIsTownPart)
        {
            Debug.Log("Clear town");
            townObject.SetActive(false);
            lastIsTownPart = false;
        }
    }

    BuiltPathPiece RandomiseNextPathPiece(PathedArea pathArea)
    {
        BuiltPathPiece nextPathPiece = null;

        bool newPiece = false;
        do
        {
            //Randomise number - more to do with this calculation some other time
            int randomR = Random.Range(0, pathArea.thisAreaPoolObjects.Count);

            //Check it was not a piece previously/recently used
            /*
            if(pathArea.thisAreaPoolObjects[randomR] != currentPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != lastPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != lastPathPiece_LRAlternative
                && pathArea.thisAreaPoolObjects[randomR] != nextRightPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != nextLeftPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != nextLeft_NextLeftPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != nextLeft_NextRightPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != nextRight_NextLeftPathPiece
                && pathArea.thisAreaPoolObjects[randomR] != nextRight_NextRightPathPiece)*/
            if(allBuiltPoolObjects.Exists(x => x == pathArea.thisAreaPoolObjects[randomR]) == false)
            {
                bool wasChangePiece = false;
                for(int i = 0; i < pathArea.allTurnChangeConditions.Count; i++)
                {
                    if(pathArea.thisAreaPoolObjects[randomR] == pathArea.allTurnChangeConditions[i].connectorPieceToNextArea)
                    {
                        wasChangePiece = true;

                        if (pathArea.allTurnChangeConditions[i].minPosToOffer <= currentPathProgress)
                        {
                            newPiece = true;
                            nextPathPiece = pathArea.thisAreaPoolObjects[randomR];
                        }
                    }
                }
                if (wasChangePiece == false)
                {
                    newPiece = true;
                    nextPathPiece = pathArea.thisAreaPoolObjects[randomR];
                }
            }

        } while (newPiece == false);

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
    }
}
