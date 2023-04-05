using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    [Header("Sensitivity & Axis")]
    [Tooltip("Give negative values to invert")]
    [SerializeField] private float mouseSpeedX;
    [SerializeField] private float mouseSpeedY;
    [SerializeField] private bool clampX, clampY;
    [SerializeField] private Vector2 xClampValues, yClampValues;

    [Header("Lerp & Transition")]
    [SerializeField] private bool smoothLerp = true;
    [SerializeField] private float lerpSpeed;

    private Vector3 currentRotation, targetRotation;
    private float lerpTimer = 0f;

    private void Start()
    {
        currentRotation = Vector3.zero;
        targetRotation = transform.localEulerAngles;      
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameState.IsPaused())
        {
            targetRotation += new Vector3(Input.GetAxis("Mouse Y") * mouseSpeedY, Input.GetAxis("Mouse X") * mouseSpeedX, 0f);
            targetRotation.x = clampY ? Mathf.Clamp(targetRotation.x, yClampValues.x, yClampValues.y) : targetRotation.x;
            targetRotation.y = clampX ? Mathf.Clamp(targetRotation.y, yClampValues.x, yClampValues.y) : targetRotation.y;
            //targetRotation.z = 0f;

            lerpTimer += Time.unscaledDeltaTime * lerpSpeed;
            lerpTimer = targetRotation == currentRotation ? 0f : lerpTimer;
            currentRotation = lerpTimer >= 1f ? targetRotation : currentRotation;

            if (smoothLerp)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(currentRotation), Quaternion.Euler(targetRotation), lerpTimer);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(Quaternion.Euler(currentRotation), Quaternion.Euler(targetRotation), lerpTimer);
            }
        }  
    }
}