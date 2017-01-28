using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaFormat
{
    None,
    Fixed,
    Procedural
}

[System.Serializable]
public class PathedArea
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
}
