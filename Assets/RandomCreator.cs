using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCreator : MonoBehaviour
{
    [SerializeField] private Transform creationOrigin;

    private enum RandomState {oneSided, twoSided}
    [SerializeField] RandomState randomizerMethod = RandomState.twoSided;
    [SerializeField] private Vector3 randomizerOffset;
    [SerializeField] private Vector2 randomizedDelay;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private List<GameObject> objectsToCreate = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CreateRandomized(List<GameObject> objectsToCreate, Transform creationOrigin, RandomState randomizerMethod, Vector2 randomizedDelay)
    { 
        GameObject instantiated;

        foreach (GameObject gameobject in objectsToCreate)
        {
            instantiated = Instantiate(gameobject);
            yield return new WaitForSecondsRealtime(Random.Range(randomizedDelay.x + 0.1f, randomizedDelay.y));
        }
    }
}
