using UnityEngine;
using System.Collections;

public class SectionTriggerArea : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
        }
    }
}
