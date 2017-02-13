using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour {

    public ResourcesManager resourcesMan;
    public EquipmentManager equipmentMan;

    public WorldEvent currentEventRef;

    public GameObject eventInteractionPanel;

	public void EventFired(WorldEvent thisEvent)
    {
        currentEventRef = thisEvent;
        eventInteractionPanel.SetActive(true);
    }

    public void EndCurrentEvent()
    {
        if(currentEventRef != null)
        {
            currentEventRef.EndEvent();
            currentEventRef = null;
            eventInteractionPanel.SetActive(false);
        }
    }
}
