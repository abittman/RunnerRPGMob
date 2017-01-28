using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Are effects handled here? Thoughts on effects:
 *      - Healing potion needs to talk to player status
 *      - Shield or speed potion talks to player movement or status
 *      - Ability equipment creates a demonstratable and world effect:
 *          - Wind sickle cuts all grass in a range around player
 *          - Cloud walk keeps player in air by creating ground underneath them
 * */

public class PlayerUsableActionItem : MonoBehaviour {

    public ResourcesManager resourceMan;

    public string usableItem1Name;
    public string usableItem2Name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UseItem1()
    {
        Debug.Log("use item 1");
    }

    public void UseItem2()
    {
        Debug.Log("use item 2");
    }
}
