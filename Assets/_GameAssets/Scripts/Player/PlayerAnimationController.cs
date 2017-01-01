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
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);
    }

    public void LeftLaneChangeAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoLeftLaneChange", true);
    }

    public void RightLaneChangeAnimation()
    {

        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoRightLaneChange", true);
    }

    public void JumpAnimation()
    {

        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoJump", true);
    }

    public void SlideAnimation()
    {

        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoSlide", true);
    }

    public void FrontAttackAnimation()
    {
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoRightAttack", false);

        playerAnimator.SetBool("DoFrontAttack", true);
    }

    public void RightAttackAnimation()
    {
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoRightAttack", true);
    }

    public void LeftAttackAnimation()
    {
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoLeftAttack", true);

    }

    public void JumpAttackAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoSlideAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoJumpAttack", true);
    }

    public void SlideAttackAnimation()
    {
        playerAnimator.SetBool("DoJump", false);
        playerAnimator.SetBool("DoLeftLaneChange", false);
        playerAnimator.SetBool("DoRightLaneChange", false);
        playerAnimator.SetBool("DoSlide", false);
        playerAnimator.SetBool("DoRightAttack", false);
        playerAnimator.SetBool("DoLeftAttack", false);
        playerAnimator.SetBool("DoJumpAttack", false);
        playerAnimator.SetBool("DoFrontAttack", false);

        playerAnimator.SetBool("DoSlideAttack", true);
    }
}
