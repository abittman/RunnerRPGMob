using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoolManager : MonoBehaviour {

    public List<PathPoolGroup> allPathedAreas = new List<PathPoolGroup>();

    public PathPoolGroup currentPathPoolGroup;

    public PathBuilderv2 pathBuilder;

    public BuiltPathPiece currentBuiltPathPiece;
    public BuiltPathPiece lastBuiltPathPiece;

    void Start()
    {
        DeactivateAllPoolPieces();
        SetStartingArea(AreaTypes.Town);
    }

    //Deactivate and clear all active pool pieces
    public void DeactivateAllPoolPieces()
    {
        for (int i = 0; i < allPathedAreas.Count; i++)
        {
            if (allPathedAreas[i].thisPathedArea.thisAreaFormat == AreaFormat.Procedural)
            {
                for (int j = 0; j < allPathedAreas[i].thisPathedArea.thisAreaProceduralPieces.Count; j++)
                {
                    allPathedAreas[i].thisPathedArea.thisAreaProceduralPieces[j].DeactivateToPool(true);
                }
                allPathedAreas[i].activeGroupPieces.Clear();
            }
            else if(allPathedAreas[i].thisPathedArea.thisAreaFormat == AreaFormat.Fixed)
            {
                allPathedAreas[i].thisPathedArea.fixedAreaObject.SetActive(false);
            }
        }
    }

    public void SetStartingArea(AreaTypes areaType)
    {
        currentPathPoolGroup = allPathedAreas.Find(x => x.thisPathedArea.thisAreaType == areaType);
        if(currentPathPoolGroup == null)
        {
            Debug.LogError("Invalid area type set as starting area.");
        }
        else
        {
            pathBuilder.BuildFixedArea(currentPathPoolGroup);
            pathBuilder.BuildExtensionsToFixedArea();
        }
    }

    public void MoveToNewArea(AreaTypes aType)
    {
        currentPathPoolGroup = allPathedAreas.Find(x => x.thisPathedArea.thisAreaType == aType);
        if (currentPathPoolGroup == null)
        {
            Debug.LogError("Invalid area type set as starting area.");
        }
    }

    public PathedArea GetAreaOfType(AreaTypes aType)
    {
        return allPathedAreas.Find(x => x.thisPathedArea.thisAreaType == aType).thisPathedArea;
    }

    //Requested by path builder. Returns the pathed area of the passed BPP.
    //Used to check changes in areas.
    //[TODO] Should this also check if it's an exit piece? How to handle that?
    public PathedArea GetAreaForBuiltPathPiece(BuiltPathPiece bppRef)
    {
        for(int i = 0; i < allPathedAreas.Count; i++)
        {
            if (allPathedAreas[i].thisPathedArea.thisAreaFormat == AreaFormat.Procedural)
            {
                for (int j = 0; j < allPathedAreas[i].activeGroupPieces.Count; j++)
                {
                    if (allPathedAreas[i].activeGroupPieces[j] == bppRef)
                    {
                        return allPathedAreas[i].thisPathedArea;
                    }
                }
            }
            else if(allPathedAreas[i].thisPathedArea.thisAreaFormat == AreaFormat.Fixed)
            {
                if(bppRef.transform.parent.gameObject == allPathedAreas[i].thisPathedArea.fixedAreaObject)
                {
                    return allPathedAreas[i].thisPathedArea;
                }
            }
        }

        Debug.LogWarning("Could not find area for requested built path piece");
        return null;
    }

    public BuiltPathPiece GetValidBPPForAreaType(AreaTypes aType)
    {
        //Find the related group
        PathPoolGroup ppGroup = allPathedAreas.Find(x => x.thisPathedArea.thisAreaType == aType);

        //Get a random available piece from that group
        BuiltPathPiece availablePiece = GetProceduralBPPForGroup(ppGroup);

        //Add to active pool
        ppGroup.activeGroupPieces.Add(availablePiece);

        //Return the piece
        return availablePiece;
    }

    public BuiltPathPiece GetValidBPPForPathedArea(PathedArea pArea)
    {
        //Find the related group
        PathPoolGroup ppGroup = allPathedAreas.Find(x => x.thisPathedArea == pArea);

        //Get a random available piece from that group
        BuiltPathPiece availablePiece = GetProceduralBPPForGroup(ppGroup);

        //Add to active pool
        ppGroup.activeGroupPieces.Add(availablePiece);

        //Return the piece
        return availablePiece;
    }

    public BuiltPathPiece GetProceduralBPPForGroup(PathPoolGroup pathPoolGroup)
    {
        //[TODO] Better way of doing this? - counts are not that great...
        if (pathPoolGroup.activeGroupPieces.Count < pathPoolGroup.thisPathedArea.thisAreaProceduralPieces.Count)
        {
            //This loop sucks [TODO] make better
            bool loopForever = true;
            do
            {
                int rand = Random.Range(0, pathPoolGroup.thisPathedArea.thisAreaProceduralPieces.Count);

                if (pathPoolGroup.thisPathedArea.thisAreaProceduralPieces[rand].isActive == false)
                {
                    return pathPoolGroup.thisPathedArea.thisAreaProceduralPieces[rand];
                }
            } while (loopForever);

            return null;
        }
        else
        {
            Debug.LogWarning("No inactive bpp found");
            return null;
        }
    }

    public void ActivatePathPiece_InConnectedArea(PathedArea pArea, BuiltPathPiece bppRef, bool isLeft, bool isRight)
    {
        PathPoolGroup thisGroup = allPathedAreas.Find(x => x.thisPathedArea == pArea);

        if(isLeft)
        {
            thisGroup.pathFirstLeftPiece = bppRef;
        }
        else if(isRight)
        {
            thisGroup.pathFirstRightPiece = bppRef;
        }

        //[TODO] proper piece activation call
        bppRef.ActivateFromPool();
    }

    public void ActivatePathPiece_InCurrentArea(BuiltPathPiece bppRef)
    {
        //[TODO] proper piece activation call
        bppRef.ActivateFromPool();
    }

    #region Deactivation
    //[TODO] deactivate from pool for recycling
    #endregion
}
