using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FixedAreaEntrances
{
    public AreaTypes areaFrom;
    public BuiltPathPiece connectedEntrancePathPiece;
    public MoveDirection entranceDirection;
}
