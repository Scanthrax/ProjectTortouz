using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement pm = collision.GetComponent<PlayerMovement>();
        if(pm)
        {
            pm.endgame = true;
        }

        StartCoroutine(Fade.instance.FadeOut(3f, "CREDITS"));

    }


}
