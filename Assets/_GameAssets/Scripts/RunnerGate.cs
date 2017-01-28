using UnityEngine;
using System.Collections.Generic;

public class RunnerGate : MonoBehaviour {

    public RunnerResource requiredResource;

    public List<GameObject> collisionObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            if (GameObject.Find("ResourcesManager").GetComponent<ResourcesManager>().ResourceHasAmount(requiredResource.resourceType, requiredResource.resourceVal, true))
            {
                foreach(GameObject g in collisionObjects)
                {
                    g.SetActive(false);
                }
            }
        }
    }
}
