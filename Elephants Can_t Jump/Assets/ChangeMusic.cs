using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{

    public Music music;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MusicManager.instance.SwitchMusic(music);
    }
}
