using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuFunctions : MonoBehaviour
{
    public Object gameScene;
    public static Object GameScene;

    private void Start()
    {
        GameScene = gameScene;
    }

    public void NewGame()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
        }
        StartCoroutine(Fade.FadeOut(1.5f));
    }


    public void ContinueGame()
    {
        StartCoroutine(Fade.FadeOut(1.5f));
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene(GameScene.name);
    }
}
