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

    private void Start()
    {
        if (alien != null && !SaveController.alienCollectables[alien.name])
        {
            gameObject.SetActive(true);
        }
        else gameObject.SetActive(false);
    }
}
