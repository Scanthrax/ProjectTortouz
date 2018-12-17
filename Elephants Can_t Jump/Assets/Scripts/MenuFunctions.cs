﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuFunctions : MonoBehaviour
{
    public Object gameScene;
    public Object menuScene;
    public static Object GameScene;
    public static Object MenuScene;

    private void Start()
    {
        GameScene = gameScene;
        MenuScene = menuScene;
    }

    public void NewGame()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
        }
        SaveController.buttonsDict = new Dictionary<string, bool>();
        SaveController.breakableDict = new Dictionary<string, bool>();
        SaveController.alienCollectables = new Dictionary<string, bool>();
        StartCoroutine(Fade.FadeOut(1.5f, "ComicScene"));
    }


    public void ContinueGame()
    {
        StartCoroutine(Fade.FadeOut(1.5f, "Buildout_Art_Final"));
    }

    public static void LoadGame(Object scene)
    {
        SceneManager.LoadScene(scene.name);
    }
    public static void LoadGame(string str)
    {
        SceneManager.LoadScene(str);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
