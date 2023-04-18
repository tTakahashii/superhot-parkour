using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private Transform raycastCenterObject;
    [SerializeField] private LayerMask layersToIgnore;

    [SerializeField] private float pickupRange, throwForce, transitionSecond, objectDistance;
    [SerializeField] private bool smoothLerp = true;

    private Transform heldObject = null;
    private RaycastHit hit;

    private IEnumerator transitionCoroutine;

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(raycastCenterObject.position, raycastCenterObject.forward, out hit, pickupRange, ~layersToIgnore);

        if (Input.GetKeyDown("e") || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (heldObject != null)
            {
                StopAllCoroutines();

                if (heldObject.TryGetComponent<Rigidbody>(out Rigidbody heldRb))
                {
                    heldRb.isKinematic = false;
                    heldRb.detectCollisions = true;
                    heldRb.AddForce(raycastCenterObject.forward * throwForce, ForceMode.VelocityChange);
                }
        
                heldObject.SetParent(null);
                heldObject = null;
            }
            else
            {
                if (hit.transform != null)
                {
                    if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody heldRb))
                    {
                        StopAllCoroutines();

                        heldObject = hit.transform;
                        heldRb.isKinematic = true;
                        heldRb.detectCollisions = false;
                        heldObject.SetParent(raycastCenterObject);

                        transitionCoroutine = MoveObjectToCenter(heldObject, raycastCenterObject, transitionSecond, objectDistance, true, true, smoothLerp);
                        StartCoroutine(transitionCoroutine);
                    }
                }           
            }      
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(raycastCenterObject.position, raycastCenterObject.position + raycastCenterObject.forward * pickupRange);
    }

    private IEnumerator MoveObjectToCenter(Transform heldObject, Transform targetObject, float transitionSecond, float objectDistance, bool move, bool rotate, bool smoothLerp)
    {
        Vector3 initialPosition = heldObject.position;
        Quaternion initialRotation = heldObject.rotation;
        float interpolator = 0f;

        //Debug.Log("Coroutine Started");

        while (true)
        {
            if (smoothLerp)
            {
                if (move)
                {
                    heldObject.position = Vector3.Slerp(initialPosition, (targetObject.position + (targetObject.forward * objectDistance)), interpolator);

                    //Debug.Log("POS: " + targetObject.position);
                    //Debug.Log("DIR: " + targetObject.forward);
                    //Debug.Log("SUM: " + (targetObject.position + targetObject.forward));
                }

                if (rotate)
                {
                    heldObject.rotation = Quaternion.Slerp(initialRotation, targetObject.rotation, interpolator);
                }
            }

            else
            {
                if (move)
                {
                    heldObject.position = Vector3.Lerp(initialPosition, (targetObject.position + (targetObject.forward * objectDistance)), interpolator);

                    //Debug.Log("POS: " + targetObject.position);
                    //Debug.Log("DIR: " + targetObject.forward);
                    //Debug.Log("SUM: " + (targetObject.position + targetObject.forward));
                }

                if (rotate)
                {
                    heldObject.rotation = Quaternion.Lerp(initialRotation, targetObject.rotation, interpolator);
                }
            }

            interpolator += Time.deltaTime / transitionSecond;
            yield return null;
        }      
    }
}
