using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilderv2 : MonoBehaviour {

    public PathPoolManager ppMan;

    PathedArea currentPathArea;

    BuiltPathPiece currentBPP;

    public MoveDirection startingMoveDirection;
    MoveDirection currentMoveDirection;

    public int currentPathProgress = 0;

    //Used for positioning the next piece
    Vector3 northAdd;
    Vector3 eastAdd;
    Vector3 southAdd;
    Vector3 westAdd;

    public void PlayerEntersNewPiece(BuiltPathPiece newPieceRef)
    {
        Debug.Log("Player enters " + newPieceRef);
        //Area check
        //PathedArea pa = ppMan.GetAreaForBuiltPathPiece(newPieceRef);
        bool enteredNewArea = false;

        //[TODO] temporary? Will town pieces be BPP?
        if (currentBPP != null
            && ((currentPathArea.thisAreaFormat != AreaFormat.Fixed)
                || (currentPathArea.thisAreaFormat == AreaFormat.Fixed && currentPathProgress == 0)))
        {
            ppMan.DeactivatePieces(currentBPP, newPieceRef);
        }

        //Current piece is new piece
        currentBPP = newPieceRef;

        currentPathProgress++;

        //If the player has travelled to a new area
        for (int i = 0; i < currentPathArea.connectionPieces.Count; i++)
        {
            //If it's a connection piece that the player entered, they are moving to a new area
            if (currentPathArea.connectionPieces[i].thisConnectionPiece == newPieceRef)
            {
                #region Area Change
                enteredNewArea = true;

                PathedArea newPA = ppMan.GetAreaOfType(currentPathArea.connectionPieces[i].areaTo);
                PlayerEntersNewArea(newPA);

                if (currentPathArea.thisAreaFormat == AreaFormat.Fixed)
                {
                    //Spawn extensions if it's a fixed area
                    BuildExtensionsToFixedArea();
                }
                else if (currentPathArea.thisAreaFormat == AreaFormat.Procedural)
                {
                    //Otherwise do normal spawning of extensions for procedural area
                    StartOnProceduralPath();
                }
                #endregion
            }
        }

        //If the area remains the same - continue building forward
        if(enteredNewArea == false)
        {
            ContinueProceduralArea();
        }
    }

    public void PlayerEntersNewArea(PathedArea pa)
    {
        //Reset counters
        currentPathProgress = 0;
        currentPathArea = pa;
        ppMan.MoveToNewArea(pa.thisAreaType);

        Debug.Log("Player enters new area " + currentPathArea.thisAreaType);
    }

    public void StartOnProceduralPath()
    {
        Debug.Log("Start on new procedural path");

        //Get first left and right
        //Spawn extensions for each
        BuildExtensionsForBPP(ppMan.currentPathPoolGroup.pathFirstLeftPiece);
        BuildExtensionsForBPP(ppMan.currentPathPoolGroup.pathFirstRightPiece);
    }

    public void BuildFixedArea(PathPoolGroup areaPoolGroup)
    {
        PlayerEntersNewArea(areaPoolGroup.thisPathedArea);

        //Activates fixed object
        areaPoolGroup.thisPathedArea.fixedAreaObject.SetActive(true);

        //Most likely need to re-position with this also for when town is rebuilt
    }

    //Must be current area
    public void BuildExtensionsToFixedArea()
    {
        Debug.Log("Build fixed area extensions");

        for (int i = 0; i < currentPathArea.connectionPieces.Count; i++)
        {
            PathedArea connectedPathArea = ppMan.GetAreaOfType(currentPathArea.connectionPieces[i].areaTo);
            //For each connection exit piece
            //Get Exit Location ref
            BuiltPathPiece extensionBPP = currentPathArea.connectionPieces[i].thisConnectionPiece;
            //Get position for pieces to be placed
            Vector3 exitPosition = extensionBPP.exitLocations[0].pathTurnLocation.position;
            if (extensionBPP.exitLocations[0].canDoLeft)
            {
                //Spawn a left piece for this pathed area
                BuiltPathPiece leftPiece = ppMan.GetValidBPPForAreaType(currentPathArea.connectionPieces[i].areaTo);

                //Position and rotate accordingly
                leftPiece.intendedMoveDirection = GetLeftMoveDirection(currentPathArea.connectionPieces[i].connectionPieceDirection);
                leftPiece.transform.position = GetNextPlacementPosition(leftPiece.intendedMoveDirection,
                                                                        exitPosition,
                                                                        leftPiece);
                leftPiece.transform.eulerAngles = GetEulerAnglesForMoveDirection(leftPiece.intendedMoveDirection);

                extensionBPP.exitLocations[0].nextLeftPathPiece = leftPiece;
                switch (leftPiece.intendedMoveDirection)
                {
                    case MoveDirection.North:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.northPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.East:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.eastPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.South:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.southPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.West:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.westPiece = leftPiece.connectedPathPiece;
                        break;
                }

                //Activate piece
                ppMan.ActivatePathPiece_InConnectedArea(connectedPathArea, leftPiece, true, false);
            }
            if (extensionBPP.exitLocations[0].canDoRight)
            {
                //Spawn a right piece for this pathed area
                BuiltPathPiece rightPiece = ppMan.GetValidBPPForAreaType(currentPathArea.connectionPieces[i].areaTo);

                //Position and rotate accordingly
                rightPiece.intendedMoveDirection = GetRightMoveDirection(currentPathArea.connectionPieces[i].connectionPieceDirection);
                rightPiece.transform.position = GetNextPlacementPosition(rightPiece.intendedMoveDirection,
                                                                        exitPosition,
                                                                        rightPiece);
                rightPiece.transform.eulerAngles = GetEulerAnglesForMoveDirection(rightPiece.intendedMoveDirection);

                extensionBPP.exitLocations[0].nextRightPathPiece = rightPiece;
                switch (rightPiece.intendedMoveDirection)
                {
                    case MoveDirection.North:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.northPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.East:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.eastPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.South:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.southPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.West:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.westPiece = rightPiece.connectedPathPiece;
                        break;
                }
                //Activate piece
                ppMan.ActivatePathPiece_InConnectedArea(connectedPathArea, rightPiece, false, true);
            }
        }
    }
    
    public void ContinueProceduralArea()
    {
        Debug.Log("Spawn procedural pieces");

        //For each of the current piece's exit locations
        for(int i = 0; i < currentBPP.exitLocations.Count; i++)
        {
            //Check if it's a connection
            PathConnectionPiece pcpLeft = currentPathArea.GetConnectionPieceOfBPP(currentBPP.exitLocations[i].nextLeftPathPiece);
            //If it's null, there was nothing to return
            //If not, then it is a connection piece, and extensions should be spawned for it
            if (pcpLeft != null)
            {
                BuildConnectionsToConnectedArea(pcpLeft);
            }
            //Else, spawn extensions normally
            else
            {
                BuildExtensionsForBPP(currentBPP.exitLocations[i].nextLeftPathPiece);
            }

            //Check if it's a connection
            PathConnectionPiece pcpRight = currentPathArea.GetConnectionPieceOfBPP(currentBPP.exitLocations[i].nextRightPathPiece);
            //If it's null, there was nothing to return
            //If not, then it is a connection piece, and extensions should be spawned for it
            if (pcpRight != null)
            {
                BuildConnectionsToConnectedArea(pcpRight);
            }
            //Else, spawn extensions normally
            else
            {
                BuildExtensionsForBPP(currentBPP.exitLocations[i].nextRightPathPiece);
            }
        }
    }

    //[TODO] Extension code could probably / definitely be cleaned up (duplicates)
    //Procedural version of the above
    void BuildConnectionsToConnectedArea(PathConnectionPiece connectionPiece)
    {
        PathedArea connectedPathArea = ppMan.GetAreaOfType(connectionPiece.areaTo);

        //Move fixed areas accordingly
        if (connectedPathArea.thisAreaFormat == AreaFormat.Fixed)
        {
            //[TODO] properly reposition
            PositionFixedArea(connectionPiece.thisConnectionPiece, connectedPathArea);
        }
        //OR spawn extensions of next procedural area
        else
        {
            //Get Exit Location ref
            BuiltPathPiece extensionBPP = connectionPiece.thisConnectionPiece;
            //Get position for pieces to be placed
            Vector3 exitPosition = extensionBPP.exitLocations[0].pathTurnLocation.position;

            if (extensionBPP.exitLocations[0].canDoLeft)
            {
                //Spawn a left piece for this pathed area
                BuiltPathPiece leftPiece = ppMan.GetValidBPPForAreaType(connectionPiece.areaTo);

                //Position and rotate accordingly
                leftPiece.intendedMoveDirection = GetLeftMoveDirection(connectionPiece.connectionPieceDirection);
                leftPiece.transform.position = GetNextPlacementPosition(leftPiece.intendedMoveDirection,
                                                                        exitPosition,
                                                                        leftPiece);
                leftPiece.transform.eulerAngles = GetEulerAnglesForMoveDirection(leftPiece.intendedMoveDirection);

                extensionBPP.exitLocations[0].nextLeftPathPiece = leftPiece;
                switch (leftPiece.intendedMoveDirection)
                {
                    case MoveDirection.North:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.northPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.East:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.eastPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.South:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.southPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.West:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.westPiece = leftPiece.connectedPathPiece;
                        break;
                }

                //Activate piece
                ppMan.ActivatePathPiece_InConnectedArea(connectedPathArea, leftPiece, true, false);
            }
            if (extensionBPP.exitLocations[0].canDoRight)
            {
                //Spawn a right piece for this pathed area
                BuiltPathPiece rightPiece = ppMan.GetValidBPPForAreaType(connectionPiece.areaTo);

                //Position and rotate accordingly
                rightPiece.intendedMoveDirection = GetRightMoveDirection(connectionPiece.connectionPieceDirection);
                rightPiece.transform.position = GetNextPlacementPosition(rightPiece.intendedMoveDirection,
                                                                        exitPosition,
                                                                        rightPiece);
                rightPiece.transform.eulerAngles = GetEulerAnglesForMoveDirection(rightPiece.intendedMoveDirection);

                extensionBPP.exitLocations[0].nextRightPathPiece = rightPiece;
                switch (rightPiece.intendedMoveDirection)
                {
                    case MoveDirection.North:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.northPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.East:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.eastPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.South:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.southPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.West:
                        extensionBPP.exitLocations[0].connectedTurnTriggerArea.westPiece = rightPiece.connectedPathPiece;
                        break;
                }
                //Activate piece
                ppMan.ActivatePathPiece_InConnectedArea(connectedPathArea, rightPiece, false, true);
            }
        }
    }

    public void BuildExtensionsForBPP(BuiltPathPiece bpp)
    {
        //For each exit location
        for(int i = 0; i < bpp.exitLocations.Count; i++)
        {
            //Spawn a left
            if(bpp.exitLocations[i].canDoLeft)
            {
                //Spawn a right piece for this pathed area
                BuiltPathPiece leftPiece = ppMan.GetValidBPPForPathedArea(currentPathArea);

                //Position and rotate accordingly
                leftPiece.intendedMoveDirection = GetLeftMoveDirection(bpp.intendedMoveDirection);
                leftPiece.transform.position = GetNextPlacementPosition(leftPiece.intendedMoveDirection,
                                                                        bpp.exitLocations[i].pathTurnLocation.position,
                                                                        leftPiece);
                leftPiece.transform.eulerAngles = GetEulerAnglesForMoveDirection(leftPiece.intendedMoveDirection);

                bpp.exitLocations[i].nextLeftPathPiece = leftPiece;
                switch (leftPiece.intendedMoveDirection)
                {
                    case MoveDirection.North:
                        bpp.exitLocations[i].connectedTurnTriggerArea.northPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.East:
                        bpp.exitLocations[i].connectedTurnTriggerArea.eastPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.South:
                        bpp.exitLocations[i].connectedTurnTriggerArea.southPiece = leftPiece.connectedPathPiece;
                        break;
                    case MoveDirection.West:
                        bpp.exitLocations[i].connectedTurnTriggerArea.westPiece = leftPiece.connectedPathPiece;
                        break;
                }
                //Activate piece
                //[TODO] next version 
                ppMan.ActivatePathPiece_InCurrentArea(leftPiece);
            }

            //Spawn a right
            if (bpp.exitLocations[i].canDoRight)
            {
                //Spawn a right piece for this pathed area
                BuiltPathPiece rightPiece = ppMan.GetValidBPPForPathedArea(currentPathArea);

                //Position and rotate accordingly
                rightPiece.intendedMoveDirection = GetRightMoveDirection(bpp.intendedMoveDirection);
                rightPiece.transform.position = GetNextPlacementPosition(rightPiece.intendedMoveDirection,
                                                                        bpp.exitLocations[i].pathTurnLocation.position,
                                                                        rightPiece);
                rightPiece.transform.eulerAngles = GetEulerAnglesForMoveDirection(rightPiece.intendedMoveDirection);

                bpp.exitLocations[i].nextRightPathPiece = rightPiece;
                switch (rightPiece.intendedMoveDirection)
                {
                    case MoveDirection.North:
                        bpp.exitLocations[i].connectedTurnTriggerArea.northPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.East:
                        bpp.exitLocations[i].connectedTurnTriggerArea.eastPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.South:
                        bpp.exitLocations[i].connectedTurnTriggerArea.southPiece = rightPiece.connectedPathPiece;
                        break;
                    case MoveDirection.West:
                        bpp.exitLocations[i].connectedTurnTriggerArea.westPiece = rightPiece.connectedPathPiece;
                        break;
                }
                //Activate piece
                //[TODO] next version 
                ppMan.ActivatePathPiece_InCurrentArea(rightPiece);
            }
        }
    }

    void PositionFixedArea(BuiltPathPiece exitPiece, PathedArea fixedArea)
    {
        //Get entrance area to align
        FixedAreaEntrances entranceArea = fixedArea.GetFixedAreaEntranceForAreaType(currentPathArea.thisAreaType);

        //"Place" entrance piece going left
        //entranceArea.connectedEntrancePathPiece.intendedMoveDirection = GetLeftMoveDirection(exitPiece.intendedMoveDirection);
        //Piece should go straight ahead
        entranceArea.connectedEntrancePathPiece.intendedMoveDirection = exitPiece.intendedMoveDirection;

        Vector3 exitPosition = GetNextPlacementPosition(entranceArea.connectedEntrancePathPiece.intendedMoveDirection,
                                                                exitPiece.exitLocations[0].pathTurnLocation.position,
                                                                entranceArea.connectedEntrancePathPiece);

        //Get rotation where "intended direction" compares to "entrance direction"
        int directionDiff = (int)entranceArea.connectedEntrancePathPiece.intendedMoveDirection
                                - (int)entranceArea.entranceDirection;
        directionDiff++; //Because north is 1
        if(directionDiff < 1)
        {
            directionDiff += 4;
        }
        else if(directionDiff > 4)
        {
            directionDiff -= 4;
        }
        MoveDirection directionDiffDir = (MoveDirection)directionDiff;
        Vector3 fixedEul = Vector3.zero;
        Vector3 newFixedAreaPos = Vector3.zero;

        switch (directionDiffDir)
        {
            case MoveDirection.West:
                fixedEul.y = 270f;
                break;
            case MoveDirection.South:
                fixedEul.y = 180f;
                break;
            case MoveDirection.East:
                fixedEul.y = 90f;
                break;
            //exit and move are same
            case MoveDirection.North:
                fixedEul.y = 0f;
                break;
        }
        
        //Update fixed area connection piece directions to work with new rotation
        for(int i = 0; i < fixedArea.connectionPieces.Count; i++)
        {
            int moveDirChange = (int)fixedArea.connectionPieces[i].originalPieceDirection + directionDiff;
            moveDirChange--;    //Because north is 1
            if(moveDirChange > 4)
            {
                moveDirChange -= 4;
            }

            fixedArea.connectionPieces[i].connectionPieceDirection = (MoveDirection)moveDirChange;
        }
       
        Debug.Log(exitPiece.intendedMoveDirection + " " + entranceArea.connectedEntrancePathPiece.intendedMoveDirection + " " + entranceArea.entranceDirection + " " + directionDiffDir);

        //Adjust town based on entrance
        fixedArea.fixedAreaObject.transform.eulerAngles = fixedEul;
        
        //Get original settings of entrancePiece to fixedArea
        Vector3 pieceOffset = entranceArea.connectedEntrancePathPiece.transform.position - fixedArea.fixedAreaObject.transform.position;

        //entranceArea.connectedEntrancePathPiece.transform.position = exitPosition;
        newFixedAreaPos = exitPosition - pieceOffset;
        Debug.Log(exitPosition + " " + pieceOffset + " " + newFixedAreaPos);
        fixedArea.fixedAreaObject.transform.position = newFixedAreaPos;

        fixedArea.fixedAreaObject.SetActive(true);

        exitPiece.exitLocations[0].connectedFixedArea = fixedArea;
    }

    #region positional functions

    MoveDirection GetLeftMoveDirection(MoveDirection moveDir)
    {
        MoveDirection leftMoveDir;

        int val = (int)moveDir;

        val--;

        if (val < 1)
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

    Vector3 GetEulerAnglesForMoveDirection(MoveDirection moveDir)
    {
        Vector3 eulAngles = Vector3.zero;
        switch(moveDir)
        {
            case MoveDirection.North:
                eulAngles = new Vector3(0f, 0f, 0f);
                break;
            case MoveDirection.East:
                eulAngles = new Vector3(0f, 90f, 0f);
                break;
            case MoveDirection.South:
                eulAngles = new Vector3(0f, 180f, 0f);
                break;
            case MoveDirection.West:
                eulAngles = new Vector3(0f, 270f, 0f);
                break;
        }

        return eulAngles;
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

    #endregion
}
