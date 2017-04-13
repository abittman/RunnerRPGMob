using UnityEngine;
using System.Collections.Generic;
/*
public enum MoveDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
*/

public enum RunningLane
{
    None,
    Left,
    Mid,
    Right
}

public class PlayerRunner : MonoBehaviour {

    [Header("References")]
    public PlayerEventHandler pEventHandler;
    public MainGameplayCamera gameCamera;

    [Space]
    public PlayerCombat pCombat;
    public PlayerAnimationController pAniController;

    public Rigidbody playerRB;

    public float moveSpeed = 10f;
    public float tiltSpeed = 1f;

    public float jumpForce = 100f;
    public float slideAirDownForce = 100f;

    public bool doMove = true;

    public bool canTurn = false;
    public MoveDirection currentMoveDirection;
    public RunningLane currentLane;

    public GameObject meshObject;

    public LayerMask groundingMask;
    bool isGrounded = false;
    RaycastHit hit;

    //Lane changing variables
    Vector3 currentMidLanePos = Vector3.zero;
    public float laneChangeTransitionTime = 0.2f;
    float laneChangeTimer = 0f;
    public float laneChangeDistance = 2f;
    bool doLaneChange = false;
    float startX;
    float goalX;
    float startZ;
    float goalZ;

    public float bumpBackTransitionTime = 0.2f;
    float bumpBackTimer = 0f;
    bool doBumpBack = false;

    public Vector3 nextLeftMidPos;
    public Vector3 nextRightMidPos;
    Vector3 lastMidPos;
    bool lastTurnIsLeft = false;
    bool lastTurnIsRight = false;

    //Sliding
    bool doSliding = false;
    public float slideTimer = 1f;
    float currentSlideTimer = 0f;

    //Building travel
    BuildingInteraction currentBuildingRef;
    public bool moveIntoBuilding = false;
    bool moveOutOfBuilding = false;

    public bool movingToPoint = false;
    Vector3 goalLocation;

    // Use this for initialization
    void Start ()
    {
        currentMoveDirection = MoveDirection.North;
        currentLane = RunningLane.Mid;
        currentMidLanePos.x = -40f;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(doMove)
        {
            AutoRun();
            if(doLaneChange)
            {
                LaneChangeRunner();
            }
            if(doBumpBack)
            {
                BumpBackRunner();
            }
            if(doSliding)
            {
                currentSlideTimer += Time.deltaTime;
                if(currentSlideTimer >= slideTimer)
                {
                    currentSlideTimer = 0f;
                    doSliding = false;
                    pAniController.StraightRunAnimation();
                }
            }
        }
        else if(moveIntoBuilding)
        {
            MoveToBuildingPoint();
        }
        else if(moveOutOfBuilding)
        {
            MoveAwayFromBuilding();
        }
        else if(movingToPoint)
        {
            MoveToPoint();
        }
	}

    public void StopRunner()
    {
        doMove = false;
        playerRB.isKinematic = true;
        playerRB.useGravity = false;
    }

    public void StartRunner()
    {
        doMove = true;

        playerRB.isKinematic = false;
        playerRB.useGravity = true;
    }

    void AutoRun()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;

