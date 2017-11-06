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
    public PlayerAnimationController pAniController;
    public PlayerFXController fxController;

    public Rigidbody playerRB;

    [Header("Current Status")]
    public bool doMove = true;
    public bool inHitRecovery = false;
    public bool canTurn = false;
    public bool canMoveInAir = false;       //Upgrade ability
    public bool canWallRun = false;         //Upgrade ability
    public bool canJump = false;            //In case it gets cancelled somehow?
    public MoveDirection currentMoveDirection;
    public RunningLane currentLane;

    bool isGrounded = false;
    RaycastHit hit;

    [Header("Movement Stats")]
    float currentMoveSpeed;
    public float moveSpeed = 10f;
    [Space]
    public float hitMoveSpeed = 3f;
    public float hitRecoveryTime = 2f;
    float hitRecoveryTimer = 0f;
    public AnimationCurve hitRecoveryAccelCurve;

    [Header("Jumping Stats")]
    public Transform playerGroundingRoot;
    public LayerMask groundingMask;
    public float jumpForce = 100f;

    [Header("Sliding Stats")]
    public float slideAirDownForce = 100f;
    //Sliding
    bool doSliding = false;
    bool pendingSlideAnimation = false;
    public float slideTimer = 1f;
    float currentSlideTimer = 0f;

    [Header("Lane Changing Stats")]
    //Lane changing variables
    Vector3 currentMidLanePos = Vector3.zero;
    public float laneChangeTransitionTime = 0.2f;
    float laneChangeTimer = 0f;
    public float laneChangeDistance = 2f;
    public AnimationCurve laneChangeAniCurve;
    bool doLaneChange = false;
    float startX;
    float goalX;
    float startZ;
    float goalZ;

    [Header("Bumping Stats")]
    public float bumpBackTransitionTime = 0.2f;
    float bumpBackTimer = 0f;
    bool doBumpBack = false;

    [Header("Turning stats")]
    public Vector3 nextLeftMidPos;
    public Vector3 nextRightMidPos;
    Vector3 lastMidPos;
    bool lastTurnIsLeft = false;
    bool lastTurnIsRight = false;
    public bool doCompleteStopOnTurn = true;
    public float timeToReturnToSpeed = 1f;
    public AnimationCurve turnReturnToSpeedAniCurve;

    //Tracking
    BuiltPathPiece currentBPP;

    [Header("Wall running stats")]
    bool isWallRunning = false;
    public float wallDetectionLength = 3f;
    public float wallRunningTime = 1f;
    public float wallRunningHeight = 5f;
    float wallRunningTimer = 0f;

    #region Runner_Setup
    public void SetupRunner(BuiltPathPiece startingBPP, Transform startingLocation)
    {
        currentLane = RunningLane.Mid;
        currentMoveDirection = MoveDirection.North;

        transform.position = startingLocation.position;
        transform.rotation = startingLocation.rotation;

        currentBPP = startingBPP;
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {
	    if(doMove)
        {
            AutoRun();

            if(isWallRunning)
            {
                ContinueWallRunning();
            }

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
                if(pendingSlideAnimation)
                {
                    if(isGrounded)
                    {
                        pAniController.SlideAnimation();
                        pendingSlideAnimation = false;
                    }
                }

                if(currentSlideTimer >= slideTimer)
                {
                    currentSlideTimer = 0f;
                    doSliding = false;
                    pAniController.StraightRunAnimation();
                }
            }
        }
	}

    #region RunningMovement
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
        if(inHitRecovery)
        {
            hitRecoveryTimer += Time.deltaTime;
            float curveEval = hitRecoveryAccelCurve.Evaluate(hitRecoveryTimer / hitRecoveryTime);
            currentMoveSpeed = Mathf.Lerp(hitMoveSpeed, moveSpeed, curveEval);

            if(hitRecoveryTimer >= hitRecoveryTime)
            {
                currentMoveSpeed = moveSpeed;
                inHitRecovery = false;
            }
        }
        else
        {
            currentMoveSpeed = moveSpeed;
        }

        transform.position += currentMoveSpeed * Time.deltaTime * transform.forward;

        //Check grounding
        if (Physics.Raycast(playerGroundingRoot.position, Vector3.down, out hit, .25f, groundingMask))
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
    }

    public void PlayerHit()
    {
        Debug.Log("Player hit");
        inHitRecovery = true;
        hitRecoveryTimer = 0f;
        fxController.PlayPlayerHitFX();
    }
    #endregion

    #region Left_Right_Behaviour
    public void DoLeft()
    {
        CheckWallRunning(-1);

        if (canTurn)
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
        CheckWallRunning(1);

        if (canTurn)
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

    //Code to turn the runner on a piece to face a new direction
    //[TODO] Really long. Clean it up in the future
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

    //Function changes the lane of the runner
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
        }
    }
    #endregion

    #region Jumping_Behaviour
    public void DoJump()
    {
        if (isGrounded)
        {
            JumpRunner();
        }
    }

    void JumpRunner()
    {
        playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        pAniController.JumpAnimation();
        
        doSliding = false;
        currentSlideTimer = 0f;
    }
    #endregion

    #region Sliding_Behaviour
    public void DoSlide()
    {
        SlideRunner();
    }

    void SlideRunner()
    {
        //Setup an animation to handle this
        doSliding = true;
        if(!pendingSlideAnimation)
        if(isGrounded == false)
        {
            playerRB.AddForce(-transform.up * slideAirDownForce, ForceMode.Impulse);
            pendingSlideAnimation = true;
        }
        else
        {
            pAniController.SlideAnimation();
            pendingSlideAnimation = false;
        }
    }
    #endregion

    //I dont think bumping works yet / at the moment?
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

    #region Turning_Prep
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
    #endregion

    #region Wall_Running
    void CheckWallRunning(int direction)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.right * direction, out hit, wallDetectionLength, groundingMask))
        {
            if(hit.collider.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Can wall run!~");
                StartWallRunning(direction);
            }
        }
    }

    void StartWallRunning(float direction)
    {
        isWallRunning = true;
        //Left
        if (direction == -1)
        {
            //playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerRB.useGravity = false;
            wallRunningTimer = 0f;
            pAniController.DoWallRunAnimation(-1);
        }
        else if(direction == 1)
        {
            playerRB.useGravity = false;
            wallRunningTimer = 0f;
            pAniController.DoWallRunAnimation(1);
        }
    }

    void ContinueWallRunning()
    {
        wallRunningTimer += Time.deltaTime;

        if(wallRunningTimer >= wallRunningTime)
        {
            ExitWallRunning();
        }
    }

    void ExitWallRunning()
    {
        playerRB.useGravity = true;
        pAniController.StraightRunAnimation();
    }
    #endregion
}
