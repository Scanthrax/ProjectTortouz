using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {
    public float mutetime = 2f;

    public AudioClip stretchTentacle;
    public AudioClip[] launch;
    public AudioClip doorOpen;
    public AudioClip tentacleSlap;
    public AudioClip[] wallBreak;
    public AudioClip crateBreak;
    public AudioClip button;


    public static AudioSource[] AudioSource;
    public static AudioClip StretchTentacle;
    public static AudioClip[] Launch;
    public static AudioClip DoorOpen;
    public static AudioClip TentacleSlap;
    public static AudioClip[] WallBreak;
    public static AudioClip CrateBreak;
    public static AudioClip Button;

    // Use this for initialization
    void Start () {
        AudioSource = GetComponents<AudioSource>();

        StretchTentacle = stretchTentacle;
        Launch = launch;
        DoorOpen = doorOpen;
        TentacleSlap = tentacleSlap;
        WallBreak = wallBreak;
        CrateBreak = crateBreak;
        Button = button;


        AudioSource[0].volume = 0f;
        MuteVolume();
    }
	


    void MuteVolume()
    {
        StartCoroutine(AnimateMove());
    }

    IEnumerator AnimateMove()
    {
        // timer for moving the menu
        float journey = 0f;

        // keep adjusting the position while there is time
        while (journey <= mutetime)
        {
            // add to timer
            journey = journey + Time.deltaTime;
            // wait a frame
            yield return null;
        }

        // the loop is now over
        AudioSource[0].volume = 1f;


    }


}
