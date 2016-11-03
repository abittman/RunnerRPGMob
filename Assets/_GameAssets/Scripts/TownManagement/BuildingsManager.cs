using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TownBuilding
{
    public string buildingName = "";
    public GameObject buildingObject_unbuilt;
    public GameObject buildingObject_built;

    public List<RunnerResource> requiredBuildingResources = new List<RunnerResource>();

    public void BuildBuilding()
    {
        buildingObject_unbuilt.SetActive(false);
        buildingObject_built.SetActive(true);
    }
}

public class BuildingsManager : MonoBehaviour {

    public ResourcesManager resourcesMan;

    public List<TownBuilding> allTownBuildings = new List<TownBuilding>();

	// Use this for initialization
	void Start ()
    {
	    
	}
	
    public void BuildBuilding(string buildingID)
    {
        //Get building
        TownBuilding buildingToCheck = allTownBuildings.Find(x => x.buildingName == buildingID);

        if (CanBuildTownBuilding(buildingToCheck))
        {
            Debug.Log("Build it!");
            for(int i = 0; i<buildingToCheck.requiredBuildingResources.Count; i++)
            {
                resourcesMan.SpendResource(buildingToCheck.requiredBuildingResources[i].resourceType, buildingToCheck.requiredBuildingResources[i].resourceVal);
            }
            buildingToCheck.BuildBuilding();
        }
        else
        {
            Debug.Log("Not enough resources");
        }
    }

    public bool CanBuildTownBuilding(TownBuilding townBuildingRef)
    {
        bool canBuild = false;


        for (int i = 0; i < townBuildingRef.requiredBuildingResources.Count; i++)
        {
            if (resourcesMan.ResourceHasAmount(townBuildingRef.requiredBuildingResources[i].resourceType, townBuildingRef.requiredBuildingResources[i].resourceVal))
            {
                canBuild = true;
            }
            else
            {
                canBuild = false;
            }
        }
        return canBuild;
    }
}
