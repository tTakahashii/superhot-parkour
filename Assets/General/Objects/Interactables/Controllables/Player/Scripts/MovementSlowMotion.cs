using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSlowMotion : MonoBehaviour
{
    [SerializeField] private MovementManager movementManager;
    [SerializeField] private bool slowmoEnabled = true;
    [SerializeField] private float speedUpScale, slowedDownScale, transitionSecond;

    private float startingScale, currentScale, desiredScale, transitionTimer = 0f;

    private enum TimescaleState { slowingDown, speedingUp, neutral }

    private bool movementStateChanged = false, staticStateChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ScaleChanger");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.timeScale);

        if (movementManager.GetVelocity().magnitude > 0.1f)
        {
            if (!movementStateChanged)
            {
                transitionTimer = 0f;
                startingScale = GameState.GetTimeScale();
                desiredScale = speedUpScale;

                movementStateChanged = true;
                staticStateChanged = false;
            }
            
        }
        else
        {
            if (!staticStateChanged)
            {
                transitionTimer = 0f;
                startingScale = GameState.GetTimeScale();
                desiredScale = slowedDownScale;

                staticStateChanged = true;
                movementStateChanged = false;
            }
        }
    }

    private IEnumerator ScaleChanger()
    {
        while (true)
        {
            yield return new WaitUntil(() => !GameState.IsPaused() && slowmoEnabled);

            transitionTimer += Time.unscaledDeltaTime / transitionSecond;
            float scale = Mathf.Lerp(startingScale, desiredScale, transitionTimer);
            GameState.SetTimeScale(scale);

            //switch (currentState)
            //{
            //    case TimescaleState.slowingDown:
            //        Debug.Log("Slowing Down");
                    

            //        break;

            //    case TimescaleState.speedingUp:
            //        Debug.Log("Speeding Up");
                    
            //        break;
            //}
        }
    }
}
