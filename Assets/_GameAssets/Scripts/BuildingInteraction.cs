using UnityEngine;
using System.Collections;

public class BuildingInteraction : MonoBehaviour {

    public GameObject uiPanelToActivate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnterInteraction()
    {
        if(uiPanelToActivate != null)
        {
            uiPanelToActivate.SetActive(true);
        }
    }

    public void ExitInteraction()
    {

        if (uiPanelToActivate != null)
        {
            uiPanelToActivate.SetActive(false);
        }
    }
}
