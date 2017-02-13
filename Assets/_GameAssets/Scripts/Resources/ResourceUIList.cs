using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIList : MonoBehaviour {

    [Header("References")]
    public ResourcesManager resourcesMan;

    [Header("List properties")]
    public List<ResourceUIElement> allResourceUIElements = new List<ResourceUIElement>();
    public Text simpleResourceList;

    public void DisplayAllResources()
    {
        string s = "";
        for (int i = 0; i < resourcesMan.allResources.Count; i++)
        {
            s += resourcesMan.allResources[i].resourceName + " : " + resourcesMan.allResources[i].resourceVal + " - ";
        }

        simpleResourceList.text = s;
            
    }
}
