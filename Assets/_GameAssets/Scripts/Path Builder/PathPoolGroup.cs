using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetupPoolGroup()
    {
        allPoolPieces.AddRange(thisPathedArea.thisAreaProceduralPieces);
        for(int i = 0; i < thisPathedArea.connectionPieces.Count; i++)
        {
            allPoolPieces.Add(thisPathedArea.connectionPieces[i].thisConnectionPiece);
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
}
