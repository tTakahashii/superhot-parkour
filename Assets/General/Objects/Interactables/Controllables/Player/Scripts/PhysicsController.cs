using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed, horizontalSpeed, jumpSpeed, runMultiplier;
    [SerializeField] private bool smoothMovement = true;
    private Rigidbody rb;

    private void Start()
    {
        TryGetComponent(out rb);
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.IsPaused())
        {
            float horizontal = horizontalSpeed * (smoothMovement ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
            float forward = forwardSpeed * (smoothMovement ? Input.GetAxis("Forward") : Input.GetAxisRaw("Forward"));
            float run = Mathf.Clamp(runMultiplier * (smoothMovement ? Input.GetAxis("Run") : Input.GetAxisRaw("Run")), 1f, runMultiplier);

            Vector3 relativeVelocity = transform.InverseTransformDirection(rb.velocity);
            relativeVelocity.x = horizontal * run;
            relativeVelocity.y = Input.GetAxis("Vertical") != 0f ? jumpSpeed * (smoothMovement ? Input.GetAxis("Vertical"): Input.GetAxisRaw("Vertical")) : relativeVelocity.y;
            relativeVelocity.z =forward * run; ;
            rb.velocity = transform.TransformDirection(relativeVelocity);
        }
    }
}
