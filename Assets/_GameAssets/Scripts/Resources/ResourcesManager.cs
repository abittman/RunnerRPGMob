using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class RunnerResource
{
    public ResourceType resourceType;
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

    public Text coinText;
    public Text woodText;
    public Text stoneText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddResource(ResourceType typeToAdd, int addAmount)
    {
        allResources.Find(x => x.resourceType == typeToAdd).resourceVal += addAmount;

        UpdateUI();
    }

    public void SpendResource(ResourceType typeToAdd, int removeAmount)
    {
        allResources.Find(x => x.resourceType == typeToAdd).resourceVal += removeAmount;

        UpdateUI();
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

    void UpdateUI()
    {
        coinText.text = allResources.Find(x => x.resourceType == ResourceType.Gold).resourceVal.ToString();
        woodText.text = allResources.Find(x => x.resourceType == ResourceType.Wood).resourceVal.ToString();
        stoneText.text = allResources.Find(x => x.resourceType == ResourceType.Stone).resourceVal.ToString();
    }
}
