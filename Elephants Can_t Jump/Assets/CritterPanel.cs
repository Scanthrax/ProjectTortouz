using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CritterPanel : MonoBehaviour
{
    public AlienObjects critter;
    public Image rend;
    public GameObject black;

    private void OnValidate()
    {
        if (critter != null)
        {
            rend.sprite = critter.sprites[0];
        }
    }

    private void Start()
    {
        if (SaveController.alienCollectables.ContainsKey(critter.name))
        {
            black.SetActive(!SaveController.alienCollectables[critter.name]);
        }
        else
        {
            black.SetActive(true);
        }
    }
}
