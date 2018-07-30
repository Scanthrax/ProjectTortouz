using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCollectable : MonoBehaviour {

    public AlienObjects alien;
    public SpriteRenderer rend;

    private void OnValidate()
    {
        if (alien != null)
        {
            rend.sprite = alien.sprite;
        }
    }
}
