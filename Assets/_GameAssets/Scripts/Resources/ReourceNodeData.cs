using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
    None,
    Tree,
    Stone,
    Crop,
    Ore,
    Essence
}

[System.Serializable]
public class ResourceNodeDataElement
{
    public ResourceNode thisNode;
    public int chance;
}

[CreateAssetMenu(menuName = "Data/ResourceNodeData")]
public class ResourceNodeData : ScriptableObject
{
    //For confirmation?
    public AreaTypes thisAreaType;

    public List<ResourceNodeDataElement> allTreeResourceNodes = new List<ResourceNodeDataElement>();

    public ResourceNode GetRandomResourceNode(ResourceNodeType nodeType)
    {
        ResourceNode rn = null;
        List<ResourceNodeDataElement> thisList = new List<ResourceNodeDataElement>();
        switch(nodeType)
        {
            default:
                thisList = allTreeResourceNodes;
                break;
        }

        int chanceSum = 0;
        for (int i = 0; i < thisList.Count; i++)
        {
            chanceSum += thisList[i].chance;
        }
        int randomiser = Random.Range(0, chanceSum + 1);

        int checkSum = 0;
        for(int i = 0; i < thisList.Count; i++)
        {
            if(randomiser <= thisList[i].chance)
            {
                rn = thisList[i].thisNode;
                break;
            }
            else
            {
                checkSum += thisList[i].chance;
            }
        }

        return rn;
    }
}
