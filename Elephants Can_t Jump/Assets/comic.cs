using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comic : MonoBehaviour {

    public RectTransform[] pages;

    int pageNum = 0;

    int dir = 1;


    public Transform upButton, downButton;


    public Text sceneButtonText;

    bool switching;

    MenuFunctions menuFunctions;

    private void Start()
    {
        upButton.gameObject.SetActive(false);

        print("attempting to fade");
        StartCoroutine(Fade.instance.FadeIn(0.5f));


        MusicManager.instance.PlaySong(Music.Comic);

        switching = false;

        menuFunctions = GetComponent<MenuFunctions>();
    }



    private void Update()
    {
        if(Input.GetButton("Y"))
        {
            PreviousPage();
        }

        if (Input.GetButton("Launch"))
        {
            NextPage();
        }


        if (Input.GetButton("Wall Bash"))
        {
            menuFunctions.GoToGame();
        }
    }


    public void NextPage()
    {
        if (switching) return;

        if (pageNum >= pages.Length - 1) return;

        pageNum++;
        dir = 1;

        GoToNextMenu(pages[pageNum - 1], pages[pageNum]);

        if (pageNum == 2)
        {
            downButton.gameObject.SetActive(false);
            sceneButtonText.text = "PLAY";
        }
        else
        {
            downButton.gameObject.SetActive(true);
            sceneButtonText.text = "SKIP";
        }

        if (pageNum == 0)
            upButton.gameObject.SetActive(false);

        else
            upButton.gameObject.SetActive(true);

    }



    public void PreviousPage()
    {
        if (switching) return;

        if (pageNum <= 0) return;

        pageNum--;
        dir = -1;

        

        GoToNextMenu(pages[pageNum + 1], pages[pageNum]);


        if (pageNum == 2)
        {
            downButton.gameObject.SetActive(false);
            sceneButtonText.text = "PLAY";
        }
        else
        {
            downButton.gameObject.SetActive(true);
            sceneButtonText.text = "SKIP";
        }

        if (pageNum == 0)
            upButton.gameObject.SetActive(false);

        else
            upButton.gameObject.SetActive(true);
    }





    // <summary>
    /// Transitions to next menu
    /// </summary>
    /// <param name="outMenu">The menu we are leaving</param>
    /// <param name="inMenu">The menu we are entering</param>
    /// <param name="dir">The menu we are entering</param>
    public void GoToNextMenu(RectTransform outMenu, RectTransform inMenu)
    {
        MoveMenus(outMenu, true);
        MoveMenus(inMenu, false);
    }

    /// <summary>
    /// Transition between 2 menus
    /// </summary>
    /// <param name="obj">The menu to move</param>
    /// <param name="setInactive">Do we set the menu as inactive after transitioning?</param>
    void MoveMenus(RectTransform obj, bool setInactive)
    {
        StartCoroutine(AnimateMove(obj, setInactive));
    }

    IEnumerator AnimateMove(RectTransform obj, bool setInactive)
    {
        switching = true;

        Vector2 target;
        Vector2 origin;
        Vector2 offset = new Vector2(0, -2840 * dir);


        // If false, this means that the menu is currently disabled.  We need to enable it & set it off screen so it can swipe in
        if (!setInactive)
        {
            obj.gameObject.SetActive(true);
            origin = offset;
            target = Vector2.zero;
        }
        // otherwise, we need to tell the current menu to go off the screen
        else
        {
            origin = Vector2.zero;
            target = -offset;
        }

        // timer for moving the menu
        float journey = 0f;
        // percentage of completion, used for finding position on animation curve
        float percent = 0f;

        float duration = 1f;

        // keep adjusting the position while there is time
        while (journey <= duration)
        {
            // add to timer
            journey = journey + Time.deltaTime;
            // calculate percentage
            percent = Mathf.Clamp01(journey / duration);
            // adjust the position of the menu
            obj.transform.localPosition = Vector2.LerpUnclamped(origin, target, percent);
            // wait a frame
            yield return null;
        }

        // the loop is now over, so if the menu is going out, we disable it
        if (setInactive)
            obj.gameObject.SetActive(false);

        switching = false;
    }



}
