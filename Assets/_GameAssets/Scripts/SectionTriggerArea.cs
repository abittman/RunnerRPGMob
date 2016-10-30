using UnityEngine;
using System.Collections;

public class SectionTriggerArea : MonoBehaviour {

    public TownSectionSpawn attachedSection;

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            attachedSection.EnterSectionArea();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            attachedSection.ExitSectionArea();
        }
    }
}
