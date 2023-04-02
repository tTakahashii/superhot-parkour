using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    private static bool gamePaused = false;

    public static bool IsPaused()
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
}

public class GameManager : MonoBehaviour
{
    [Header("Object Management")]
    [SerializeField] private GameObject[] objectsEnabledAtGamePause;
    [SerializeField] private GameObject[] objectsDisabledAtGamePause;

    private bool gamePaused;

    private void Start()
    {
        gamePaused = GameState.IsPaused();

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
        if (gamePaused != GameState.IsPaused())
        {
            gamePaused = GameState.IsPaused();

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
