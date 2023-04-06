using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCreator : MonoBehaviour
{
    [SerializeField] private Transform creationOrigin;
    [SerializeField] private List<GameObject> objectsToCreate = new List<GameObject>();

    private enum RandomState {oneSided, twoSided}
    [SerializeField] private RandomState randomizerMethod = RandomState.twoSided;
    [SerializeField] private Vector3 randomizerOffset;
    [SerializeField] private Vector2 randomizedDelay;
    [SerializeField] private float destroyTime = 10f;

    private IEnumerator coroutine;

    // Update is called once per frame
    void Start()
    {
        coroutine = CreateRandomized(objectsToCreate, creationOrigin, randomizerMethod, randomizerOffset, randomizedDelay, destroyTime);
        StartCoroutine(coroutine);
    }

    private void Update()
    {
    }

    private IEnumerator CreateRandomized(List<GameObject> objectsToCreate, Transform creationOrigin, RandomState randomizerMethod, Vector3 randomizerOffset, Vector2 randomizedDelay, float destroyTime = 0f)
    { 
        GameObject instantiated;
        Vector3 randomizedPosition = Vector3.zero;

        while (true)
        {
            yield return new WaitUntil(() => !GameState.IsPaused());

            foreach (GameObject gameobject in objectsToCreate)
            {
                if (randomizerMethod == RandomState.oneSided)
                {
                    randomizedPosition.x = Random.Range(0f, randomizerOffset.x);
                    randomizedPosition.y = Random.Range(0f, randomizerOffset.y);
                    randomizedPosition.z = Random.Range(0f, randomizerOffset.z);
                }
                else
                {
                    randomizedPosition.x = Random.Range(-randomizerOffset.x, randomizerOffset.x);
                    randomizedPosition.y = Random.Range(-randomizerOffset.y, randomizerOffset.y);
                    randomizedPosition.z = Random.Range(-randomizerOffset.z, randomizerOffset.z);
                }

                instantiated = Instantiate(gameobject, creationOrigin.position + randomizedPosition, Quaternion.identity, creationOrigin);
                if (destroyTime > 0f) { Destroy(instantiated, destroyTime); }                
            }

            yield return new WaitForSecondsRealtime(Random.Range(randomizedDelay.x, randomizedDelay.y));
        }    
    }
}
