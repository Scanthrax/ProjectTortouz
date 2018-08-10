using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    //DontDestroyOnLoad(Camera.main);
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }
}
