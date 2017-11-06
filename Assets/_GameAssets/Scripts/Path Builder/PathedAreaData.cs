using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Path/PathedAreaData")]
public class PathedAreaData : ScriptableObject
{

    [Header("Area settings")]
    public AreaTypes thisAreaType;
    public AreaFormat thisAreaFormat;

    public int minPathLength = 5;
    public int maxPathLength = 10;

    [Header("Piece references")]
    public GameObject fixedAreaObject;
    [Tooltip("Minimum 16 pieces required")]
    public List<BuiltPathPiece> thisAreaProceduralPieces = new List<BuiltPathPiece>();

    [Header("Connection pieces")]
    public List<PathConnectionPiece> connectionPieces = new List<PathConnectionPiece>();

    [Header("Fixed Area Only")]
    public Transform fixedAreaStartLocation;
    public BuiltPathPiece startBPP;

    public List<FixedAreaEntrances> entrances = new List<FixedAreaEntrances>();

    public PathConnectionPiece GetConnectionPieceOfBPP(BuiltPathPiece bppRef)
    {
        return connectionPieces.Find(x => x.thisConnectionPiece == bppRef);
    }

    public FixedAreaEntrances GetFixedAreaEntranceForAreaType(AreaTypes aType)
    {
        return entrances.Find(x => x.areaFrom == aType);
    }
}
