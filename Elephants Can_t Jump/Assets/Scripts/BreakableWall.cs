using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakableWall : MonoBehaviour, IBreakable
{
    /// <summary>
    /// Wall health; how many hits it can withstand
    /// </summary>
    public int health;

    public bool isBroken = false;

    public void Break(int damage)
    {
        // reduce wall health
        health -= damage;
        // destroy if health falls below 0
        if(health <= 0)
        {
            gameObject.SetActive(false);
            isBroken = true;

            SoundLibrary.AudioSource[0].clip = SoundLibrary.WallBreak[0];
            SoundLibrary.AudioSource[0].volume = 0.45f;
            SoundLibrary.AudioSource[1].Play();

            SoundLibrary.AudioSource[1].clip = SoundLibrary.CrateBreak;
            SoundLibrary.AudioSource[1].volume = 0.7f;
            SoundLibrary.AudioSource[1].Play();
        }
    }

    private void Start()
    {
        if(isBroken)
            gameObject.SetActive(false);
    }
}
