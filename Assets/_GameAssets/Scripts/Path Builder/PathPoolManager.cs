using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoolManager : MonoBehaviour {

    public List<PathPoolGroup> allPathedAreas = new List<PathPoolGroup>();

    public PathPoolGroup currentPathPoolGroup;

    public PathBuilderv2 pathBuilder;

    public BuiltPathPiece currentBuiltPathPiece;
    public BuiltPathPiece lastBuiltPathPiece;

    public void InitialPathSetup()
    {
        for(int i = 0; i < allPathedAreas.Count; i++)
        {
            allPathedAreas[i].SetupPoolGroup();
        }
        DeactivateAllPoolPieces();
        SetStartingArea(AreaTypes.Town);
    }

    public void SetupAllStartData(List<PathPoolGroup_Data> ppg_Data)
    {
        for(int i = 0; i < ppg_Data.Count; i++)
        {
            PathPoolGroup pa = allPathedAreas.Find(x => x.thisPPGData.pathPoolGroupID == ppg_Data[i].pathPoolGroupID);

            if(pa != null)
            {
                pa.thisPPGData = ppg_Data[i];
                pa.SetupPoolGroup();
            }
        }

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
                    allPathedAreas[i].thisPathedArea.thisAreaProceduralPieces[j].DeactivateToPool();
                }
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
            currentPathPoolGroup.Fixed_SetupLocalReferences();
        }
    }

    public void MoveToNewArea(AreaTypes aType)
    {
        currentPathPoolGroup = allPathedAreas.Find(x => x.thisPathedArea.thisAreaType == aType);

        if (currentPathPoolGroup == null)
        {
            Debug.LogError("Invalid area type set as starting area.");
        }
        else
        {
            currentPathPoolGroup.Fixed_SetupLocalReferences();
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
                for (int j = 0; j < allPathedAreas[i].allPoolPieces.Count; j++)
                {
                    if (allPathedAreas[i].allPoolPieces[j] == bppRef)
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

        //Return the piece
        return availablePiece;
    }

    public BuiltPathPiece GetValidBPPForPathedArea(PathedArea pArea)
    {
        //Find the related group
        PathPoolGroup ppGroup = allPathedAreas.Find(x => x.thisPathedArea == pArea);

        //Get a random available piece from that group
        BuiltPathPiece availablePiece = GetProceduralBPPForGroup(ppGroup);

        //Return the piece
        return availablePiece;
    }

    //Alternate version required for connection pieces also?
    public BuiltPathPiece GetProceduralBPPForGroup(PathPoolGroup pathPoolGroup)
    {
        List<BuiltPathPiece> allActivePieces = pathPoolGroup.GetOnlyInactivePoolPieces();

        if (allActivePieces.Count > 0)
        {
            //This loop sucks [TODO] make better
            bool loopForever = true;
            do
            {
                bool pieceFailed = false;
                int rand = Random.Range(0, allActivePieces.Count);

                for(int i = 0; i < pathPoolGroup.thisPathedArea.connectionPieces.Count; i++)
                {
                    if(pathPoolGroup.thisPathedArea.connectionPieces[i].thisConnectionPiece == allActivePieces[rand])
                    {
                        if(pathPoolGroup.thisPathedArea.connectionPieces[i].minPosToOffer <= pathBuilder.currentPathProgress)
                        {
                            return allActivePieces[rand];
                        }
                        else
                        {
                            pieceFailed = true;
                        }
                    }
                }

                if (allActivePieces[rand].isActive == false
                    && pieceFailed == false)
                {
                    return allActivePieces[rand];
                }
            } while (loopForever);

            return null;
        }
        else
        {
            Debug.LogWarning("No inactive bpp found for group " + pathPoolGroup.thisPathedArea.thisAreaType);
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
        
        bppRef.ActivateFromPool();
    }

    public void ActivatePathPiece_InCurrentArea(BuiltPathPiece bppRef)
    {
        bppRef.ActivateFromPool();
    }

    #region Deactivation
    //[TODO] deactivate from pool for recycling

    public void DeactivatePieces(BuiltPathPiece lastPiece, BuiltPathPiece currentPiece)
    {
        Debug.Log("Deactivate pieces");
        //Deactivate last piece on delay
        PathedArea lastPA = GetAreaForBuiltPathPiece(lastPiece);
        StartCoroutine(DeactivatePieceOnDelay(lastPiece, lastPA));

        //Deactivate alternate piece on delay
        //Deactivate non alternate exits, and children, instantly
        for (int i = 0; i < lastPiece.exitLocations.Count; i++)
        {
            //If player did not turn left here, disable it
            if(lastPiece.exitLocations[i].nextLeftPathPiece != currentPiece
                && lastPiece.exitLocations[i].nextLeftPathPiece != null)
            {
                PathedArea altPA = GetAreaForBuiltPathPiece(lastPiece.exitLocations[i].nextLeftPathPiece);

                if (altPA.thisAreaFormat == AreaFormat.Procedural)
                {
                    lastPiece.exitLocations[i].nextLeftPathPiece.DeactivateToPool();
                    for (int j = 0; j < lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations.Count; j++)
                    {
                        if (lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextLeftPathPiece != null)
                        {
                            lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextLeftPathPiece.DeactivateToPool();
                        }
                        if (lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextRightPathPiece != null)
                        {
                            lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].nextRightPathPiece.DeactivateToPool();
                        }
                        if(lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].connectedFixedArea.fixedAreaObject != null)
                        {
                            lastPiece.exitLocations[i].nextLeftPathPiece.exitLocations[j].connectedFixedArea.fixedAreaObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    altPA.fixedAreaObject.SetActive(false);
                }
            }
            //If player did not turn right here, disable it
            if (lastPiece.exitLocations[i].nextRightPathPiece != currentPiece
                && lastPiece.exitLocations[i].nextRightPathPiece != null)
            {
                PathedArea altPA = GetAreaForBuiltPathPiece(lastPiece.exitLocations[i].nextRightPathPiece);

                if (altPA.thisAreaFormat == AreaFormat.Procedural)
                {
                    lastPiece.exitLocations[i].nextRightPathPiece.DeactivateToPool();
                    for (int j = 0; j < lastPiece.exitLocations[i].nextRightPathPiece.exitLocations.Count; j++)
                    {
                        if (lastPiece.exitLocations[i].nextRightPathPiece.exitLocations[j].nextLeftPathPiece != null)
                        {
                            lastPiece.exitLocations[i].nextRightPathPiece.exitLocations[j].nextLeftPathPiece.DeactivateToPool();
                        }
                        if (lastPiece.exitLocations[i].nextRightPathPiece.exitLocations[j].nextRightPathPiece != null)
                        {
                            lastPiece.exitLocations[i].nextRightPathPiece.exitLocations[j].nextRightPathPiece.DeactivateToPool();
                        }
                        if (lastPiece.exitLocations[i].nextRightPathPiece.exitLocations[j].connectedFixedArea.fixedAreaObject != null)
                        {
                            lastPiece.exitLocations[i].nextRightPathPiece.exitLocations[j].connectedFixedArea.fixedAreaObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    altPA.fixedAreaObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator DeactivatePieceOnDelay(BuiltPathPiece deactivateBPP, PathedArea pArea)
    {
        yield return new WaitForSeconds(0.5f);

        if (pArea.thisAreaFormat == AreaFormat.Procedural)
        {
            deactivateBPP.DeactivateToPool();
        }
        else
        {
            //Deactivate fixed area
            pArea.fixedAreaObject.SetActive(false);
            
            //Also deactivate any connected areas
            for(int i = 0; i < pArea.connectionPieces.Count; i++)
            {
                PathPoolGroup ppGroup = allPathedAreas.Find(x => x.thisPathedArea.thisAreaType == pArea.connectionPieces[i].areaTo);

                if(ppGroup != null
                    && ppGroup != currentPathPoolGroup)
                {
                    ppGroup.pathFirstLeftPiece.DeactivateToPool();
                    ppGroup.pathFirstRightPiece.DeactivateToPool();
                }
            }
        }
    }
    #endregion
}
