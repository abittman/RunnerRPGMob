using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour {

    public string thisScreenID;
    public GameObject uiObj;

	public virtual void ActivateScreen()
    {
        uiObj.SetActive(true);
    }

    public virtual void DeactivateScreen()
    {
        uiObj.SetActive(false);
    }
}
