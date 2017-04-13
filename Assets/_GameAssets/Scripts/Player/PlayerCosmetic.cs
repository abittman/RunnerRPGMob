using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCosmetic : MonoBehaviour {

    public GameObject sproutObj;
    public bool showSprout = false;

    void Start()
    {
        if(PlayerPrefs.HasKey("SproutActive"))
        {
            showSprout = true;
        }

        SetupAppearance();
    }

    public void SetupAppearance()
    {
        if(showSprout)
        {
            sproutObj.SetActive(true);
        }
        else
        {
            sproutObj.SetActive(false);
        }
    }

    public void SproutIsActive()
    {
        PlayerPrefs.SetInt("SproutActive", 0);
    }
}