        //Check grounding
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2.5f, groundingMask))
        {
            //Is grounded
            if(isGrounded == false)
            {
                pAniController.StraightRunAnimation();
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //Check if attacking ahead
        if(pCombat.CanDoFrontAttack())
        {
            pCombat.DoFrontAttack();
        }
    }

    public void DoLeft()
    {
        if (currentBuildingRef != null)
        {
            TurnIntoBuilding();
        }
        else if (canTurn)
        {
            TurnRunner(-1f);
        }
        else
        {
            //Clear right enemy as you move away
            //pCombat.rightEnemy = null;

            RunningLane lastLane = currentLane;
            switch (currentLane)
            {
                case RunningLane.Left:
                    doLaneChange = false;
                    break;
                case RunningLane.Mid:
                    currentLane = RunningLane.Left;
                    doLaneChange = true;
                    break;
                case RunningLane.Right:
                    currentLane = RunningLane.Mid;
                    doLaneChange = true;
                    break;
            }

            if (doLaneChange)
            {
                pAniController.LeftLaneChangeAnimation();

                switch (currentMoveDirection)
                {
                    case MoveDirection.North:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startX = currentMidLanePos.x;
                                break;
                            case RunningLane.Right:
                                startX = currentMidLanePos.x + laneChangeDistance;
                                break;
                        }
                        goalX = startX - laneChangeDistance;
                        break;
                    case MoveDirection.South:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startX = currentMidLanePos.x;
                                break;
                            case RunningLane.Right:
                                startX = currentMidLanePos.x - laneChangeDistance;
                                break;
                        }
                        goalX = startX + laneChangeDistance;
                        break;
                    case MoveDirection.West:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startZ = currentMidLanePos.z;
                                break;
                            case RunningLane.Right:
                                startZ = currentMidLanePos.z + laneChangeDistance;
                                break;
                        }
                        goalZ = startZ - laneChangeDistance;
                        break;
                    case MoveDirection.East:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startZ = currentMidLanePos.z;
                                break;
                            case RunningLane.Right:
                                startZ = currentMidLanePos.z - laneChangeDistance;
                                break;
                        }
                        goalZ = startZ + laneChangeDistance;
                        break;
                }
            }
            
        }
    }

    public void DoRight()
    {
        if (currentBuildingRef != null)
        {
            TurnIntoBuilding();
        }
        else if (canTurn)
        {
            TurnRunner(1f);
        }
        else
        {
                //Clear left Enemy as you move away

                RunningLane lastLane = currentLane;

                switch (currentLane)
                {
                    case RunningLane.Left:
                        currentLane = RunningLane.Mid;
                        doLaneChange = true;
                        break;
                    case RunningLane.Mid:
                        currentLane = RunningLane.Right;
                        doLaneChange = true;
                        break;
                    case RunningLane.Right:
                        doLaneChange = false;
                        break;
                }

            if (doLaneChange)
            {
                pAniController.RightLaneChangeAnimation();
                switch (currentMoveDirection)
                {
                    case MoveDirection.North:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startX = currentMidLanePos.x;
                                break;
                            case RunningLane.Left:
                                startX = currentMidLanePos.x - laneChangeDistance;
                                break;
                        }
                        goalX = startX + laneChangeDistance;
                        break;
                    case MoveDirection.South:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startX = currentMidLanePos.x;
                                break;
                            case RunningLane.Left:
                                startX = currentMidLanePos.x + laneChangeDistance;
                                break;
                        }
                        goalX = startX - laneChangeDistance;
                        break;
                    case MoveDirection.West:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startZ = currentMidLanePos.z;
                                break;
                            case RunningLane.Left:
                                startZ = currentMidLanePos.z - laneChangeDistance;
                                break;
                        }
                        goalZ = startZ + laneChangeDistance;
                        break;
                    case MoveDirection.East:
                        switch (lastLane)
                        {
                            case RunningLane.Mid:
                                startZ = currentMidLanePos.z;
                                break;
                            case RunningLane.Left:
                                startZ = currentMidLanePos.z + laneChangeDistance;
                                break;
                        }
                        goalZ = startZ - laneChangeDistance;
                        break;
                }
            }
            
        }
    }

    void TurnRunner(float inputDir)
    {
        bool isLeftTurn = false;
        bool isRightTurn = false;

        lastMidPos = currentMidLanePos;

        if(inputDir > 0f)
        {
            isRightTurn = true;
            lastTurnIsRight = true;
            currentMidLanePos = nextRightMidPos;
        }
        else
        {
            isLeftTurn = true;
            lastTurnIsLeft = true;
            currentMidLanePos = nextLeftMidPos;
        }

        transform.eulerAngles += new Vector3(0f, inputDir * 90f, 0f);

        if ((int)currentMoveDirection + inputDir < 1)
        {
            currentMoveDirection = MoveDirection.West;
        }
        else if ((int)currentMoveDirection + inputDir > 4)
        {
            currentMoveDirection = MoveDirection.North;
        }
        else
        {
            currentMoveDirection = (MoveDirection)((int)currentMoveDirection + inputDir);
        }
        
        canTurn = false;

        Vector3 currentPos = transform.position;
        Debug.Log("Pre turn pos " + currentPos);

        bool turningOntoPiece = false;
        if(isRightTurn && nextRightMidPos != Vector3.zero)
        {
            turningOntoPiece = true;
            Debug.Log("Right to " + nextRightMidPos);
        }
        else if(isLeftTurn && nextLeftMidPos != Vector3.zero)
        {
            turningOntoPiece = true;
            Debug.Log("Left to " + nextLeftMidPos);
        }

        float distToNextMidLane = 0f;
        float distToNextLeftLane = 0f;
        float distToNextRightLane = 0f;

        if (turningOntoPiece)
        {
            //Teleport for now
            switch (currentMoveDirection)
            {
                case MoveDirection.North:
                    if (isRightTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x - 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x));
                        distToNextRightLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x + 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.x = nextRightMidPos.x + 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.x = nextRightMidPos.x - 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.x = nextRightMidPos.x;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    else if (isLeftTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x - 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x));
                        distToNextRightLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x + 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.x = nextLeftMidPos.x + 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.x = nextLeftMidPos.x - 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.x = nextRightMidPos.x;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    break;
                case MoveDirection.East:
                    if (isRightTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z + 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z));
                        distToNextRightLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z - 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.z = nextRightMidPos.z - 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.z = nextRightMidPos.z + 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.z = nextRightMidPos.z;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    else if (isLeftTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z + 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z));
                        distToNextRightLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z - 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.z = nextLeftMidPos.z - 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.z = nextLeftMidPos.z + 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.z = nextRightMidPos.z;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    break;
                case MoveDirection.South:
                    if (isRightTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x + 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x));
                        distToNextRightLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x - 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.x = nextRightMidPos.x - 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.x = nextRightMidPos.x + 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.x = nextRightMidPos.x;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    else if (isLeftTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x + 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x));
                        distToNextRightLane = Mathf.Abs(currentPos.x - (nextLeftMidPos.x - 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.x = nextLeftMidPos.x - 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.x = nextLeftMidPos.x + 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.x = nextRightMidPos.x;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    break;
                case MoveDirection.West:
                    if (isRightTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.z - (nextRightMidPos.z - 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.z - (nextRightMidPos.z));
                        distToNextRightLane = Mathf.Abs(currentPos.z - (nextRightMidPos.z + 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.z = nextRightMidPos.z + 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.z = nextRightMidPos.z - 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.z = nextRightMidPos.z;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    else if (isLeftTurn)
                    {
                        distToNextLeftLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z - 2f));
                        distToNextMidLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z));
                        distToNextRightLane = Mathf.Abs(currentPos.z - (nextLeftMidPos.z + 2f));
                        //Debug.Log("Left " + distToNextLeftLane + " mid " + distToNextMidLane + " right " + distToNextRightLane);

                        if (distToNextRightLane <= distToNextMidLane
                            && distToNextRightLane <= distToNextLeftLane)
                        {
                            currentPos.z = nextLeftMidPos.z + 2f;
                            currentLane = RunningLane.Right;
                        }
                        else if (distToNextLeftLane <= distToNextMidLane
                           && distToNextLeftLane <= distToNextRightLane)
                        {
                            currentPos.z = nextLeftMidPos.z - 2f;
                            currentLane = RunningLane.Left;
                        }
                        else
                        {
                            currentPos.z = nextRightMidPos.z;
                            currentLane = RunningLane.Mid;
                        }
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Don't turn?");
        }
        //Debug.Log("current pos " + currentPos);
        transform.position = currentPos;

        nextLeftMidPos = Vector3.zero;
        nextRightMidPos = Vector3.zero;
    }

    void LaneChangeRunner()
    {
        laneChangeTimer += Time.deltaTime;
        if (currentMoveDirection == MoveDirection.North
            || currentMoveDirection == MoveDirection.South)
        {
            float currentX = Mathf.Lerp(startX, goalX, laneChangeTimer / laneChangeTransitionTime);
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
        }
        else
        {
            float currentZ = Mathf.Lerp(startZ, goalZ, laneChangeTimer / laneChangeTransitionTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, currentZ);
        }

        if(laneChangeTimer >= laneChangeTransitionTime)
        {
            doLaneChange = false;
            laneChangeTimer = 0f;
            pAniController.StraightRunAnimation();
            pCombat.pFXController.StopAttackFX();
            pEventHandler.ChangeLaneEvent(currentLane);
        }
    }

    public void DoJump()
    {
        if (isGrounded)
        {
            JumpRunner();
        }
    }

    void JumpRunner()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        pAniController.JumpAnimation();
        
        doSliding = false;
        currentSlideTimer = 0f;
    }

    public void DoSlide()
    {
        if (isGrounded)
        {
            
            SlideRunner();
            
        }
        else
        {
            SlideRunner();
        }
    }

    void SlideRunner()
    {
        //Setup an animation to handle this
        doSliding = true;
        pAniController.SlideAnimation();
        if(isGrounded == false)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * slideAirDownForce, ForceMode.Impulse);
        }
    }

    public void DoBumpLeft()
    {
        doLaneChange = false;
        doBumpBack = true;

        float tempVal = 0f;
        switch (currentMoveDirection)
        {
            case MoveDirection.North:
                tempVal = startX;
                startX = goalX;
                goalX = tempVal;
                break;
            case MoveDirection.South:
                tempVal = startX;
                startX = goalX;
                goalX = tempVal;
                break;
            case MoveDirection.West:
                tempVal = startZ;
                startZ = goalZ;
                goalZ = tempVal;
                break;
            case MoveDirection.East:
                tempVal = startZ;
                startZ = goalZ;
                goalZ = tempVal;
                break;
        }
    }

    public void DoBumpRight()
    {
        doLaneChange = false;
        doBumpBack = true;

        float tempVal = 0f;
        switch (currentMoveDirection)
        {
            case MoveDirection.North:
                tempVal = startX;
                startX = goalX;
                goalX = tempVal;
                break;
            case MoveDirection.South:
                tempVal = startX;
                startX = goalX;
                goalX = tempVal;
                break;
            case MoveDirection.West:
                tempVal = startZ;
                startZ = goalZ;
                goalZ = tempVal;
                break;
            case MoveDirection.East:
                tempVal = startZ;
                startZ = goalZ;
                goalZ = tempVal;
                break;
        }
    }

    void BumpBackRunner()
    {
        bumpBackTimer += Time.deltaTime;
        if (currentMoveDirection == MoveDirection.North
            || currentMoveDirection == MoveDirection.South)
        {
            float currentX = Mathf.Lerp(startX, goalX, bumpBackTimer / bumpBackTransitionTime);
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
        }
        else
        {
            float currentZ = Mathf.Lerp(startZ, goalZ, bumpBackTimer / bumpBackTransitionTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, currentZ);
        }

        if (laneChangeTimer >= laneChangeTransitionTime)
        {
            doLaneChange = false;
            laneChangeTimer = 0f;
        }
    }

    public void PrepareTurn(TurnTriggerArea triggerAreaRef)
    {
        canTurn = true;

        //Get turndirections
        switch(currentMoveDirection)
        {
            case MoveDirection.North:
                if(triggerAreaRef.eastPiece != null)
                {
                    nextRightMidPos = triggerAreaRef.eastPiece.transform.position;
                }
                if(triggerAreaRef.westPiece != null)
                {
                    nextLeftMidPos = triggerAreaRef.westPiece.transform.position;
                }
                break;
            case MoveDirection.South:
                if (triggerAreaRef.eastPiece != null)
                {
                    nextLeftMidPos = triggerAreaRef.eastPiece.transform.position;
                }
                if (triggerAreaRef.westPiece != null)
                {
                    nextRightMidPos = triggerAreaRef.westPiece.transform.position;
                }
                break;
            case MoveDirection.East:
                if (triggerAreaRef.southPiece != null)
                {
                    nextRightMidPos = triggerAreaRef.southPiece.transform.position;
                }
                if (triggerAreaRef.northPiece != null)
                {
                    nextLeftMidPos = triggerAreaRef.northPiece.transform.position;
                }
                break;
            case MoveDirection.West:
                if (triggerAreaRef.northPiece != null)
                {
                    nextRightMidPos = triggerAreaRef.northPiece.transform.position;
                }
                if (triggerAreaRef.southPiece != null)
                {
                    nextLeftMidPos = triggerAreaRef.southPiece.transform.position;
                }
                break;
        }
    }

    public void CancelTurnPrep()
    {
        canTurn = false;
        lastTurnIsLeft = false;
        lastTurnIsRight = false;
        lastMidPos = Vector3.zero;
    }

    public void CanTurnIntoBuildingFromLane(BuildingInteraction building)
    {
        currentBuildingRef = building;
    }

    public void BuildingEntered(BuildingInteraction building)
    {
        currentBuildingRef = building;
        doMove = false;
        gameCamera.WatchObject(currentBuildingRef.cameraHoldPosition.position, currentBuildingRef.cameraNewLookAt);
    }

    public void CanNoLongerTurnIntoBuildingFromLane(BuildingInteraction building)
    {
        if (moveIntoBuilding == false
            && moveOutOfBuilding == false)
        {
            currentBuildingRef = null;
        }
    }

    public void TurnIntoBuilding()
    {
        moveIntoBuilding = true;
        doMove = false;
        gameCamera.WatchObject(currentBuildingRef.cameraHoldPosition.position, currentBuildingRef.cameraNewLookAt);
    }

    void MoveToBuildingPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentBuildingRef.playerInteractionLocation.position, Time.deltaTime);
        if (Vector3.Distance(transform.position, currentBuildingRef.playerInteractionLocation.position) < 1f)
        {
            moveIntoBuilding = false;
        }
    }

    public void ExitBuilding()
    {
        moveOutOfBuilding = true;
        gameCamera.WatchPlayer();
        if(currentBuildingRef.turnPlayerAround)
        {
            transform.eulerAngles += new Vector3(0f, 180f, 0f);
            int moveDir = (int)currentMoveDirection + 2;
            if(moveDir > 4)
            {
                moveDir -= 4;
            }
            currentMoveDirection = (MoveDirection)moveDir;
        }
    }

    void MoveAwayFromBuilding()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentBuildingRef.exitLocation.position, Time.deltaTime);
        if (Vector3.Distance(transform.position, currentBuildingRef.exitLocation.position) < 1f)
        {
            moveOutOfBuilding = false;
            BuildingExited();
        }
    }

    void BuildingExited()
    {
        doMove = true;
        currentBuildingRef = null;
    }

    public void MovePlayerToLocation(Vector3 waitPoint)
    {
        movingToPoint = true;
        doMove = false;
        goalLocation = waitPoint;
    }

    void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, goalLocation, Time.deltaTime);
        if (Vector3.Distance(transform.position, goalLocation) < 1f)
        {
            movingToPoint = false;
        }
    }

    public void Bump_ForcePlayerToTurn()
    {
        //Animation?

        //Turn player - [TODO] Default right for now. Determine if this is more elaborate later?
        if (lastTurnIsRight)
        {
            nextLeftMidPos = lastMidPos;
            TurnRunner(-1f);
        }
        else if(lastTurnIsLeft)
        {
            nextRightMidPos = lastMidPos;
            TurnRunner(1f);
        }
        else
        {
            TurnRunner(1f);
        }
    }
}
