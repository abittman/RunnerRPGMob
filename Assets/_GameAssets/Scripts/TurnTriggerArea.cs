using UnityEngine;
using System.Collections.Generic;

public class TurnTriggerArea : MonoBehaviour {

    public bool isTownTurnArea = false;
    public List<BuiltPathPiece> townConnectedPieces = new List<BuiltPathPiece>();

    public List<BuiltPathPiece> proceduralConnectedPieces = new List<BuiltPathPiece>();

    public PlayerRunner pRunner;
    
    public BuiltPathPiece northPiece;
    public BuiltPathPiece southPiece;
    public BuiltPathPiece eastPiece;
    public BuiltPathPiece westPiece;

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
            if (isTownTurnArea)
            {
                for (int i = 0; i < townConnectedPieces.Count; i++)
                {
                    townConnectedPieces[i].PrepareArea(pRunner.transform.position);

                    switch(townConnectedPieces[i].pieceFacingDirection)
                    {
                        case MoveDirection.North:
                            northPiece = townConnectedPieces[i];
                            break;
                        case MoveDirection.East:
                            eastPiece = townConnectedPieces[i];
                            break;
                        case MoveDirection.South:
                            southPiece = townConnectedPieces[i];
                            break;
                        case MoveDirection.West:
                            westPiece = townConnectedPieces[i];
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < proceduralConnectedPieces.Count; i++)
                {
                    proceduralConnectedPieces[i].PrepareArea(pRunner.transform.position);

                    switch (proceduralConnectedPieces[i].pieceFacingDirection)
                    {
                        case MoveDirection.North:
                            northPiece = proceduralConnectedPieces[i];
                            break;
                        case MoveDirection.East:
                            eastPiece = proceduralConnectedPieces[i];
                            break;
                        case MoveDirection.South:
                            southPiece = proceduralConnectedPieces[i];
                            break;
                        case MoveDirection.West:
                            westPiece = proceduralConnectedPieces[i];
                            break;
                    }
                }
            }

            pRunner.PrepareTurn(this);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            pRunner.CancelTurnPrep();
        }
    }
}
