using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private GameObject devConsole;

    public void ResumeGame() { GameState.ResumeGame(); }
    public void ExitGame() { Application.Quit(); }

    public void OpenDeveloperConsole()
    {
        GameObject console = Instantiate(devConsole);
        console.transform.SetParent(transform);
        console.transform.position = Vector3.zero;
        console.transform.rotation = Quaternion.identity;
        
    }
}
