using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TownManagementManager : MonoBehaviour {

    public GameObject mainMenuObj;
    public GameObject buildingUpgradeUIObj;

	// Use this for initialization
	void Start ()
    {
	}
	
	public void OpenBuildingUpgradeObj(GameObject buildingReference)
    {
        buildingUpgradeUIObj.SetActive(true);
        mainMenuObj.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        gameObject.SetActive(true);
        mainMenuObj.SetActive(true);
        buildingUpgradeUIObj.SetActive(false);
    }

    public void CloseMainMenu()
    {
        gameObject.SetActive(false);
    }
}
