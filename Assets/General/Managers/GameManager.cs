using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    private static bool gamePaused = false;
    private static float timescale = 1f;

    public static bool GetState()
    {
        return gamePaused;
    }

    public static void ResumeGame()
    {
        gamePaused = false;
    }

    public static void PauseGame()
    {
        gamePaused = true;
    }

    public static float GetTimeScale()
    {
        return timescale;
    }

    public static void SetTimeScale(float scale = 1f)
    {
        timescale = scale;
    }
}

public class GameManager : MonoBehaviour
{
    [Header("Object Management")]
    [SerializeField] private GameObject[] objectsEnabledAtGamePause;
    [SerializeField] private GameObject[] objectsDisabledAtGamePause;

    private bool gamePaused;
    [HideInInspector] public float timescale = 1f;

    private void Start()
    {
        gamePaused = GameState.GetState();

        Time.timeScale = gamePaused ? 0.001f : 1f;
        MouseManager(gamePaused);

        EnableAtGamePause(objectsEnabledAtGamePause);
        DisableAtGamePause(objectsDisabledAtGamePause);
    }

    void Update()
    {
        OnStateChange();
        StateManager();
    }

    private void MouseManager(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;     
    }

    private void StateManager()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                GameState.ResumeGame();
            }
            else
            {
                GameState.PauseGame();
            }
        }
    }

    private void OnStateChange()
    {
        if (gamePaused != GameState.GetState())
        {
            gamePaused = GameState.GetState();

            Time.timeScale = gamePaused ? 0.01f : GameState.GetTimeScale();

            MouseManager(gamePaused);

            EnableAtGamePause(objectsEnabledAtGamePause);
            DisableAtGamePause(objectsDisabledAtGamePause);
        }
    }

    

    private void EnableAtGamePause(GameObject[] objectsEnabledAtGamePause)
    {
        foreach (GameObject gameobject in objectsEnabledAtGamePause)
        {
            gameobject.SetActive(gamePaused);
        }
    }

    private void DisableAtGamePause(GameObject[] objectsDisabledAtGamePause)
    {
        foreach (GameObject gameobject in objectsDisabledAtGamePause)
        {
            gameobject.SetActive(!gamePaused);
        }
    }
}
