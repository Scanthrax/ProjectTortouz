using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearFade : MonoBehaviour {

    SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

    }


    private void Update()
    {
        Color col = renderer.color;
        col.a -= 0.02f;
        renderer.color = col;
        transform.position += Vector3.down * 0.01f;

        if(renderer.color.a <= 0f)
        {
            Destroy(this);
        }
    }
}
