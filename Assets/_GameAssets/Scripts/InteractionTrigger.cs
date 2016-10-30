using UnityEngine;
using System.Collections;

public class InteractionTrigger : MonoBehaviour {

    public BuildingInteraction building;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            building.EnterInteraction();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            building.ExitInteraction();
        }
    }
}
