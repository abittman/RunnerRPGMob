using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ResourceNodeDataManager")]
public class ResourceNodeDataManager : ScriptableObject {
    [SerializeField]
    private List<ResourceNodeData> resNodeData = new List<ResourceNodeData>();

    public List<ResourceNodeData> resNodeDatas
    {
        get { return resNodeData; }
    }

    public ResourceNodeData GetResourceNodeCollection(AreaTypes collectionAreaType)
    {
        ResourceNodeData rnd = null;

        rnd = resNodeData.Find(x => x.thisAreaType == collectionAreaType);

        return rnd;
    }
}
