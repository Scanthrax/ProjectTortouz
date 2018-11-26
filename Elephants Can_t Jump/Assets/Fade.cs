﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    Image screen;
    public static Image Screen;

	void Awake ()
    {
        
        print(gameObject);
        screen = GetComponent<Image>();
        Screen = screen;

    }
	
	public static IEnumerator FadeIn(float time)
    {
        float timer = 0f;
        float percent = 0f;
        Screen.color = new Color(Screen.color.r, Screen.color.g, Screen.color.b, 1);

        while (timer <= time)
        {
            timer += Time.deltaTime * 0.8f;
            percent = timer / time;
            Screen.color = new Color(Screen.color.r, Screen.color.g, Screen.color.b, 1-percent);
            yield return null;
        }
    }


    public static IEnumerator FadeOut(float time, Object scene)
    {
        float timer = 0f;
        float percent = 0f;

        Screen.color = new Color(Screen.color.r, Screen.color.g, Screen.color.b, 0);

        while (timer <= time)
        {

            timer += Time.deltaTime;
            percent = timer / time;
            Screen.color = new Color(Screen.color.r, Screen.color.g, Screen.color.b, percent);
            yield return null;
        }

        MenuFunctions.LoadGame(scene);
    }


}
