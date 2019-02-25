using System.Collections;
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
        StartCoroutine(Fade.instance.FadeOut(1.5f, "ComicScene"));
    }


    public void ContinueGame()
    {
        string scene = !SaveController.instance.FileExists() ? "ComicScene" : "Buildout_Art_Final_2.0";

        StartCoroutine(Fade.instance.FadeOut(1.5f, scene));
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


    public void GoToGame()
    {

        StartCoroutine(Fade.instance.FadeOut(1.5f, "Buildout_Art_Final_2.0"));
    }

}
