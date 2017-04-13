using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SurfaceChunkData
{
    public SurfaceChunk thisSurfaceChunkPiece;
}

[System.Serializable]
public class SurfaceChunkNode
{
    public Transform nodeTransform;
    public ResourceNode resourceNode;
    public GameObject otherObject;

    public GameObject spawnedObj;
}

public class SurfaceChunk : MonoBehaviour {

    public List<SurfaceChunkNode> surfaceNodes = new List<SurfaceChunkNode>();

    //Should perhaps resource nodes only be spawned in play?? Let's do that for now
    public void SpawnAllResourceNodes()
    {
        for(int i = 0; i < surfaceNodes.Count; i++)
        {
            if(surfaceNodes[i].spawnedObj != null)
            {
                DestroyImmediate(surfaceNodes[i].spawnedObj);
                surfaceNodes[i].spawnedObj = null;
            }

            /*
            if(surfaceNodes[i].resourceNode != null)
            {
                surfaceNodes[i].spawnedObj = Instantiate(surfaceNodes[i].resourceNode.gameObject, surfaceNodes[i].nodeTransform);
                //Reset position required??
                surfaceNodes[i].spawnedObj.transform.localPosition = Vector3.zero;
            }
            */
        }
    }
}
