using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPieceBuildTool : MonoBehaviour {

    [SerializeField]
    private PathPieceToolManager pathToolMan;
    public Transform pathGeometryParent;
    public List<PathChunkData> chunkPathObjects = new List<PathChunkData>();
    public List<GameObject> spawnedPathObjects = new List<GameObject>();

    public Transform surfaceChunkParent;
    public List<SurfaceChunkData> surfaceChunkObjects = new List<SurfaceChunkData>();
    public List<GameObject> spawnedSurfaceObjects = new List<GameObject>();

    public Transform environmentChunkParent;
    public List<EnvironmentChunkData> enviroChunkObjects = new List<EnvironmentChunkData>();
    public EnvironmentChunk pathEndChunk;
    public List<GameObject> spawnedEnvironmentObjects = new List<GameObject>();

    public int pathBlockLength;

    Transform startTransform;
    Vector3 placementPosition;

    public BuiltPathPiece thisBPP;
    //public GameObject turnPiece;

    public GameObject killAreaTrigger;
    public GameObject pathEntryTrigger;

    public void RebuildPath()
    {
        thisBPP = gameObject.GetComponent<BuiltPathPiece>();
        //pathGeometryParent = transform;

        for(int i = 0; i < spawnedPathObjects.Count; i++)
        {
            //Debug.Log("i");
            if (spawnedPathObjects[i] != null)
                DestroyImmediate(spawnedPathObjects[i]);
        }
        spawnedPathObjects.Clear();

        for (int i = 0; i < spawnedSurfaceObjects.Count; i++)
        {
            //Debug.Log("i2");
            if(spawnedSurfaceObjects[i] != null)
                DestroyImmediate(spawnedSurfaceObjects[i]);
        }
        spawnedSurfaceObjects.Clear();

        for (int i = 0; i < spawnedEnvironmentObjects.Count; i++)
        {
            //Debug.Log("i2");
            if (spawnedEnvironmentObjects[i] != null)
                DestroyImmediate(spawnedEnvironmentObjects[i]);
        }
        spawnedEnvironmentObjects.Clear();

        placementPosition = Vector3.zero;
        for(int i = 0; i < chunkPathObjects.Count; i++)
        {
            //Debug.Log("Build i");
            if (chunkPathObjects[i].thisPathChunkPiece != null)
            {
                GameObject g = Instantiate(chunkPathObjects[i].thisPathChunkPiece.gameObject, pathGeometryParent) as GameObject;
                g.transform.position = placementPosition;
                spawnedPathObjects.Add(g);
            }

            if (surfaceChunkObjects.Count > i)
            {
                GameObject surf = null;
                if (surfaceChunkObjects[i].thisSurfaceChunkPiece != null)
                {
                    surf = Instantiate(surfaceChunkObjects[i].thisSurfaceChunkPiece.gameObject, surfaceChunkParent) as GameObject;
                    surf.transform.position = placementPosition;
                    surf.GetComponent<SurfaceChunk>().SpawnAllResourceNodes();
                }
                spawnedSurfaceObjects.Add(surf);
            }

            if (enviroChunkObjects.Count > i)
            {
                GameObject enviro = null;
                if (enviroChunkObjects[i].thisEnviroChunk != null)
                {
                    enviro = Instantiate(enviroChunkObjects[i].thisEnviroChunk.gameObject, environmentChunkParent) as GameObject;
                    enviro.transform.position = placementPosition;
                    enviro.GetComponent<EnvironmentChunk>().SpawnEnvironmentPieces();
                }
                spawnedEnvironmentObjects.Add(enviro);
            }

            //move position forward
            placementPosition += chunkPathObjects[i].thisPathChunkPiece.endRelativeToStart;   //Equal to end point
        }

        //turnPiece.transform.position = placementPosition;
        if(pathEndChunk != null)
        {
            GameObject e = Instantiate(pathEndChunk.gameObject, environmentChunkParent) as GameObject;
            e.transform.position = placementPosition;    //Cap to end of turn piece
            e.GetComponent<EnvironmentChunk>().SpawnEnvironmentPieces();
            spawnedEnvironmentObjects.Add(e);
        }

        //Set up BPP as required
        thisBPP.exitLocations.Clear();
        for (int i = 0; i < spawnedPathObjects.Count; i++)
        {
            PathChunkPiece pcp = spawnedPathObjects[i].GetComponent<PathChunkPiece>();
            if (pcp != null)
            {
                if (pcp.isTurnArea == true)
                {
                    if (thisBPP.exitLocations.Exists(x => x.pathTurnLocation == spawnedPathObjects[i].transform) == false)
                    {
                        PathTurnConditions ptc = new PathTurnConditions();
                        ptc.connectedTurnTriggerArea = spawnedPathObjects[i].GetComponentInChildren<TurnTriggerArea>();
                        ptc.pathTurnLocation = spawnedPathObjects[i].transform;
                        thisBPP.exitLocations.Add(ptc);
                    }
                }
            }
        }
    }
    
}
