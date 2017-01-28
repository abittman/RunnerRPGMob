using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathTurnConditions
{
    public TurnTriggerArea connectedTurnTriggerArea;
    public Transform pathTurnLocation;
    public bool canDoLeft = false;
    public bool canDoRight = false;
    public BuiltPathPiece presetLeftPathPiece;
    public BuiltPathPiece presetRightPathPiece;

    public BuiltPathPiece nextLeftPathPiece;
    public BuiltPathPiece nextRightPathPiece;
}
