using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilderv2 : MonoBehaviour {

    public PathPoolManager ppMan;

    PathedArea currentPathArea;

    BuiltPathPiece currentBPP;

    public MoveDirection startingMoveDirection;
    MoveDirection currentMoveDirection;

    int currentPathProgress = 0;

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

                //Activate piece
                ppMan.ActivatePathPiece_InConnectedArea(connectedPathArea, rightPiece, false, true);
            }
        }
    }

    //Procedural version of the above
    void BuildConnectionsToProceduralArea()
    {
        //Move fixed areas accordingly

        //OR spawn extensions of next procedural area
    }

    public void ContinueProceduralArea()
    {
        Debug.Log("Spawn procedural pieces");
        //Confirm whether the player turned left or right
        //For each of the current piece's exit locations

        //If left, then it becomes the current piece and the right (and it's connections) are disabled)
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

                //Activate piece
                //[TODO] next version 
                ppMan.ActivatePathPiece_InCurrentArea(rightPiece);
            }
        }
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
