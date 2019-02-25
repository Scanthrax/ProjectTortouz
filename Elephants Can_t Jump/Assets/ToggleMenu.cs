using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour {

    KeyCode esc = KeyCode.Escape;
    public GameObject buttons;

    bool isMenuActive;


    private void Start()
    {
        buttons.SetActive(false);
        isMenuActive = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(esc) || Input.GetButtonDown("Menu"))
        {
            buttons.SetActive(!buttons.activeSelf);
            isMenuActive = !isMenuActive;
        }


        if (Input.GetButton("Grip") && Input.GetButton("Y"))
        {
            Restart();
        }


        if (Input.GetButton("Back"))
        {
            MainMenu();
        }




    }

    public void Restart()
    {
        StartCoroutine(Fade.instance.FadeOut(1.5f, "Buildout_Art_Final_2.0"));
    }

    public void MainMenu()
    {
        StartCoroutine(Fade.instance.FadeOut(1.5f, "TestMenu_10-28"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
