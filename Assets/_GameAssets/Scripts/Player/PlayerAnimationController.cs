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
    }

    public void LeftLaneChangeAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);

        playerAnimator.SetBool("DoLeftLaneChange", true);
    }

    public void RightLaneChangeAnimation()
    {

        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);

        playerAnimator.SetBool("DoRightLaneChange", true);
    }

    public void JumpAnimation()
    {

        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);

        playerAnimator.SetBool("DoJump", true);
    }

    public void SlideAnimation()
    {

        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);

        playerAnimator.SetBool("DoSlide", true);
    }

    public void RightAttackAnimation()
    {

    }

    public void LeftAttackAnimation()
    {

    }

    public void JumpAttackAnimation()
    {

    }

    public void SlideAttackAnimation()
    {

    }
}
