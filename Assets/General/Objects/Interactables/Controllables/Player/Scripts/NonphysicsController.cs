using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonphysicsController : MonoBehaviour
{
    [SerializeField] private bool smoothMovement = true;
    [SerializeField] private float forwardSpeed, horizontalSpeed, verticalSpeed, runMultiplier = 2f;  

    // Update is called once per frame
    void Update()
    {
        if (smoothMovement)
        {
            Vector3 position = transform.localPosition;
            float run = Mathf.Clamp(Input.GetAxis("Run") * runMultiplier, 1f, runMultiplier);

            position += transform.forward 
                * (Input.GetAxis("Forward") * forwardSpeed) 
                * run
                * Time.unscaledDeltaTime;

            position += transform.right 
                * (Input.GetAxis("Horizontal") * horizontalSpeed) 
                * run
                * Time.unscaledDeltaTime;

            position += new Vector3(0f, 
                Input.GetAxis("Vertical") * verticalSpeed 
                * run 
                * Time.unscaledDeltaTime, 
                0f);

            transform.localPosition = position;
        }

        else
        {
            Vector3 position = transform.localPosition;

            position += transform.forward * (Input.GetAxisRaw("Forward") * forwardSpeed * Time.unscaledDeltaTime);
            position += transform.right * (Input.GetAxisRaw("Horizontal") * horizontalSpeed * Time.unscaledDeltaTime);
            position += new Vector3(0f, Input.GetAxisRaw("Vertical") * verticalSpeed * Time.unscaledDeltaTime, 0f);
            transform.localPosition = position;
        }
    }
}
