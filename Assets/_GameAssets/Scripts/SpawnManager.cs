using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnObject
{
    public string spawnID = "";
    public GameObject spawnObjPrefab;
}

public class SpawnManager : MonoBehaviour {

    public List<SpawnObject> allPossibleSpawnObjects = new List<SpawnObject>();

    public List<TownSectionSpawn> allSections = new List<TownSectionSpawn>();

    public ResourcesManager resourcesMan;

	// Use this for initialization
	void Start ()
    {
	    foreach(TownSectionSpawn sect in allSections)
        {
            UpdateSection(sect);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateSection(TownSectionSpawn sectionToUpdate)
    {
        int randomInt = Random.Range(0, allPossibleSpawnObjects.Count);
        //Debug.Log("Add " + allPossibleSpawnObjects[randomInt].spawnObjPrefab + " to " + sectionToUpdate.gameObject.name);
        sectionToUpdate.SpawnNewObjectOnSection(allPossibleSpawnObjects[randomInt]);
    }
}
