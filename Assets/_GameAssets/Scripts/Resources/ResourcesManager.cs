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
    public Sprite associatedIcon;
}

public enum ResourceType
{
    None,
    Wood,
    Stone,
    Organic_Material,
    Ore,
    Magic,
    Golem
}

public class ResourcesManager : MonoBehaviour {

    //Need to consider a way in which I can set out a better database of resources here.
    //Perhaps broken down into "types" - does make for a lot of lists. Will need to at least compile them at runtime...
    public List<RunnerResource> allResources = new List<RunnerResource>();
    [Header("The Database")]
    public List<RunnerResource> woodResources = new List<RunnerResource>();
    public List<RunnerResource> stoneResources = new List<RunnerResource>();
    public List<RunnerResource> organicResources = new List<RunnerResource>();
    public List<RunnerResource> oreResources = new List<RunnerResource>();
    public List<RunnerResource> magicResources = new List<RunnerResource>();

    //public List<RunnerResource> potionResources = new List<RunnerResource>();
    public List<RunnerResource> golems = new List<RunnerResource>();

    public List<RunnerResource> keyItems = new List<RunnerResource>();

    [Header("Current Run")]
    public List<RunnerResource> currentRunResources = new List<RunnerResource>();

    public ResourceResponsiveUI resourceGameUI;

	// Use this for initialization
	void Start ()
    {
        //Make allresources a total of other lists
        allResources.AddRange(woodResources);
        allResources.AddRange(stoneResources);
        allResources.AddRange(organicResources);
        allResources.AddRange(oreResources);
        allResources.AddRange(magicResources);
        //allResources.AddRange(potionResources);
        allResources.AddRange(golems);
        allResources.AddRange(keyItems);
	}

    public void AddResource(RunnerResource resourceAdded, bool saveToMain)
    {
        if (saveToMain == false)
        {
            if (currentRunResources.Exists(x => x.resourceName == resourceAdded.resourceName))
            {
                currentRunResources.Find(x => x.resourceName == resourceAdded.resourceName).resourceVal += resourceAdded.resourceVal;
            }
            else
            {
                currentRunResources.Add(resourceAdded);
            }
        }
        else
        {
            if (allResources.Exists(x => x.resourceName == resourceAdded.resourceName))
            {
                allResources.Find(x => x.resourceName == resourceAdded.resourceName).resourceVal += resourceAdded.resourceVal;
            }
            else
            {
                allResources.Add(resourceAdded);
            }
        }
        resourceGameUI.ResourcePickedUp(resourceAdded);
    }

    public void SpendResource(ResourceType typeToAdd, int removeAmount)
    {
        allResources.Find(x => x.resourceType == typeToAdd).resourceVal -= removeAmount;
        //Check current and all? Take out of current first?
    }

    public void SpendResource(RunnerResource spendRes)
    {
        int amountRemaining = spendRes.resourceVal;
        //Subtract from current first
        RunnerResource currRef = currentRunResources.Find(x => x.resourceName == spendRes.resourceName);

        if(currRef != null)
        {
            if(currRef.resourceVal >= amountRemaining)
            {
                currRef.resourceVal -= amountRemaining;
                amountRemaining = 0;
            }
            else
            {
                amountRemaining -= currRef.resourceVal;
                currRef.resourceVal -= currRef.resourceVal;
            }
        }

        //If there is remaining, now check all resources
        if (amountRemaining > 0)
        {
            RunnerResource allRef = allResources.Find(x => x.resourceName == spendRes.resourceName);
            if (allRef != null)
            {
                if(allRef.resourceVal >= amountRemaining)
                {
                    allRef.resourceVal -= amountRemaining;
                }
                else
                {
                    Debug.LogError("[RESOURCE] attempting to spend more resources than are available");
                }
            }
        }
    }

    public bool ResourceHasAmount(RunnerResource resourceCheck, bool includeCurrentResources)
    {
        RunnerResource allRef = allResources.Find(x => x.resourceName == resourceCheck.resourceName);
        RunnerResource currRef = currentRunResources.Find(x => x.resourceName == resourceCheck.resourceName);

        int currentAmount = 0;
        //If item exists in both, add together
        if(allRef != null)
        {
            currentAmount += allRef.resourceVal;
        }
        if(currRef != null)
        {
            currentAmount += currRef.resourceVal;
        }

        //Debug.Log("Check if there are " + resourceCheck.resourceVal + " " + resourceCheck.resourceName
       //             + ". \n There are " + currentAmount + " found in all and current");

        if(currentAmount >= resourceCheck.resourceVal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SavePercentageOfTempResources(float savePercentage)
    {
        for (int i = 0; i < currentRunResources.Count; i++)
        {
            RunnerResource addRes = allResources.Find(x => x.resourceType == currentRunResources[i].resourceType);
            int amountToAdd = (int)((float)currentRunResources[i].resourceVal * savePercentage);
            if (amountToAdd > 0)
            {
                if (addRes != null)
                {
                    addRes.resourceVal += amountToAdd;
                }
                else
                {
                    allResources.Add(currentRunResources[i]);
                }
            }
        }

        currentRunResources.Clear();
    }

    public void SaveAllTempResources()
    {
        for(int i = 0; i< currentRunResources.Count; i++)
        {
            RunnerResource addRes = allResources.Find(x => x.resourceType == currentRunResources[i].resourceType);
            if (addRes != null)
            {
                addRes.resourceVal += currentRunResources[i].resourceVal;
            }
            else
            {
                allResources.Add(currentRunResources[i]);
            }
        }

        currentRunResources.Clear();
    }

    //[TODO] Make a unique item drop on death that a player can retrieve at a later date
}
