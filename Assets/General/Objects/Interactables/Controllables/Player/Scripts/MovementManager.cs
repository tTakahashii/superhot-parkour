using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementManager : MonoBehaviour
{  
    public enum MovementState { None, ControllerBased, PhysicsBased, NonPhysicsBased };
    private MovementState previousState = MovementState.None;

    [Header("Movement Settings")]
    public MovementState playerState;

    [Header("Boxcast & Ground Check")]
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 boxcastOffset, boxcastSize;
    [SerializeField] private LayerMask layerMask;
    private bool isGrounded;
    private RaycastHit hit;

    //[Header("CharacterController Settings")]
    public CharacterController charController;
    [SerializeField] private float forwardSpeedCH, horizontalSpeedCH, jumpForceCH, runMultiplierCH, gravityMultiplier;
    [SerializeField] private bool smoothCH, groundDetectionCH = true;

    //[Header("PhysicsController Settings")]
    public Rigidbody rb;
    [SerializeField] private float forwardSpeedPH, horizontalSpeedPH, jumpForcePH, runMultiplierPH;
    [SerializeField] private bool smoothPH, groundDetectionPH = true;

    //[Header("NonPhysicsController Settings")]
    [SerializeField] private Transform cam;
    [SerializeField] private float forwardSpeedNonPH, horizontalSpeedNonPH, verticalSpeedNonPH, runMultiplierNonPH;

    private float forward = 0f, horizontal = 0f, vertical = 0f, run = 0f; 
    private Vector3 gravity = Vector3.zero;
    private bool shouldJump = false;

    public float forwardSpeed
    {
        get
        {
            switch (playerState)
            {
                case MovementState.ControllerBased:
                    return forwardSpeedCH;

                case MovementState.PhysicsBased:
                    return forwardSpeedPH;

                default:
                    return 0f;
            }
        }
    }

    public float horizontalSpeed
    {
        get
        {
            switch (playerState)
            {
                case MovementState.ControllerBased:
                    return horizontalSpeedCH;

                case MovementState.PhysicsBased:
                    return horizontalSpeedPH;

                default:
                    return 0f;
            }
        }
    }

    public float runningSpeed
    {
        get
        {
            switch (playerState)
            {
                case MovementState.ControllerBased:
                    return runMultiplierCH;

                case MovementState.PhysicsBased:
                    return runMultiplierPH;

                default:
                    return 0f;
            }
        }
    }

    void Update()
    {
        if (!GameState.IsPaused())
        {
            OnStateChange(charController, rb);
            MovementStateManager(playerState);
        }
    }

    private void FixedUpdate()
    {
        if (!GameState.IsPaused())
        {
            isGrounded = GroundDetection();
        } 
    }

    private void OnDrawGizmosSelected()
    {
        float rayLength = isGrounded ? hit.distance : maxDistance;
        Gizmos.DrawRay(transform.position, -transform.up * rayLength);
        Gizmos.DrawWireCube(transform.position + -transform.up * rayLength, boxcastSize * 2f);

        //Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawWireCube(boxcastOffset, boxcastSize * 2f);
    }

    public Vector3 GetVelocity(bool localVelocity = true)
    {
        switch (playerState)
        {
            case MovementState.ControllerBased:
                return localVelocity ? transform.InverseTransformDirection(charController.velocity) : charController.velocity;
            case MovementState.PhysicsBased:
                return localVelocity ? transform.InverseTransformDirection(rb.velocity) : rb.velocity;
            default:
                return Vector3.zero;
        }
    }

    private void MovementStateManager(MovementState state)
    {
        switch (state)
        {
            case MovementState.ControllerBased:
                CharController(charController, forwardSpeedCH, horizontalSpeedCH, jumpForceCH, runMultiplierCH, gravityMultiplier, isGrounded, smoothCH);
                break;
            case MovementState.PhysicsBased:
                PhysicsController(rb, forwardSpeedPH, horizontalSpeedPH, jumpForcePH, runMultiplierPH, smoothPH, shouldJump);
                break;
            case MovementState.NonPhysicsBased:
                NonPhysicsController(cam, forwardSpeedNonPH, horizontalSpeedNonPH, verticalSpeedNonPH, runMultiplierNonPH);
                break;
        }
    }

    private bool GroundDetection()
    {
        bool isGrounded;

        switch (playerState)
        {
            case MovementState.ControllerBased:
                isGrounded = groundDetectionCH ? Physics.BoxCast(transform.position, boxcastSize, -transform.up, out hit, transform.rotation, maxDistance, layerMask) : false;
                break;
            case MovementState.PhysicsBased:
                isGrounded = groundDetectionPH ? Physics.BoxCast(transform.position, boxcastSize, -transform.up, out hit, transform.rotation, maxDistance, layerMask) : false;
                break;
            default:
                isGrounded = false;
                break;
        }

        return isGrounded;
    }

    public void ChangeState(MovementState newState)
    {
        playerState = newState;
    }


    private void OnStateChange(CharacterController charController, Rigidbody rb)
    {
        if (previousState != playerState)
        {
            previousState = playerState;

            switch (playerState)
            {
                case MovementState.ControllerBased:
                    charController.enabled = true;

                    rb.isKinematic = true;
                    rb.detectCollisions = false;

                    break;

                case MovementState.PhysicsBased:                    
                    rb.isKinematic = false;
                    rb.detectCollisions = true;

                    charController.enabled = false;

                    break;

                case MovementState.NonPhysicsBased:
                    rb.isKinematic = true;
                    rb.detectCollisions = false;

                    charController.enabled = false;

                    break;
            }
        }
    }

    private void CharController(CharacterController charController, float forwardSpeed, float horizontalSpeed, float jumpSpeed, float runMultiplier, float gravityMultiplier, bool isGrounded, bool smoothMovement)
    {
        horizontal = horizontalSpeed * (smoothMovement ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
        forward = forwardSpeed * (smoothMovement ? Input.GetAxis("Forward") : Input.GetAxisRaw("Forward"));

        if (isGrounded)
        {
            if (Input.GetAxisRaw("Vertical") > 0f)
            {
                vertical = jumpForceCH;
            }
            else
            {
                vertical = 0f;
            }
        }

        //vertical = Input.GetAxisRaw("Vertical") > 0f ? jumpForceCH : vertical;
        //vertical = isGrounded ? 0f : vertical;
        run = Mathf.Clamp(runMultiplier * (smoothMovement ? Input.GetAxis("Run") : Input.GetAxisRaw("Run")), 1f, runMultiplier);

        Vector3 forwardMovement = transform.forward * forward * run;
        Vector3 horizontalMovement = transform.right * horizontal * run;
        Vector3 verticalMovement = transform.up * vertical;

        gravity += ((Physics.gravity * gravityMultiplier) * Time.deltaTime);
        gravity = isGrounded ? Vector3.zero : gravity;
        Debug.Log(gravity);

        charController.Move(((forwardMovement + horizontalMovement) + (verticalMovement + gravity)) * Time.deltaTime);
    }

    private void PhysicsController(Rigidbody rb, float forwardSpeed, float horizontalSpeed, float jumpSpeed, float runMultiplier, bool smoothMovement, bool shouldJump)
    {
        horizontal = horizontalSpeed * (smoothMovement ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
        forward = forwardSpeed * (smoothMovement ? Input.GetAxis("Forward") : Input.GetAxisRaw("Forward"));
        run = Mathf.Clamp(runMultiplier * (smoothMovement ? Input.GetAxis("Run") : Input.GetAxisRaw("Run")), 1f, runMultiplier);

        Vector3 relativeVelocity = transform.InverseTransformDirection(rb.velocity);
        relativeVelocity.x = horizontal * run;
        shouldJump = (isGrounded && Input.GetAxisRaw("Vertical") > 0f);
        relativeVelocity.y = shouldJump ? jumpSpeed : relativeVelocity.y;
        relativeVelocity.z = forward * run; ;
        rb.velocity = transform.TransformDirection(relativeVelocity);
    }

    private void NonPhysicsController(Transform camera, float forwardSpeed, float horizontalSpeed, float verticalSpeed, float runMultiplier)
    {
        Vector3 position = transform.localPosition;
        run = Mathf.Clamp(Input.GetAxisRaw("Run") * runMultiplier, 1f, runMultiplier);

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

}