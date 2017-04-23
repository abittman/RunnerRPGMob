using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    public ProgressManager progressMan;

    [Header("Gameplay references")]
    public PathPoolManager ppMan;
    public PathBuilderv2 pathBuilder;
    public PlayerRunner pRunner;

	// Use this for initialization
	void Start ()
    {
        progressMan.LoadProgress();

        ppMan.DeactivateAllPoolPieces();
        ppMan.SetStartingArea(AreaTypes.Town);

        pRunner.SetupRunner(ppMan.currentBuiltPathPiece, ppMan.currentPathPoolGroup.thisPathedArea.fixedAreaStartLocation);
	}

}
