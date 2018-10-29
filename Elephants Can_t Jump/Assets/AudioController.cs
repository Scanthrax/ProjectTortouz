using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource openDoor;
    public AudioSource launch;

    public static AudioSource OpenDoor;
    public static  AudioSource Launch;

    private void Awake()
    {
        OpenDoor = openDoor;
        Launch = launch;
    }

}
