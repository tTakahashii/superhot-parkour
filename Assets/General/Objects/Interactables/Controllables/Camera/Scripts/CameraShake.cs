using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //[SerializeField] private Vector3 randomnessOffset;
    //[SerializeField] private float duration = 5f, shakePerSecond = 10f;

    //private IEnumerator shakeCamera;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    shakeCamera = ShakeCamera(randomnessOffset, duration, shakePerSecond);
    //    StartCoroutine(shakeCamera);
    //}

    public IEnumerator ShakeCamera(Vector3 randomnessOffset, float duration, float shakePerSecond, bool looping = false)
    {
        Vector3 originalLocalPoint = transform.localPosition;
        Vector3 currentPosition, startingLocalPoint, targetLocalPoint;
        float durationTimer = 0f, shakeTimer = 0f, shakeDelta = 0f;
        float shakeConstant = 1f / shakePerSecond;

        while (looping || durationTimer <= duration)
        {
            currentPosition = startingLocalPoint = transform.localPosition;

            targetLocalPoint = originalLocalPoint + new Vector3(Random.Range(-randomnessOffset.x, randomnessOffset.x),
                Random.Range(-randomnessOffset.y, randomnessOffset.y),
                Random.Range(-randomnessOffset.z, randomnessOffset.z));

            //Debug.Log(targetLocalPoint);

            while (transform.localPosition != targetLocalPoint)
            {
                shakeDelta += Time.deltaTime / shakeConstant;

                currentPosition = Vector3.Slerp(startingLocalPoint, targetLocalPoint, shakeDelta);

                transform.localPosition = currentPosition;

                shakeTimer += Time.deltaTime;
                yield return null;
            }

            durationTimer += shakeTimer;
            shakeTimer = shakeDelta = 0f;

            yield return null;
        }          
    }
}
