using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathPoolGroup_Data
{
    public string pathPoolGroupID;
    public List<ResourceNode_Data> resourceNode_Data = new List<ResourceNode_Data>();
    public List<WorldEvent_Data> worldEvent_Data = new List<WorldEvent_Data>();
}

public class PathPoolGroup : MonoBehaviour {

    public PathedArea thisPathedArea;

    [Space]

    //Combines pieces in area lists
    public List<BuiltPathPiece> allPoolPieces = new List<BuiltPathPiece>();

    //Showonly
    public BuiltPathPiece pathFirstLeftPiece;
    public BuiltPathPiece pathFirstRightPiece;

    [Header("Local References")]
    public List<ResourceNode> fixedAreaResourceNodes = new List<ResourceNode>();
    public List<WorldEvent> fixedAreaWorldEvents = new List<WorldEvent>();

    public PathPoolGroup_Data thisPPGData;

    public void SetupPoolGroup()
    {
        allPoolPieces.AddRange(thisPathedArea.thisAreaProceduralPieces);
        for(int i = 0; i < thisPathedArea.connectionPieces.Count; i++)
        {
            allPoolPieces.Add(thisPathedArea.connectionPieces[i].thisConnectionPiece);
        }

        for(int i = 0; i < thisPPGData.resourceNode_Data.Count; i++)
        {
            ResourceNode rn = fixedAreaResourceNodes.Find(x => x.optionalUniqueID == thisPPGData.resourceNode_Data[i].resourceNodeID);
            if(rn != null)
            {
                rn.nodeData = thisPPGData.resourceNode_Data[i];
            }
        }

        for(int i = 0; i < thisPPGData.worldEvent_Data.Count; i++)
        {
            WorldEvent we = fixedAreaWorldEvents.Find(x => x.thisEventData.eventID == thisPPGData.worldEvent_Data[i].eventID);
            if(we != null)
            {
                we.SetupEvent(thisPPGData.worldEvent_Data[i]);
            }
        }
    }

    public List<BuiltPathPiece> GetOnlyInactivePoolPieces()
    {
        return allPoolPieces.FindAll(x => x.isActive == false);
    }

    public void Fixed_SetupLocalReferences()
    {
        for(int i = 0; i < fixedAreaResourceNodes.Count; i++)
        {
            fixedAreaResourceNodes[i].SetupNode();
        }
    }

    public PathPoolGroup_Data GetPPG_Data()
    {
        if (thisPathedArea.thisAreaFormat == AreaFormat.Fixed)
        {
            thisPPGData.resourceNode_Data.Clear();
            for (int i = 0; i < fixedAreaResourceNodes.Count; i++)
            {
                thisPPGData.resourceNode_Data.Add(fixedAreaResourceNodes[i].nodeData);
            }

            for(int i = 0; i < fixedAreaWorldEvents.Count; i++)
            {
                thisPPGData.worldEvent_Data.Add(fixedAreaWorldEvents[i].thisEventData);
            }
        }

        return thisPPGData;
    }
}
