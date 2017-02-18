using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBuilding : MonoBehaviour {

    [Header("References")]
    public CraftingBuildingUI thisCraftingUI;
    public ResourcesManager resourcesMan;
    [Space]
    public List<CraftedItem> craftableItems = new List<CraftedItem>();

	// Use this for initialization
	void Start ()
    {
        thisCraftingUI.SetupCraftUI();
	}
	
	public void CraftItem(int itemIndex)
    {
        //Remove required resources
        for(int i = 0; i < craftableItems[itemIndex].resourcesRequiredToCraft.Count; i++)
        {
            resourcesMan.SpendResource(craftableItems[itemIndex].resourcesRequiredToCraft[i]);
        }
        resourcesMan.AddResource(craftableItems[itemIndex].craftedItem, craftableItems[itemIndex].safeItem);
        
        craftableItems[itemIndex].craftedStatus = true;
    }
}
