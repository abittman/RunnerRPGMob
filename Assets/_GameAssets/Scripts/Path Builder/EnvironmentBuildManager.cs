using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentPiece
{
    public GameObject piecePrefab;
    //Size? Rotation? Etc?
}

[System.Serializable]
public class EnvironmentPieceCollection
{
    public AreaTypes thisAreaType;
    public List<EnvironmentPiece> allEnvironmentPieces = new List<EnvironmentPiece>();
}

public class EnvironmentBuildManager : MonoBehaviour
{
    public List<EnvironmentPieceCollection> allEnviroPieceCollections = new List<EnvironmentPieceCollection>();

    public EnvironmentPiece GetPieceForAreaType(AreaTypes findType)
    {
        EnvironmentPieceCollection collection = allEnviroPieceCollections.Find(x => x.thisAreaType == findType);

        int randomIndex = Random.Range(0, collection.allEnvironmentPieces.Count);

        return collection.allEnvironmentPieces[randomIndex];
    }
}
