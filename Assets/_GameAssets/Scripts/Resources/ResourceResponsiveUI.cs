using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ResourceUIElement
{
    public Text resourceNameText;
    public Text resourceValText;

    public int addAmount = 0;

    public RunnerResource associatedResource;

    public float displayTimer;
}

public class ResourceResponsiveUI : MonoBehaviour {

    public List<ResourceUIElement> allResourceTexts = new List<ResourceUIElement>();

    public float timeToDisplayUI;

    // Use this for initialization
    void Start ()
    {
	    for(int i = 0; i < allResourceTexts.Count; i++)
        {
            allResourceTexts[i].resourceNameText.gameObject.SetActive(false);
            allResourceTexts[i].resourceValText.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    for(int i = 0; i < allResourceTexts.Count; i++)
        {
            if(allResourceTexts[i].displayTimer > 0f)
            {
                allResourceTexts[i].displayTimer -= Time.deltaTime;
                
                if(allResourceTexts[i].displayTimer <= 0f)
                {
                    allResourceTexts[i].displayTimer = 0f;
                    allResourceTexts[i].associatedResource = null;
                    allResourceTexts[i].resourceNameText.gameObject.SetActive(false);
                    allResourceTexts[i].resourceValText.gameObject.SetActive(false);
                }
            }
        }
	}

    public void ResourcePickedUp(RunnerResource resource, int addAmount)
    {
        for(int i = 0; i < allResourceTexts.Count; i++)
        {
            //Find first null / inactive text
            if(allResourceTexts[i].associatedResource.resourceType == ResourceType.None)
            {
                allResourceTexts[i].associatedResource = resource;
                allResourceTexts[i].resourceNameText.text = resource.resourceName;
                allResourceTexts[i].resourceValText.text = addAmount.ToString();

                allResourceTexts[i].displayTimer = timeToDisplayUI;

                allResourceTexts[i].resourceNameText.gameObject.SetActive(true);
                allResourceTexts[i].resourceValText.gameObject.SetActive(true);

                break;
            }
        }
    }
}
