using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour {

    KeyCode esc = KeyCode.Escape;
    public GameObject buttons;
    
    private void Start()
    {
        buttons.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(esc))
        {
            buttons.SetActive(!buttons.activeSelf);
        }
    }

    public void Restart()
    {
        StartCoroutine(Fade.FadeOut(1.5f, "Buildout_Art_Final"));
    }

    public void MainMenu()
    {
        StartCoroutine(Fade.FadeOut(1.5f, "TestMenu_10-28"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
