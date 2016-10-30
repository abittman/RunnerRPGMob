using UnityEngine;
using System.Collections;

public class PathTriggerArea : MonoBehaviour {

    public PathBuilder pathBuilder;
    public MoveDirection thisMoveDirection;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(pathBuilder.currentPathProgress == 0)
            {
                pathBuilder.PathStarted(transform.parent.parent.gameObject, thisMoveDirection);
            }

            pathBuilder.CreateNextPath(transform.parent.parent.gameObject);
        }
    }
}
