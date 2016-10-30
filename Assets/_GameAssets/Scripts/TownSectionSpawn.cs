using UnityEngine;
using System.Collections.Generic;

public class TownSectionSpawn : MonoBehaviour {

    public SpawnManager spawnMan;

    public List<GameObject> spawnedObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnNewObjectOnSection(SpawnObject spawnRef)
    {
        GameObject g = Instantiate(spawnRef.spawnObjPrefab, transform) as GameObject;

        g.transform.localPosition = spawnRef.spawnObjPrefab.transform.position;
        g.transform.localRotation = spawnRef.spawnObjPrefab.transform.rotation;

        ItemPickup[] items = g.GetComponentsInChildren<ItemPickup>();
        foreach (ItemPickup it in items)
        {
            it.SetupPickup(this);
        }
        spawnedObjects.Add(g);
    }

    public void RemoveSpawnedObject(GameObject gameObjectRef)
    {
        if(spawnedObjects.Exists(x => x == gameObjectRef))
        {
            spawnedObjects.Remove(gameObjectRef);
            Destroy(gameObjectRef);
        }
    }

    public void EnterSectionArea()
    {

    }

    public void ExitSectionArea()
    {
        spawnMan.UpdateSection(this);
    }
}
