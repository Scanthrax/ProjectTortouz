using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {

    public GameObject Credits;
    public Transform endPoint;

    public float speed;

    bool endOfCredits;

    bool achieved = true;


    public UnityEngine.UI.Button goToMainMenu;

    // Use this for initialization
    void Start () {

        MusicManager.instance.PlaySong(Music.Credits);

        StartCoroutine(Fade.instance.FadeIn(1.5f));

        endOfCredits = false;


        goToMainMenu.onClick.AddListener(delegate { StartCoroutine(Fade.instance.FadeOut(2f,"TestMenu_10-28")); });

        Cursor.visible = true;

    }
	
	// Update is called once per frame
	void Update ()
    {
        //Move credits towards end point
        Credits.transform.position = Vector3.MoveTowards(Credits.transform.position, endPoint.position, Time.deltaTime*speed);


        if(!endOfCredits)
        {
            if(Credits.transform.position.y <= endPoint.position.y)
            {
                endOfCredits = true;


                // this checks if the achievement has been achieved & spits out the result in the bool
                SteamUserStats.GetAchievement("Watch Credits", out achieved);

                // if we don't have the achievement yet, we can say that we've achieved it
                if (!achieved)
                {
                    // set the achievement
                    SteamUserStats.SetAchievement("Watch Credits");
                    // store it on steam's side
                    SteamUserStats.StoreStats();
                }
            }
        }

        if (Input.GetButton("Back"))
        {
            MainMenu();
        }

    }


    public void MainMenu()
    {
        StartCoroutine(Fade.instance.FadeOut(1.5f, "TestMenu_10-28"));
    }
}
