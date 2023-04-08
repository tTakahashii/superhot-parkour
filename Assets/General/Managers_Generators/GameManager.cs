using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    private static bool gamePaused = false;
    private static float timescale = 1f;

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
    [SerializeField] private List<GameObject> enabledAtGamePause;
    [SerializeField] private List<GameObject> disabledAtGamePause;
    [SerializeField] private List<GameObject> instantiatedAtGameResume, instantiatedAtGamePause;
    private List<GameObject> createdAtGameResume, createdAtGamePause;

    [SerializeField] private Transform mainUI;

    private bool gamePaused;
    [HideInInspector] public float timescale = 1f;

    private void Awake()
    {
        createdAtGameResume = createdAtGamePause = new List<GameObject>();
    }

    private void Start()
    {
        gamePaused = GameState.IsPaused();

        Time.timeScale = gamePaused ? 0f : 1f;
        MouseManager(gamePaused);

        ObjectManager.EnableAtGamePause(gamePaused, enabledAtGamePause);
        ObjectManager.DisableAtGamePause(gamePaused, disabledAtGamePause);
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

            Time.timeScale = gamePaused ? 0f : GameState.GetTimeScale();

            MouseManager(gamePaused);

            ObjectManager.DestroyAtGamePause(gamePaused, createdAtGameResume);
            ObjectManager.DestroyAtGameResume(gamePaused, createdAtGamePause);

            createdAtGamePause = ObjectManager.InstantiateAtGamePause(gamePaused, instantiatedAtGamePause, mainUI);
            createdAtGameResume = ObjectManager.InstantiateAtGameResume(gamePaused, instantiatedAtGameResume, mainUI);

            ObjectManager.EnableAtGamePause(gamePaused, enabledAtGamePause);
            ObjectManager.DisableAtGamePause(gamePaused, disabledAtGamePause);
        }
    }

    private static class ObjectManager
    {
        static internal List<GameObject> InstantiateAtGamePause(bool gamePaused, List<GameObject> instantiatedAtGamePause, Transform parent = null)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject instantiated;
            int index = 0;

            if (gamePaused)
            {
                foreach (GameObject gameobject in instantiatedAtGamePause)
                {
                    instantiated = Instantiate(gameobject, parent);
                    instantiated.transform.SetSiblingIndex(index);
                    createdObjects.Add(instantiated);

                    index++;
                }
            }

            return createdObjects;
        }

        static internal List<GameObject> InstantiateAtGameResume(bool gamePaused, List<GameObject> instantiatedAtGameResume, Transform parent = null)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject instantiated;
            int index = 0;

            if (!gamePaused)
            {
                foreach (GameObject gameobject in instantiatedAtGameResume)
                {
                    instantiated = Instantiate(gameobject, parent);
                    instantiated.transform.SetSiblingIndex(index);
                    createdObjects.Add(instantiated);

                    index++;
                }
            }

            return createdObjects;
        }

        static internal void DestroyAtGamePause(bool gamePaused, List<GameObject> destroyedAtGamePause, float delay = 0f)
        {
            if (gamePaused)
            {
                foreach (GameObject gameobject in destroyedAtGamePause)
                {
                    Destroy(gameobject, delay);
                }

                destroyedAtGamePause.Clear();
            }
        }

        static internal void DestroyAtGameResume(bool gamePaused, List<GameObject> destroyedAtGameResume, float delay = 0f)
        {
            if (!gamePaused)
            {
                foreach (GameObject gameobject in destroyedAtGameResume)
                {
                    Destroy(gameobject, delay);
                }

                destroyedAtGameResume.Clear();
            }    
        }

        static internal void EnableAtGamePause(bool gamePaused, List<GameObject> enabledAtGamePause)
        {
            foreach (GameObject gameobject in enabledAtGamePause)
            {
                gameobject.SetActive(gamePaused);
            }
        }

        static internal void DisableAtGamePause(bool gamePaused, List<GameObject> disabledAtGamePause)
        {
            foreach (GameObject gameobject in disabledAtGamePause)
            {
                gameobject.SetActive(!gamePaused);
            }
        }
    }
}
