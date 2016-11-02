using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InteractionTrigger : MonoBehaviour {

    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerExitEvent;

    public BuildingInteraction building;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(triggerEnterEvent != null)
            {
                triggerEnterEvent.Invoke();
            }
            //building.EnterInteraction();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //building.ExitInteraction();
            if(triggerExitEvent != null)
            {
                triggerExitEvent.Invoke();
            }
        }
    }
}
