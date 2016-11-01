using UnityEngine;
using System.Collections;

public class SaveHouseHandler : MonoBehaviour {

    public PlayerSwipeInput pInput;
    public PlayerRunner pRunner;

    public GameObject saveUIObj;

    public ProgressManager progressMan;

    public void EnteringHouse()
    {
        pInput.canControl = false;
        saveUIObj.SetActive(true);
    }

    public void InHouse()
    {
        pRunner.doMove = false;
        pRunner.gameObject.transform.eulerAngles += new Vector3(0f, 180f, 0f);
    }

    public void ExitingHouse()
    {
        progressMan.SaveProgress();
        pRunner.doMove = true;
        pInput.canControl = true;
        saveUIObj.SetActive(false);
    }
}
