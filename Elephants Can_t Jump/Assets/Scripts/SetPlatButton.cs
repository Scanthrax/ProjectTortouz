using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlatButton : MonoBehaviour
{
    public Color color;
    public SpriteRenderer[] renderers;

    void OnValidate ()
    {
        foreach(SpriteRenderer rend in renderers)
        {
            rend.color = color;
        }
	}
}
