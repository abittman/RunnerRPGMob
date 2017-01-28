using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoolGroup : MonoBehaviour {

    public PathedArea thisPathedArea;

    public List<BuiltPathPiece> activeGroupPieces = new List<BuiltPathPiece>();
    public List<BuiltPathPiece> piecesToBeDeactivated = new List<BuiltPathPiece>();

    //Showonly
    public BuiltPathPiece pathFirstLeftPiece;
    public BuiltPathPiece pathFirstRightPiece;
}
