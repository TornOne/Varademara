﻿
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	
	public void StartGame()
    {
        print("ok");
        SceneManager.LoadScene("Main");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
