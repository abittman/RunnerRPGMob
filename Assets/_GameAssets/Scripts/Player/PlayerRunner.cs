using UnityEngine;
using System.Collections;

public class PlayerRunner : MonoBehaviour {

    public Rigidbody playerRB;

    public float moveSpeed = 10f;
    public float tiltSpeed = 1f;

    public float jumpForce = 100f;

    public bool doMove = true;

    public bool canTurn = false;

    public GameObject meshObject;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(doMove)
        {
            AutoRun();
        }
        /*
        if (Input.GetButtonDown("Horizontal"))
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            if (horizontalInput != 0f)
            {
                TurnRunner(horizontalInput);
            }
        }

        if(Input.GetButton("Vertical"))
        {
            float gyroInput = Input.GetAxis("Vertical");
            TiltRunner(gyroInput);
        }*/
	}

    public void StopRunner()
    {
        doMove = false;
        playerRB.isKinematic = true;
        playerRB.useGravity = false;
    }

    void AutoRun()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }

    public void DoLeft()
    {
        if(canTurn)
        {
            TurnRunner(-1f);
        }
        else
        {
            LaneChangeRunner(-1);
        }
    }

    public void DoRight()
    {
        if (canTurn)
        {
            TurnRunner(1f);
        }
        else
        {
            LaneChangeRunner(1);
        }
    }

    void TurnRunner(float inputDir)
    {
        transform.eulerAngles += new Vector3(0f, inputDir * 90f, 0f);
        canTurn = false;
    }

    void LaneChangeRunner(int direction)
    {
        transform.position += direction * 1.5f * transform.right;
    }
    
    /*
    void TiltRunner(float inputDir)
    {
        transform.position += inputDir * Time.deltaTime * tiltSpeed * transform.right;
    }
    */

    public void DoJump()
    {
        JumpRunner();
    }

    void JumpRunner()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void DoSlide()
    {
        SlideRunner();
    }

    void SlideRunner()
    {
        //Setup an animation to handle this
    }
}
