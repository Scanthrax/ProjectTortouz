using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {

    public AudioClip stretchTentacle;
    public AudioClip[] launch;
    public AudioClip doorOpen;


    public static AudioSource[] AudioSource;
    public static AudioClip StretchTentacle;
    public static AudioClip[] Launch;
    public static AudioClip DoorOpen;

    // Use this for initialization
    void Start () {
        AudioSource = GetComponents<AudioSource>();

        StretchTentacle = stretchTentacle;
        Launch = launch;
        DoorOpen = doorOpen;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
