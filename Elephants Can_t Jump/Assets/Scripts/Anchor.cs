using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    PlayerMove playerMove;

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerMove = collision.GetComponent<PlayerMove>();
        playerMove.anchor = gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playerMove.anchor = null;
    }
}