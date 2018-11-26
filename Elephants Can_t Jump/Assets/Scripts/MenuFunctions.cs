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
        StartCoroutine(Fade.FadeOut(1.5f, GameScene));
    }


    public void ContinueGame()
    {
        StartCoroutine(Fade.FadeOut(1.5f, GameScene));
    }

    public static void LoadGame(Object scene)
    {
        SceneManager.LoadScene(scene.name);
    }
}
