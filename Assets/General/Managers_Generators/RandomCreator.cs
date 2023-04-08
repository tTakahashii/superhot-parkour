using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCreator : MonoBehaviour
{
    [SerializeField] private Transform creationOrigin;
    [SerializeField] private List<GameObject> objectsToCreate = new List<GameObject>();

    [SerializeField] private Vector3 randomizerOffset;
    [SerializeField] private Vector2 randomizedDelay;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private Color gizmoColor;

    private bool coroutineDone = true;

    private IEnumerator coroutine;

    private void Update()
    {
        if (coroutineDone)
        {
            coroutine = CreateRandomized(objectsToCreate, creationOrigin, randomizerOffset, randomizedDelay, destroyTime);
            StartCoroutine(coroutine);
            //Debug.Log("Restart...");
            
        }

        //Debug.Log(coroutineDone);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireCube(creationOrigin.position, randomizerOffset * 2f);
    }

    private IEnumerator CreateRandomized(List<GameObject> objectsToCreate, Transform creationOrigin, Vector3 randomizerOffset, Vector2 randomizedDelay, float destroyTime = 0f)
    {
        coroutineDone = false;
        yield return new WaitUntil(() => !GameState.IsPaused());

        GameObject instantiated;
        Vector3 randomizedPosition = Vector3.zero;       

        foreach (GameObject gameobject in objectsToCreate)
        {
            randomizedPosition.x = Random.Range(-randomizerOffset.x, randomizerOffset.x);
            randomizedPosition.y = Random.Range(-randomizerOffset.y, randomizerOffset.y);
            randomizedPosition.z = Random.Range(-randomizerOffset.z, randomizerOffset.z);

            //Debug.Log(randomizedPosition);

            instantiated = Instantiate(gameobject, creationOrigin.position + randomizedPosition, Quaternion.identity, creationOrigin);
            if (destroyTime > 0f) { Destroy(instantiated, destroyTime); }
        }

        yield return new WaitForSecondsRealtime(Random.Range(randomizedDelay.x, randomizedDelay.y));
        coroutineDone = true;
    }
}
