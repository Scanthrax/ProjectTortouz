using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearFade : MonoBehaviour {

    SpriteRenderer renderer;
    Rigidbody2D rb;


    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * 100f);
    }


    private void Update()
    {
        Color col = renderer.color;
        col.a -= 0.04f;
        renderer.color = col;
        //transform.position += Vector3.down * 0.01f;
        transform.localScale = new Vector3(Random.Range(0.8f, 2.2f), Random.Range(0.8f, 2.2f));
        if(renderer.color.a <= 0f)
        {
            Destroy(this);
        }
    }
}
