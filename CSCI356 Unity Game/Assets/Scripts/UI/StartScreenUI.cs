using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StartScreenUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject StartScreen;
    public GameObject LevelSelectScreen;

    public GameObject LastScreen;
    public GameObject CurrentScreen;
    public void StartGame()
    {
        StartScreen.SetActive(false);
        LevelSelectScreen.SetActive(true);
        LastScreen = StartScreen;
        CurrentScreen = LevelSelectScreen;

    }

    public void SelectScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Back()
    {
        LastScreen.SetActive(true);
        CurrentScreen.SetActive(false);
        GameObject x = LastScreen;
        LastScreen = CurrentScreen;
        CurrentScreen = x;
    
    }
}
