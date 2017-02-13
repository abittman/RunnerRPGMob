using UnityEngine;
using System.Collections;

public class ProgressManager : MonoBehaviour {

    [Header("References")]
    public ResourcesManager resourcesMan;

    [Header("Progress stats")]
    [Range(0, 1)]
    public float percentageToSave = 0f;

    public void SaveProgress()
    {
        Debug.Log("Saving progress");
        //Scores??

        //Resources
        resourcesMan.SaveAllTempResources();
    }

    public void LoseProgress()
    {
        resourcesMan.SavePercentageOfTempResources(percentageToSave);
    }
}
