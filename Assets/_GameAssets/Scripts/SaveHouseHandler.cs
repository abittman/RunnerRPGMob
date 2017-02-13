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
    }

    public void ExitingHouse()
    {
        progressMan.SaveProgress();

        //Turn around
        pRunner.gameObject.transform.eulerAngles += new Vector3(0f, 180f, 0f);
        switch(pRunner.currentMoveDirection)
        {
            case MoveDirection.North:
                pRunner.currentMoveDirection = MoveDirection.South;
                break;
            case MoveDirection.East:
                pRunner.currentMoveDirection = MoveDirection.West;
                break;
            case MoveDirection.South:
                pRunner.currentMoveDirection = MoveDirection.North;
                break;
            case MoveDirection.West:
                pRunner.currentMoveDirection = MoveDirection.East;
                break;
        }

        pRunner.StartRunner();
        pInput.ActivateControl();
        saveUIObj.SetActive(false);
    }
}
