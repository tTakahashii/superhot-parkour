using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private MovementManager playerManager;
    [SerializeField] private Animator playerAnimator;

    private enum AnimatorState { Idle, RunningForwardSlow, RunningForwardFast, RunningBackwardSlow, RunningBackwardFast,
        RunningSidewaysSlow, RunningSidewaysFast, JumpingIdle, JumpingRunningForward, JumpingRunningBackward}
    private AnimatorState animState = AnimatorState.Idle, previousState;

    // Update is called once per frame
    void Update()
    {
        AnimatorStateController();
    }

    private void AnimatorStateController()
    {
        float velocityZ = playerManager.GetVelocity().z;
        float absVelocityX = Mathf.Abs(playerManager.GetVelocity().x);

        //Debug.Log("VELOCITY: " + playerManager.GetVelocity());

        if (velocityZ >= playerManager.forwardSpeed + 0.05f)
        {
            animState = AnimatorState.RunningForwardFast;
        }
        else if (velocityZ >= 0.05f)
        {
            animState = AnimatorState.RunningForwardSlow;
        }
        else if (velocityZ <= -playerManager.forwardSpeed - 0.05f)
        {
            animState = AnimatorState.RunningBackwardFast;
        }
        else if (velocityZ <= -0.05f)
        {
            animState = AnimatorState.RunningBackwardSlow;
        }
        else if (absVelocityX >= playerManager.horizontalSpeed + 0.05f)
        {
            animState = AnimatorState.RunningSidewaysFast;
        }
        else if (absVelocityX >= 0.05f)
        {
            animState = AnimatorState.RunningSidewaysSlow;
        }
        else
        {
            animState = AnimatorState.Idle;
        }
        
        if (previousState != animState)
        {
            previousState = animState;
            playerAnimator.SetTrigger(Enum.GetName(typeof(AnimatorState), animState));
            Debug.Log("PREVIOUS STATE: " + previousState);
            Debug.Log("STATE: " + animState);
        }
        //Debug.Log();
    }
}