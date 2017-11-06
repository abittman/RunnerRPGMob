using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

    public Animator playerAnimator;
	
    public void StraightRunAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoWallRunLeft", false);
        playerAnimator.SetBool("DoWallRunRight", false);
    }

    public void LeftLaneChangeAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoWallRunLeft", false);
        playerAnimator.SetBool("DoWallRunRight", false);

        playerAnimator.SetBool("DoLeftLaneChange", true);
    }

    public void RightLaneChangeAnimation()
    {

        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoWallRunLeft", false);
        playerAnimator.SetBool("DoWallRunRight", false);

        playerAnimator.SetBool("DoRightLaneChange", true);
    }

    public void JumpAnimation()
    {

        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoWallRunLeft", false);
        playerAnimator.SetBool("DoWallRunRight", false);

        playerAnimator.SetBool("DoJump", true);
    }

    public void SlideAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoWallRunLeft", false);
        playerAnimator.SetBool("DoWallRunRight", false);

        playerAnimator.SetBool("DoSlide", true);
    }
    
    public void DoWallRunAnimation(int direction)
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);

        if(direction == -1)
        {
            playerAnimator.SetBool("DoWallRunLeft", true);
            playerAnimator.SetBool("DoWallRunRight", false);
        }
        else if (direction == 1)
        {
            playerAnimator.SetBool("DoWallRunLeft", false);
            playerAnimator.SetBool("DoWallRunRight", true);
        }
    }
}
