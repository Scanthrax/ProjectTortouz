using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearFade : MonoBehaviour {

    SpriteRenderer renderer;
    Rigidbody2D rb;
    Color newCol;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))*1000f);

        float temp = Random.Range(0.9f, 2.1f);
        transform.localScale = new Vector3(temp, temp);
        newCol = renderer.color;
    }


    private void Update()
    {
        newCol.a -= 0.04f;
        renderer.color = newCol;
        if(renderer.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
