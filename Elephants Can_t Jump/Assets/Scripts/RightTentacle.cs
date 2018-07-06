using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTentacle : MonoBehaviour {

    private void Awake()
    {
        transform.parent.parent.GetComponent<PlayerMovement>().rightTentacle.rot = transform.parent;
        transform.parent.parent.GetComponent<PlayerMovement>().rightTentacle.rend = GetComponent<SpriteRenderer>();
    }
}
