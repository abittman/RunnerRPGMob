using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameSaveData
{
    //Save resources
    public List<RunnerResource> savedResources = new List<RunnerResource>();

    //Save fixed area status
    public List<PathPoolGroup_Data> ppGroups = new List<PathPoolGroup_Data>();

    //And save path progress states
    //[TODO]
}

public class ProgressManager : MonoBehaviour {

    [Header("References")]
    public ResourcesManager resourcesMan;
    public PathPoolManager pathPoolMan;
    //public WorldEventManager worldEventMan;

    [Header("Progress stats")]
    [Range(0, 1)]
    public float percentageToSave = 0f;

    [Header("Game Data")]
    public GameSaveData gameSaveData;

    public bool saveFreshDataOnStart = false;

    public void LoadProgress()
    {
        if(saveFreshDataOnStart == true)
        {
            SaveData();
        }

        ReadData();
    }

    public void SaveProgress()
    {
        Debug.Log("Saving progress");
        //Scores??

        //Resources
        resourcesMan.SaveAllTempResources();
        SaveData();
    }

    public void LoseProgress()
    {
        resourcesMan.SavePercentageOfTempResources(percentageToSave);
        SaveData();
    }

    public void ReadData()
    {
        //Read
        if (File.Exists(Application.dataPath + "/gameSaveData.gd"))
        {
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("Reading data from " + Application.dataPath + "/gameSaveData.gd");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/gameSaveData.gd", FileMode.Open);
            gameSaveData = (GameSaveData)bf.Deserialize(file);
            file.Close();

            //Set appropriate variables
            resourcesMan.SetupResources(gameSaveData.savedResources);

            pathPoolMan.SetupAllStartData(gameSaveData.ppGroups);
        }
        else
        {
            Debug.Log("No saved data available");
            pathPoolMan.InitialPathSetup();
        }
    }

    public void SaveData()
    {
        //Prepare save data
        //Save resources
        gameSaveData.savedResources = resourcesMan.allResources;

        //Save path areas (nodes)
        List<PathPoolGroup_Data> ppg_DataList = new List<PathPoolGroup_Data>();
        for(int i = 0; i < pathPoolMan.allPathedAreas.Count; i++)
        {
            ppg_DataList.Add(pathPoolMan.allPathedAreas[i].GetPPG_Data());
        }
        gameSaveData.ppGroups = ppg_DataList;

        //Write data to file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/gameSaveData.gd");
        bf.Serialize(file, gameSaveData);
        file.Close();
        Debug.Log("Data saved to " + Application.dataPath + "/gameSaveData.gd");
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();

        resourcesMan.allResources.Clear();

        SaveData();
        ReadData();
    }
}
