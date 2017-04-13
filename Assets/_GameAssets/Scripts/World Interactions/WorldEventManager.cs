using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldEvent_Data
{
    public string eventID;
    public bool hasOccured;
}

public enum NarrativeProgress
{
    None = 0,
    Sprout_Find = 10,
    Sprout_Create = 20
}

public class WorldEventManager : MonoBehaviour {

    public ResourcesManager resourcesMan;
    public EquipmentManager equipmentMan;
    public PlayerCosmetic cosmeticMan;

    public WorldEvent currentEventRef;

    public GameObject eventInteractionPanel;

    public NarrativeProgress currentProgress;

    void Start()
    {
        if(PlayerPrefs.HasKey("Current_Narr_Prog"))
        {
            int currProg = PlayerPrefs.GetInt("Current_Narr_Prog");
            currentProgress = (NarrativeProgress)currProg;
            SetWorldToNarrativeProgress();
        }
    }

	public void EventFired(WorldEvent thisEvent)
    {
        currentEventRef = thisEvent;
        eventInteractionPanel.SetActive(true);
    }

    public void EndCurrentEvent()
    {
        if(currentEventRef != null)
        {
            if(currentEventRef.progressEvent != NarrativeProgress.None)
            {
                if((int)currentEventRef.progressEvent > (int)currentProgress)
                {
                    currentProgress = currentEventRef.progressEvent;
                    PlayerPrefs.SetInt("Current_Narr_Prog", (int)currentProgress);
                    SetWorldToNarrativeProgress();
                }
            }

            currentEventRef.EndEvent();
            currentEventRef = null;
            eventInteractionPanel.SetActive(false);
        }
    }

    void SetWorldToNarrativeProgress()
    {
        switch(currentProgress)
        {
            case NarrativeProgress.Sprout_Create:
                cosmeticMan.SproutIsActive();
                cosmeticMan.SetupAppearance();
                break;
        }
    }
}
