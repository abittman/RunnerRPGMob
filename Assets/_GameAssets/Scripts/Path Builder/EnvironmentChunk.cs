using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentChunkData
{
    public EnvironmentChunk thisEnviroChunk;
}

[System.Serializable]
public class EnvironmentSpawns
{
    public Transform spawnLoc;
}

public class EnvironmentChunk : MonoBehaviour
{
    public EnvironmentBuildManager environmentBuildMan;

    public AreaTypes environmentAreaType;
    public List<EnvironmentSpawns> thisEnvironmentsSpawns = new List<EnvironmentSpawns>();

    List<GameObject> spawnedObjects = new List<GameObject>();

    public void SpawnEnvironmentPieces()
    {
        //Destroy old
        for(int i = 0; i < spawnedObjects.Count; i++)
        {
            if(spawnedObjects != null)
                DestroyImmediate(spawnedObjects[i]);
        }
        spawnedObjects.Clear();

        for(int i = 0; i < thisEnvironmentsSpawns.Count; i++)
        {
            EnvironmentPiece ep = environmentBuildMan.GetPieceForAreaType(environmentAreaType);
            GameObject g = Instantiate(ep.piecePrefab, thisEnvironmentsSpawns[i].spawnLoc);
            g.transform.localPosition = Vector3.zero;
            spawnedObjects.Add(g);
        }
    }
}
