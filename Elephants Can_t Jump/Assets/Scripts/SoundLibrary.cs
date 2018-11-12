using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
