using UnityEngine;
using System.Collections;

public class PathTriggerArea : MonoBehaviour {

    public BuiltPathPiece parentPathPiece;
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
                pathBuilder.PathStarted(parentPathPiece, thisMoveDirection);
            }

            pathBuilder.CreateNextPath(parentPathPiece);
        }
    }
}
