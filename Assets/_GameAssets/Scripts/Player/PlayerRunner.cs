using UnityEngine;
using System.Collections;

public class PlayerRunner : MonoBehaviour {

    public float moveSpeed = 10f;
    public float tiltSpeed = 1f;

    public bool doMove = true;

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
        }
	}

    void AutoRun()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }

    void TurnRunner(float inputDir)
    {
        transform.eulerAngles += new Vector3(0f, inputDir * 90f, 0f);
    }

    void TiltRunner(float inputDir)
    {
        transform.position += inputDir * Time.deltaTime * tiltSpeed * transform.right;
    }
}
