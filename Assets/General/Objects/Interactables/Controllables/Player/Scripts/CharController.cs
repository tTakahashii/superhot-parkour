using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    private void OnEnable()
    {
        if (TryGetComponent(out CharacterController charController))
        {
            charController.enabled = true;
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent(out CharacterController charController))
        {
            charController.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
