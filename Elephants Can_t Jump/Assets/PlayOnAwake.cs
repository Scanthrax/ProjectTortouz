using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnAwake : MonoBehaviour
{
    private void Start()
    {
        MusicManager.instance.PlaySong(Music.Barn);
    }
}
