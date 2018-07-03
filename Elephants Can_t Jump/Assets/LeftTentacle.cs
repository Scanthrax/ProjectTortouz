using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTentacle : MonoBehaviour
{
    private void Awake()
    {
        transform.parent.parent.GetComponent<PlayerMovement>().leftTentacle.rot = transform.parent;
        transform.parent.parent.GetComponent<PlayerMovement>().leftTentacle.rend = GetComponent<SpriteRenderer>();
    }
}
