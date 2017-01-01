using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class RunnerResource
{
    public ResourceType resourceType;
    public string resourceName;
    public int resourceVal = 0;
}

public enum ResourceType
{
    None,
    Gold,
    Wood,
    Stone,
    Axe
}

public class ResourcesManager : MonoBehaviour {

    public List<RunnerResource> allResources = new List<RunnerResource>();

    public List<RunnerResource> currentRunResources = new List<RunnerResource>();

    public ResourceResponsiveUI resourceGameUI;

	// Use this for initialization
	void Start ()
    {
	    //Copy all resource types to current run resources
        for(int i = 0; i < allResources.Count; i++)
        {
            currentRunResources.Add(allResources[i]);
            //Problem here - linked values. Need to be copies.
            currentRunResources[i].resourceVal = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddResource(ResourceType typeToAdd, int addAmount)
    {
        RunnerResource rr = currentRunResources.Find(x => x.resourceType == typeToAdd);
        rr.resourceVal += addAmount;

        //UpdateUI();
        resourceGameUI.ResourcePickedUp(rr, addAmount);
    }

    public void SpendResource(ResourceType typeToAdd, int removeAmount)
    {
        allResources.Find(x => x.resourceType == typeToAdd).resourceVal -= removeAmount;

        //UpdateUI();
    }

    public bool ResourceHasAmount(ResourceType typeToAdd, int checkAmount)
    {
        if (allResources.Find(x => x.resourceType == typeToAdd).resourceVal >= checkAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveHalfTempResources()
    {
        for (int i = 0; i < currentRunResources.Count; i++)
        {
            RunnerResource addRes = allResources.Find(x => x.resourceType == currentRunResources[i].resourceType);
            addRes.resourceVal += (currentRunResources[i].resourceVal / 2);
        }

        currentRunResources.Clear();
    }

    public void SaveAllTempResources()
    {
        for(int i = 0; i< currentRunResources.Count; i++)
        {
            RunnerResource addRes = allResources.Find(x => x.resourceType == currentRunResources[i].resourceType);
            addRes.resourceVal += currentRunResources[i].resourceVal;
        }

        currentRunResources.Clear();
    }
}
