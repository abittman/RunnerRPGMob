using UnityEngine;
using System.Collections;

public class SaveHouseHandler : MonoBehaviour {

    public PlayerSwipeInput pInput;
    public PlayerRunner pRunner;

    public GameObject saveUIObj;

    public ProgressManager progressMan;

    public void EnteringHouse()
    {
        pInput.DeactivateControl();
        saveUIObj.SetActive(true);
    }

    public void InHouse()
    {
        pRunner.StopRunner();
        pRunner.gameObject.transform.eulerAngles += new Vector3(0f, 180f, 0f);
    }

    public void ExitingHouse()
    {
        progressMan.SaveProgress();
        pRunner.StartRunner();
        pInput.ActivateControl();
        saveUIObj.SetActive(false);
    }
}
