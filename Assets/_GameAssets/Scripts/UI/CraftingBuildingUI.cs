using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingBuildingUI : MonoBehaviour {

    public CraftingBuilding thisBuilding;

    public List<GameObject> craftItemPanelObjects = new List<GameObject>();

    public Text selectedItemName;
    public Image selectedItemImage;
    public Text requiredItemText_1;
    public Text requiredItemText_2;
    public Text requiredItemText_3;
    public Text requiredItemText_4;
    public Button craftItemButton;

    public int currentItemID;

    public void SetupCraftUI()
    {
        for(int i = 0; i < thisBuilding.craftableItems.Count; i++)
        {
            craftItemPanelObjects[i].SetActive(true);
            craftItemPanelObjects[i].GetComponentInChildren<Text>().text = thisBuilding.craftableItems[i].craftedItem.resourceName;
        }

        CraftItemButtonPressed(0);
    }

    public void CraftItemButtonPressed(int buttonID)
    {
        //selectedItemName.text = thisBuilding.craftableItems[buttonID].craftedItem.resourceName;
        selectedItemImage.sprite = thisBuilding.craftableItems[buttonID].craftedItem.associatedIcon;

        requiredItemText_1.text = thisBuilding.craftableItems[buttonID].resourcesRequiredToCraft[0].resourceName;
        requiredItemText_2.text = thisBuilding.craftableItems[buttonID].resourcesRequiredToCraft[1].resourceName;
        requiredItemText_3.text = thisBuilding.craftableItems[buttonID].resourcesRequiredToCraft[2].resourceName;
        requiredItemText_4.text = thisBuilding.craftableItems[buttonID].resourcesRequiredToCraft[3].resourceName;

        currentItemID = buttonID;

        if (thisBuilding.craftableItems[buttonID].craftedStatus == true
            && thisBuilding.craftableItems[buttonID].oneCraftOnly == true)
        {
            craftItemButton.interactable = false;
        }
        else
        {
            craftItemButton.interactable = true;

            for (int i = 0; i < thisBuilding.craftableItems[buttonID].resourcesRequiredToCraft.Count; i++)
            {
                if (thisBuilding.resourcesMan.ResourceHasAmount(thisBuilding.craftableItems[buttonID].resourcesRequiredToCraft[i], true))
                {
                    //True, continue
                }
                else
                {
                    //False, break
                    craftItemButton.interactable = false;
                    break;
                }
            }
        }
    }

    public void DoCraftItemButtonPressed()
    {
        //Call item crafter
        thisBuilding.CraftItem(currentItemID);

        //[TODO] Handle change in UI now that item has been crafted.
        //If one shot, disable button.
        //Show made item if sillhouette
        //Update numbers on item and all that jazz
        //Play animation if applicable or something?
        if (thisBuilding.craftableItems[currentItemID].craftedStatus == true
            && thisBuilding.craftableItems[currentItemID].oneCraftOnly == true)
        {
            craftItemButton.interactable = false;
        }
    }
}
