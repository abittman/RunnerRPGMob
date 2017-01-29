using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoolGroup : MonoBehaviour {

    public PathedArea thisPathedArea;

    //Combines pieces in area lists
    public List<BuiltPathPiece> allPoolPieces = new List<BuiltPathPiece>();

    //Showonly
    public BuiltPathPiece pathFirstLeftPiece;
    public BuiltPathPiece pathFirstRightPiece;

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
}
