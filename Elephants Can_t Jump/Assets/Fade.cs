using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {


    public static Fade instance;

    public Transform canvas;

    public Image screen;



	void Awake ()
    {
        if (instance == null)
            instance = this;
        else
            print("This already exists!");

        DontDestroyOnLoad(canvas.gameObject);


    }
	
	public IEnumerator FadeIn(float time)
    {
        print("fading");
        float timer = 0f;
        float percent = 0f;
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 1);

        while (timer <= time)
        {
            timer += Time.deltaTime;
            percent = timer / time;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 1-percent);
            yield return null;
        }
    }


    public IEnumerator FadeOut(float time, Object scene)
    {
        float timer = 0f;
        float percent = 0f;

        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 0);

        while (timer <= time)
        {

            timer += Time.deltaTime;
            percent = timer / time;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, percent);
            yield return null;
        }

        MenuFunctions.LoadGame(scene);
    }


    public IEnumerator FadeOut(float time, string str)
    {
        float timer = 0f;
        float percent = 0f;

        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 0);

        while (timer <= time)
        {

            timer += Time.deltaTime;
            percent = timer / time;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, percent);
            yield return null;
        }

        MenuFunctions.LoadGame(str);
    }


}
