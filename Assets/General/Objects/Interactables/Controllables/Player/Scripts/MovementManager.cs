using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private enum MovementState { controllerBased, physicsBased, nonPhysicsBased };
    [SerializeField] private MovementState playerState;
    private MovementState previousState;
    [SerializeField] private MonoBehaviour controllerScript, physicsScript, nonPhysicsScript;

    // Update is called once per frame
    void Update()
    {
        OnStateChange();
    }

    public void ChangeState()
    {

    }

    private void OnStateChange()
    {
        if (previousState != playerState)
        {
            previousState = playerState; 

            if (playerState == MovementState.controllerBased)
            {
                controllerScript.enabled = true;

                physicsScript.enabled = false;
                nonPhysicsScript.enabled = false;
            }

            if (playerState == MovementState.physicsBased)
            {
                physicsScript.enabled = true;

                controllerScript.enabled = false;
                nonPhysicsScript.enabled = false;
            }

            if (playerState == MovementState.nonPhysicsBased)
            {
                nonPhysicsScript.enabled = true;

                controllerScript.enabled = false;
                physicsScript.enabled = false;
            }
        }
    }
}
