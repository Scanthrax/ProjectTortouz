using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnAwake : MonoBehaviour
{
    private void Start()
    {


        MusicManager.instance.PlaySong(MusicManager.instance.levelMusic);
        StartCoroutine(Fade.instance.FadeIn(2f));

        Cursor.visible = false;
        
    }

}
