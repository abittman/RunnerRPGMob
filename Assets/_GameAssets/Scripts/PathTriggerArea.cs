using UnityEngine;
using System.Collections;

public class PathTriggerArea : MonoBehaviour {

    public BuiltPathPiece parentPathPiece;
    //public PathBuilder pathBuilder;
    public PathBuilderv2 pathBuilder;
    public MoveDirection thisMoveDirection;

	// Use this for initialization
	void Start ()
    {
        //[TODO] no find
        pathBuilder = GameObject.Find("WorldBuilder").GetComponent<PathBuilderv2>();
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            pathBuilder.PlayerEntersNewPiece(parentPathPiece);
            //[TODO] replace with new system
        }
    }
}
