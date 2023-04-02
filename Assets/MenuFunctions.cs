using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    public void ResumeGame() { GameState.ResumeGame(); }
    public void ExitGame() { Application.Quit(); }
}
