using UnityEngine;
using System.Collections.Generic;

public class PlayerSwipeInput : MonoBehaviour {

    public PlayerRunner pRunner;

    public bool canControl = false;

    [Header("Swipe Control")]
    bool swiping = false;
    bool eventSent = false;
    Vector2 lastTouchPosition;
    List<Touch> lastTouches = new List<Touch>();
    Vector2 lastMousePos;
    public float swipeMinAmount;
    public float swipeMagnitude = 100f;
    public float swipeMagForScreenDPI = 1f;
    public float delayForNextSwipe = 0.25f;
    float delayTimer = 0f;

    // Use this for initialization
    void Start () {

        swipeMinAmount = Screen.width / 15f;
        //float totalWidth = Screen.width * (1 / Screen.dpi);
        
        //canControl = true;
        delayTimer = delayForNextSwipe;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(canControl)
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount > 0)
                {
                    if (!eventSent)
                    {
                        if (!swiping)
                        {
                            swiping = true;
                            lastTouchPosition = Input.GetTouch(0).position;
                            
                        }
                        else
                        {
                            //Touch lastTouch = lastTouches.Find(x => x.fingerId == Input.GetTouch(i).fingerId);

                            Vector2 swipeDirection = Input.GetTouch(0).position - lastTouchPosition;

                            if (Vector3.Distance(Input.GetTouch(0).position, lastTouchPosition) >= swipeMinAmount)
                            {

                                //If more horizontal than vertical
                                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                                {
                                    //Swiped left && swipe amount acceptable
                                    if (swipeDirection.x < -swipeMinAmount)
                                    {
                                        //swipeDir.text = "Left Swipe";
                                        DoLeftSwipeAction();
                                    }
                                    else if (swipeDirection.x > swipeMinAmount)
                                    {
                                        //swipeDir.text = "Right Swipe";
                                        DoRightSwipeAction();
                                    }
                                }
                                else
                                {
                                    if (swipeDirection.y > swipeMinAmount)
                                    {
                                        //swipeDir.text = "Up Swipe";
                                        DoUpSwipeAction();
                                    }
                                    else if (swipeDirection.y < -swipeMinAmount)
                                    {
                                        //swipeDir.text = "Down Swipe";

                                        DoDownSwipeAction();
                                    }
                                }

                                //Remove last touch when complete
                                //lastTouches.Remove(lastTouches.Find(x => x.fingerId == Input.GetTouch(i).fingerId));
                            }
                        }
                    }
                    //}

                    if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                        Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        //swipeDir.text = "Ended Touch";
                        swiping = false;
                        eventSent = false;
                        //tapped = false;
                        delayTimer = delayForNextSwipe;
                        //lastTouches.Remove(lastTouches.Find(x => x.fingerId == Input.GetTouch(i).fingerId));
                    }
                }
                else
                {
                    swiping = false;
                    eventSent = false;
                    //tapped = false;
                    delayTimer = delayForNextSwipe;
                    lastTouches.Clear();
                }
            }
            else if (Application.isEditor)
            {
                if(Input.GetKeyDown(KeyCode.A))
                {
                    DoLeftSwipeAction();
                }
                else if(Input.GetKeyDown(KeyCode.D))
                {
                    DoRightSwipeAction();
                }
                else if(Input.GetKeyDown(KeyCode.W))
                {
                    DoUpSwipeAction();
                }
                else if(Input.GetKeyDown(KeyCode.S))
                {
                    DoDownSwipeAction();
                }

                if (Input.GetMouseButton(0))
                {
                    Vector2 currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                    Vector2 mouseDelta = currentMousePos - lastMousePos;
                    float mouseDistance = Vector2.Distance(currentMousePos, lastMousePos);

                    if (mouseDistance >= swipeMinAmount)
                    {
                        if (!eventSent)
                        {
                            if (!swiping)
                            {
                                swiping = true;
                                lastMousePos = currentMousePos;
                            }
                            else
                            {
                                Vector2 swipeDirection = currentMousePos - lastMousePos;
                                
                                //More horizontal than vertical
                                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                                {
                                    //Swiped left && swipe amount acceptable
                                    if (swipeDirection.x < -swipeMinAmount)
                                    {
                                        DoLeftSwipeAction();
                                    }
                                    else if (swipeDirection.x > swipeMinAmount)
                                    {
                                        DoRightSwipeAction();
                                    }
                                }
                                else
                                {
                                    if (swipeDirection.y > swipeMinAmount)
                                    {
                                        //DoTapAction();
                                        DoUpSwipeAction();
                                    }
                                    else if (swipeDirection.y < -swipeMinAmount)
                                    {
                                        DoDownSwipeAction();
                                    }
                                }
                            }
                        }
                    }

                    lastMousePos = currentMousePos;
                }
                else
                {
                    lastMousePos = Vector2.zero;
                    swiping = false;
                    eventSent = false;
                    //tapped = false;
                    delayTimer = delayForNextSwipe;
                }

            }
        }
	}

    public void ActivateControl()
    {
        canControl = true;
    }

    public void DeactivateControl()
    {
        canControl = false;
    }

    void DoUpSwipeAction()
    {
        pRunner.DoJump();
        eventSent = true;
    }

    void DoDownSwipeAction()
    {
        pRunner.DoSlide();
        eventSent = true;
    }

    void DoLeftSwipeAction()
    {
        pRunner.DoLeft();
        eventSent = true;
    }

    void DoRightSwipeAction()
    {
        pRunner.DoRight();
        eventSent = true;
    }
}
