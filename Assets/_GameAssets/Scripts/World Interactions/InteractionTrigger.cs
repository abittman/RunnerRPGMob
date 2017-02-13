using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InteractionTrigger : MonoBehaviour {

    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerExitEvent;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(triggerEnterEvent != null)
            {
                triggerEnterEvent.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(triggerExitEvent != null)
            {
                triggerExitEvent.Invoke();
            }
        }
    }
}
