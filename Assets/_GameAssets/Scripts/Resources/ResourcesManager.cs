using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/* Equipment will need to be better analysed. Will need to have resource nodes depend on some sort of value most likely since the player may have a certain level of hammer for example.
 * Probably simply a "gather level" that starts at 1 and goes up.
 * Thus the equipment will likely be a new class independent of RunnerResource.
 * */

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
    Wood,
    Stone,
    Crop,
    Ore,
    Fish,
    Food,
    Potion,
    Ability_Equipment
}

public class ResourcesManager : MonoBehaviour {

    //Need to consider a way in which I can set out a better database of resources here.
    //Perhaps broken down into "types" - does make for a lot of lists. Will need to at least compile them at runtime...
    List<RunnerResource> allResources = new List<RunnerResource>();
    [Header("The Database")]
    public List<RunnerResource> woodResources = new List<RunnerResource>();
    public List<RunnerResource> stoneResources = new List<RunnerResource>();
    public List<RunnerResource> cropResources = new List<RunnerResource>();
    public List<RunnerResource> oreResources = new List<RunnerResource>();
    public List<RunnerResource> fishResources = new List<RunnerResource>();

    public List<RunnerResource> potionResources = new List<RunnerResource>();

    //public List<RunnerResource> gatherEquipment = new List<RunnerResource>();

    [Header("Current Run")]
    public List<RunnerResource> currentRunResources = new List<RunnerResource>();

    public ResourceResponsiveUI resourceGameUI;

	// Use this for initialization
	void Start ()
    {
        //Make allresources a total of other lists
        allResources.AddRange(woodResources);
        allResources.AddRange(stoneResources);
        allResources.AddRange(cropResources);
        allResources.AddRange(oreResources);
        allResources.AddRange(fishResources);
        allResources.AddRange(potionResources);
        //allResources.AddRange(gatherEquipment);

	    //Copy all resource types to current run resources
        for(int i = 0; i < allResources.Count; i++)
        {
            currentRunResources.Add(allResources[i]);
            //Problem here - linked values. Need to be copies.
            //currentRunResources[i].resourceVal = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddResource(RunnerResource resourceAdded)
    {
        if (currentRunResources.Exists(x => x.resourceName == resourceAdded.resourceName))
        {
            currentRunResources.Find(x => x.resourceName == resourceAdded.resourceName).resourceVal += resourceAdded.resourceVal;
        }
        else
        {
            currentRunResources.Add(resourceAdded);
        }
        resourceGameUI.ResourcePickedUp(resourceAdded);
    }

    /*
    public void AddResource(ResourceType typeToAdd, int addAmount)
    {
        RunnerResource rr = currentRunResources.Find(x => x.resourceType == typeToAdd);
        rr.resourceVal += addAmount;

        //UpdateUI();
        resourceGameUI.ResourcePickedUp(rr, addAmount);
    }*/

    public void SpendResource(ResourceType typeToAdd, int removeAmount)
    {
        allResources.Find(x => x.resourceType == typeToAdd).resourceVal -= removeAmount;

        //UpdateUI();
    }

    public bool ResourceHasAmount(ResourceType typeToAdd, int checkAmount, bool includeCurrentResources)
    {
        if (allResources.Find(x => x.resourceType == typeToAdd).resourceVal >= checkAmount
            || (includeCurrentResources == true && currentRunResources.Find(x => x.resourceType == typeToAdd).resourceVal >= checkAmount))
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
