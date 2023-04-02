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
        if (!GameState.GetState())
        {
            Vector3 position = transform.localPosition;

            if (smoothMovement)
            {
                float run = Mathf.Clamp((Input.GetAxis("Run") / Time.timeScale) * runMultiplier, 1f, runMultiplier);

                position += transform.forward
                    * ((Input.GetAxis("Forward") / Time.timeScale) * forwardSpeed)
                    * run
                    * Mathf.Clamp(Time.unscaledDeltaTime, 0f, 1f/3f);

                position += transform.right
                    * ((Input.GetAxis("Horizontal") / Time.timeScale) * horizontalSpeed)
                    * run
                    * Mathf.Clamp(Time.unscaledDeltaTime, 0f, 1f/3f);

                position += new Vector3(0f,
                    (Input.GetAxis("Vertical") / Time.timeScale) * verticalSpeed
                    * run
                    * Mathf.Clamp(Time.unscaledDeltaTime, 0f, 1f/3f),
                0f);

                transform.localPosition = position;
            }

            else
            {
                float run = Mathf.Clamp(Input.GetAxisRaw("Run") * runMultiplier, 1f, runMultiplier);

                position += transform.forward
                    * ((Input.GetAxisRaw("Forward") * forwardSpeed)
                    * run
                    * Time.unscaledDeltaTime);
                position += transform.right
                    * ((Input.GetAxisRaw("Horizontal") * horizontalSpeed)
                    * run
                    * Time.unscaledDeltaTime);
                position += new Vector3(0f,
                    (Input.GetAxisRaw("Vertical") * verticalSpeed)
                    * run
                    * Time.unscaledDeltaTime,
                    0f);
                transform.localPosition = position;
            }
        }

        
    }
}
