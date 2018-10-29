using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuFunctions : MonoBehaviour
{
    public Object gameScene;
    //public SaveController saveController;

    public void NewGame()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
        }
        LoadGame();
    }


    public void ContinueGame()
    {
        LoadGame();
    }

    void LoadGame()
    {
        SceneManager.LoadScene(gameScene.name);
    }
}
