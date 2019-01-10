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
            rend.sprite = alien.sprites[0];
        }
    }

    private void Start()
    {
        if (alien != null && !SaveController.alienCollectables.ContainsKey(alien.name))
        {
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }
}
