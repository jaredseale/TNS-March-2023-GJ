using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame() {
        FindObjectOfType<LevelLoader>().LoadSceneWithDelay("Level 1");
    }

    public void exitGame() {
        Application.Quit();
    }
}
