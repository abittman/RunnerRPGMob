using UnityEngine;
using System.Collections;

public class PlayerFXController : MonoBehaviour {

    public GameObject swordTrail;

    void Start()
    {
        StopAttackFX();
    }

    public void StopAttackFX()
    {
        swordTrail.SetActive(false);
    }

	public void DoAttackFX()
    {
        swordTrail.SetActive(true);
    }
}
