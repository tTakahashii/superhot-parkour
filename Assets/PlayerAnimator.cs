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
        float velocityX = playerManager.GetVelocity().x;

        //Debug.Log("VELOCITY: " + playerManager.GetVelocity());

        // FORWARD & BACKWARD RUNNING ANIMATION

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

        // HORIZONTAL RUNNING ANIMATION

        else if (velocityX >= playerManager.horizontalSpeed - 0.05f)
        {
            animState = AnimatorState.RunningSidewaysFast;
        }
        else if (velocityX >= 0.05f)
        {
            animState = AnimatorState.RunningSidewaysSlow;
        }

        else if (velocityX <= -playerManager.horizontalSpeed + 0.05f)
        {
            animState = AnimatorState.RunningSidewaysFast;
        }
        else if (velocityX >= 0.05f)
        {
            animState = AnimatorState.RunningSidewaysSlow;
        }

        // IDLE
        else
        {
            animState = AnimatorState.Idle;
        }
        
        if (previousState != animState)
        {
            Debug.Log("PREVIOUS STATE: " + previousState);
            previousState = animState;
            playerAnimator.SetTrigger(Enum.GetName(typeof(AnimatorState), previousState));
            Debug.Log("STATE: " + animState);
            
        }
        //Debug.Log();
    }
}