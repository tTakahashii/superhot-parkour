using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementShake : MonoBehaviour
{
    public bool shakeEnabled = true;

    [SerializeField] private MovementManager movementManager;

    [Header("Required Points")]
    [SerializeField] private Vector3 startingPoint;
    [SerializeField] private Vector3 shakeOffset;

    [Header("General Settings")]
    [SerializeField] private Vector3 speedMultipliers;
    [SerializeField] private float speedClamp;

    private Vector3 interpolators = Vector3.zero, defaultInterpolators = Vector3.zero, latestPosition = Vector3.zero;
    private bool xCycle = false, yCycle = false, zCycle = false;

    private void Update()
    {
        if (shakeEnabled)
        {
            ShakeCamera();
        }
    }

    private void ShakeCamera()
    {
        Vector3 currentPosition = Vector3.zero;
        float magnitude = Mathf.Clamp(movementManager.GetVelocity().magnitude, 0f, speedClamp);;
        //Debug.Log("RB VELOCTY: " + speed);
        //Debug.Log("SPEED:" + Time.time * speed);

        if (magnitude > 0.01f && movementManager.GetVelocity().y < 0.1f)
        {
            defaultInterpolators = Vector3.zero;     

            switch (xCycle)
            {
                case true:
                    interpolators.x += -(Time.deltaTime * magnitude * speedMultipliers.x);
                    xCycle = interpolators.x > 0f ? true : false;
                    //Debug.Log("X Decreasing");
                    break;

                case false:
                    interpolators.x += Time.deltaTime * magnitude * speedMultipliers.x;
                    xCycle = interpolators.x < 1f ? false : true;
                    //Debug.Log("X Increasing");
                    break;
            }

            switch (yCycle)
            {
                case true:
                    interpolators.y += -(Time.deltaTime * magnitude * speedMultipliers.y);
                    yCycle = interpolators.y > 0f ? true : false;

                    break;

                case false:
                    interpolators.y += Time.deltaTime * magnitude * speedMultipliers.y;
                    yCycle = interpolators.y < 1f ? false : true;

                    break;
            }

            switch (zCycle)
            {
                case true:
                    interpolators.z += -(Time.deltaTime * magnitude * speedMultipliers.z);
                    zCycle = interpolators.z > 0f ? true : false;

                    break;

                case false:
                    interpolators.z += Time.deltaTime * magnitude * speedMultipliers.z;
                    zCycle = interpolators.z < 1f ? false : true;

                    break;
            }

            currentPosition.x = Mathf.Lerp(-shakeOffset.x, shakeOffset.x, interpolators.x);
            currentPosition.y = Mathf.Lerp(-shakeOffset.y, shakeOffset.y, interpolators.y);
            currentPosition.z = Mathf.Lerp(-shakeOffset.z, shakeOffset.z, interpolators.z);

            latestPosition = currentPosition;

            //Debug.Log("MOVING");
        }

        else
        {
            defaultInterpolators.x += Time.deltaTime * speedMultipliers.x;
            defaultInterpolators.y += Time.deltaTime * speedMultipliers.y;
            defaultInterpolators.z += Time.deltaTime * speedMultipliers.z;

            currentPosition.x = Mathf.Lerp(latestPosition.x, startingPoint.x, defaultInterpolators.x);
            currentPosition.y = Mathf.Lerp(latestPosition.y, startingPoint.y, defaultInterpolators.y);
            currentPosition.z = Mathf.Lerp(latestPosition.z, startingPoint.z, defaultInterpolators.z);

            //Debug.Log("STATIC");
        }

        //Debug.Log(defaultInterpolators);

        //Debug.Log("X: " + interpolators.x);

        

        //currentPosition.x = shakeOffset.x != 0f ? Mathf.PingPong(Time.time * speed * speedMultipliers.x, shakeOffset.x * 2f) - shakeOffset.x : 0f;
        //currentPosition.y = shakeOffset.y != 0f ? Mathf.PingPong(Time.time * speed * speedMultipliers.y, shakeOffset.y * 2f) - shakeOffset.y : 0f;
        //currentPosition.z = shakeOffset.z != 0f ? Mathf.PingPong(Time.time * speed * speedMultipliers.z, shakeOffset.z * 2f) - shakeOffset.z : 0f;

        //currentPosition = speed > 0.01f ? currentPosition : startingPoint;

        transform.localPosition = currentPosition;
    }
}