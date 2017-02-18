using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script handles an event occuring within normal gameplay.
 * The event may (optional?) pause gameplay, and move the player to a location.
 * The event may also set new camera parameters.
 * An event will typically also be associated with a conversation event (attached to the same object).
 * An event also indicates what the player should do after the event has passed (continue on, turn around, etc)?
 * */
public class WorldEvent : MonoBehaviour {

    [Header("References")]
    public WorldEventManager worldEventMan;
    public PlayerRunner pRunner;
    public MainGameplayCamera mainGameCamera;

    public ConversationManager conManager;
    public string associatedConversationID;

    [Space]
    public Transform playerEventWaitLocation;

    public Transform cameraHoldLocation;
    public Transform cameraLookAtLocation;

    public bool fireOnceEvent = true;

    public List<RunnerResource> eventGiveResources = new List<RunnerResource>();
    public List<EquipmentItem> eventGiveEquipments = new List<EquipmentItem>();

    //When event is triggered, do all the things to the player and camera
    //[TODO] Move a lot of this to the world event manager most likely
	public void EventTriggerEntered()
    {
        //Pause player
        //pRunner.StopRunner();
        pRunner.MovePlayerToLocation(playerEventWaitLocation.position);

        //Move camera
        mainGameCamera.WatchObject(cameraHoldLocation.position, cameraLookAtLocation);

        //Fire a conversation
        conManager.ActivateEvent(associatedConversationID);
        //[TODO] setup a listener here? Or pass a reference to this to receive a reply?
        worldEventMan.EventFired(this);
    }

    public void EndEvent()
    {
        //Give items also
        for(int i = 0; i < eventGiveResources.Count; i++)
        {
            worldEventMan.resourcesMan.AddResource(eventGiveResources[i], false);
        }
        for (int i = 0; i < eventGiveEquipments.Count; i++)
        {
            worldEventMan.equipmentMan.ObtainedNewEquipment(eventGiveEquipments[i]);
        }

        //Reactivate normal play
        mainGameCamera.WatchPlayer();
        pRunner.StartRunner();
        if(fireOnceEvent)
        {
            gameObject.SetActive(false);
        }
    }
}
