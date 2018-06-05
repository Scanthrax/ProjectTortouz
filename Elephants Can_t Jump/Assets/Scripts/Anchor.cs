using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    [SerializeField]    PlayerMove          playerMove;
                        CircleCollider2D    cc;
    //[Range(0.45f, 2f)]  public float        rad = 2f;

    private void Start()
    {
        cc = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //cc.radius = rad;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerMove.anchor = gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playerMove.anchor = null;
    }
}