using UnityEngine;
using System.Collections;

public class TurnTriggerArea : MonoBehaviour {

    public PlayerRunner pRunner;

	// Use this for initialization
	void Start ()
    {
	    if(pRunner == null)
        {
            pRunner = GameObject.Find("PLAYER").GetComponent<PlayerRunner>();
        }
	}
	
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            pRunner.canTurn = true;
        }
    }
}
