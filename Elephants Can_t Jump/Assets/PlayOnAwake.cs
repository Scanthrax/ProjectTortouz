using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnAwake : MonoBehaviour
{
    private void Start()
    {
        MusicManager.instance.PlaySong(Music.Barn);
        StartCoroutine(Fade.instance.FadeIn(2f));
        
    }

}
