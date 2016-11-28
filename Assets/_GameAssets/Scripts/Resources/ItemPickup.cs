﻿using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

    public RunnerResource resourceVal;
    public ResourcesManager resourcesMan;
    //public TownSectionSpawn parentSection;

    public bool doesRespawn = true;

	// Use this for initialization
	void Start () {
        resourcesMan = GameObject.Find("ResourcesManager").GetComponent<ResourcesManager>();
    }
	
    public void ActivatePickup()
    {
        if (doesRespawn)
        {
            gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            resourcesMan.AddResource(resourceVal.resourceType, resourceVal.resourceVal);
            gameObject.SetActive(false);
        }
    }
}
