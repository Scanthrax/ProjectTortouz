using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGif : MonoBehaviour
{

    Sprite[] frames;
    int framesPerSecond;
    SpriteRenderer renderer;
    AlienCollectable alien;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        alien = GetComponent<AlienCollectable>();
        framesPerSecond = alien.alien.framerate;
        frames = alien.alien.sprites;
    }

    void Update()
    {
        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        renderer.sprite = frames[index];
    }
}
