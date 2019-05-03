using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRoomManager : MonoBehaviour
{

    public MenuFunctions menuFunctions;
    public MenuSelection menuSelection;


    public UnityEngine.UI.Button continueButton;

    bool action;


    bool continueGame;

    void Start()
    {
        StartCoroutine(Fade.instance.FadeIn(0.2f));

        action = true;


        //SaveController.instance.DeleteFile();

        Cursor.visible = true;


        

        if (SaveController.instance.FileExists())
        {
            continueButton.gameObject.SetActive(true);
            continueGame = true;
        }
        else
        {
            continueGame = false;
            continueButton.gameObject.SetActive(false);
        }


        MusicManager.instance.PlaySong(Music.Menu);


    }



    private void Update()
    {
        if (action)
        {
            if (Input.GetButton("Grip") && Input.GetButton("Y"))
            {
                menuFunctions.NewGame();
                action = false;
            }


            if(Input.GetButton("Back"))
            {
                menuFunctions.QuitGame();
                action = false;
            }

            if (continueGame)
            {
                if (Input.GetButton("Launch"))
                {
                    menuFunctions.ContinueGame();
                    action = false;
                }
            }
            if (!menuSelection.switching)
            {
                if (Input.GetAxis("Tentacle Grab") > 0.1f && menuSelection.currentMenu == MenuSelection.Menu.Menu)
                {
                    menuSelection.GoToNextMenu(MenuSelection.Menu.Menu, MenuSelection.Menu.Critters, MenuSelection.Direction.Right);
                }
                else if (Input.GetAxis("Tentacle Grab") < -0.1f && menuSelection.currentMenu == MenuSelection.Menu.Critters)
                {
                    menuSelection.GoToNextMenu(MenuSelection.Menu.Critters, MenuSelection.Menu.Menu, MenuSelection.Direction.Left);
                }
            }
        }
    }
}
