using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceToolType
{
    Empty,
    Plain_Square,
//A square may have a level / height. Independent of it's piece type.
//Raised_Square,
//Lowered_Square,
    Resource_Node,
    Incline,
    Decline
}

public class PathPieceToolManager : MonoBehaviour {

    public GameObject pathCube1;
    public GameObject pathCube2;
    public GameObject pathCube3;

    public Vector3 cubeDimensions;

    public GameObject turnPiece;
}
