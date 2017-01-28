using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathConnectionPiece {

    public BuiltPathPiece thisConnectionPiece;
    public AreaTypes areaFrom;
    public AreaTypes areaTo;
    //[TODO] Get from bpp
    public MoveDirection connectionPieceDirection;
    public int minPosToOffer;
}
