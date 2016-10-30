using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

    public RunnerResource resourceVal;
    public ResourcesManager resourcesMan;
    public TownSectionSpawn parentSection;

	// Use this for initialization
	void Start () {
        resourcesMan = GameObject.Find("ResourcesManager").GetComponent<ResourcesManager>();

    }
	
	public void SetupPickup(TownSectionSpawn section)
    {
        parentSection = section;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            resourcesMan.AddResource(resourceVal.resourceType, resourceVal.resourceVal);
            //parentSection.RemoveSpawnedObject(gameObject.transform.parent.gameObject);
            gameObject.SetActive(false);
        }
    }
}
