using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{  
    private enum MovementState { controllerBased, physicsBased, nonPhysicsBased };
    private MovementState previousState;

    [Header("Movement States")]
    [SerializeField] private MovementState playerState;

    [Header("CharacterController Settings")]
    [SerializeField] private CharacterController charController;
    [SerializeField] private float forwardSpeedCH, horizontalSpeedCH, jumpForceCH, runMultiplierCH, gravityMultiplier;
    [SerializeField] private bool smoothCH;

    [Header("PhysicsController Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float forwardSpeedPH, horizontalSpeedPH, jumpForcePH, runMultiplierPH;
    [SerializeField] private bool smoothPH;

    [Header("NonPhysicsController Settings")]
    [SerializeField] private Transform cam;
    [SerializeField] private float forwardSpeedNonPH, horizontalSpeedNonPH, verticalSpeedNonPH, runMultiplierNonPH;

    void Update()
    {
        OnStateChange(charController, rb);
        MovementStateManager(playerState);
    }

    private void MovementStateManager(MovementState state)
    {
        switch (state)
        {
            case MovementState.controllerBased:
                CharController(charController, forwardSpeedCH, horizontalSpeedCH, jumpForceCH, runMultiplierCH, smoothCH);
                break;
            case MovementState.physicsBased:
                PhysicsController(rb, forwardSpeedPH, horizontalSpeedPH, jumpForcePH, runMultiplierPH, smoothPH);
                break;
            case MovementState.nonPhysicsBased:
                NonPhysicsController(cam, forwardSpeedNonPH, horizontalSpeedNonPH, verticalSpeedNonPH, runMultiplierNonPH);
                break;
        }
    }

    private void CharController(CharacterController charController, float forwardSpeed, float horizontalSpeed, float jumpSpeed, float runMultiplier, bool smoothMovement)
    {
        float horizontal = horizontalSpeed * (smoothMovement ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
        float forward = forwardSpeed * (smoothMovement ? Input.GetAxis("Forward") : Input.GetAxisRaw("Forward"));
        float run = Mathf.Clamp(runMultiplier * (smoothMovement ? Input.GetAxis("Run") : Input.GetAxisRaw("Run")), 1f, runMultiplier);

        Vector3 movement = transform.forward * forward * run;

        charController.Move(movement);
    }

    private void PhysicsController(Rigidbody rb, float forwardSpeed, float horizontalSpeed, float jumpSpeed, float runMultiplier, bool smoothMovement)
    {
        float horizontal = horizontalSpeed * (smoothMovement ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
        float forward = forwardSpeed * (smoothMovement ? Input.GetAxis("Forward") : Input.GetAxisRaw("Forward"));
        float run = Mathf.Clamp(runMultiplier * (smoothMovement ? Input.GetAxis("Run") : Input.GetAxisRaw("Run")), 1f, runMultiplier);

        Vector3 relativeVelocity = transform.InverseTransformDirection(rb.velocity);
        relativeVelocity.x = horizontal * run;
        relativeVelocity.y = Input.GetAxis("Vertical") != 0f ? jumpSpeed * (smoothMovement ? Input.GetAxis("Vertical") : Input.GetAxisRaw("Vertical")) : relativeVelocity.y;
        relativeVelocity.z = forward * run; ;
        rb.velocity = transform.TransformDirection(relativeVelocity);
    }

    private void NonPhysicsController(Transform camera, float forwardSpeed, float horizontalSpeed, float verticalSpeed, float runMultiplier)
    {
        Vector3 position = transform.localPosition;
        float run = Mathf.Clamp(Input.GetAxisRaw("Run") * runMultiplier, 1f, runMultiplier);

        position += camera.transform.forward
            * ((Input.GetAxisRaw("Forward") * forwardSpeed)
            * run
            * Time.unscaledDeltaTime);

        position += camera.transform.right
            * ((Input.GetAxisRaw("Horizontal") * horizontalSpeed)
            * run
            * Time.unscaledDeltaTime);

        position.y += (Input.GetAxisRaw("Vertical") * verticalSpeed)
            * run
            * Time.unscaledDeltaTime;

        transform.localPosition = position;

        // ** //
        // SMOOTH MOVEMENT BROKEN AT THE MOMENT, GONNA FIX IT LATER
        // ** //

        //if (smoothMovement)
        //{
        //    float run = Mathf.Clamp((Input.GetAxis("Run") / Time.timeScale) * runMultiplier, 1f, runMultiplier);

        //    position += transform.forward
        //        * ((Input.GetAxis("Forward") / Time.timeScale) * forwardSpeed)
        //        * run
        //        * Mathf.Clamp(Time.unscaledDeltaTime, 0f, 1f / 3f);

        //    position += transform.right
        //        * ((Input.GetAxis("Horizontal") / Time.timeScale) * horizontalSpeed)
        //        * run
        //        * Mathf.Clamp(Time.unscaledDeltaTime, 0f, 1f / 3f);

        //    position += new Vector3(0f,
        //        (Input.GetAxis("Vertical") / Time.timeScale) * verticalSpeed
        //        * run
        //        * Mathf.Clamp(Time.unscaledDeltaTime, 0f, 1f / 3f),
        //    0f);

        //    transform.localPosition = position;
        //}
    }

    public void ChangeState()
    {

    }

    private void OnStateChange(CharacterController charController, Rigidbody rb)
    {
        if (previousState != playerState)
        {
            previousState = playerState;

            switch (playerState)
            {
                case MovementState.controllerBased:
                    charController.enabled = true;

                    rb.isKinematic = true;
                    rb.detectCollisions = false;

                    break;

                case MovementState.physicsBased:                    
                    rb.isKinematic = false;
                    rb.detectCollisions = true;

                    charController.enabled = false;

                    break;

                case MovementState.nonPhysicsBased:
                    rb.isKinematic = true;
                    rb.detectCollisions = false;

                    charController.enabled = false;

                    break;
            }
        }
    }
}
