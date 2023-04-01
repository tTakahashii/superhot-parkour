using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    [Tooltip("Give negative values to invert")] [SerializeField] private float mouseSpeedX, mouseSpeedY, lerpSpeed;

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
        targetRotation += new Vector3(Input.GetAxis("Mouse Y") * mouseSpeedY, Input.GetAxis("Mouse X") * mouseSpeedX, 0f);
        targetRotation.z = 0f;

        lerpTimer = targetRotation == currentRotation ? 0f : lerpTimer;
        currentRotation = lerpTimer >= 1f ? targetRotation : currentRotation;

        transform.rotation = Quaternion.Lerp(Quaternion.Euler(currentRotation), Quaternion.Euler(targetRotation), lerpTimer);
        lerpTimer += Time.deltaTime * lerpSpeed;
    }
}