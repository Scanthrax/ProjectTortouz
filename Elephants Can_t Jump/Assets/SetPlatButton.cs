using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetPlatButton : MonoBehaviour {

    public Color color;
    public SpriteRenderer[] renderers;


    // Update is called once per frame
    void Update () {
        print("SHOULD ONLY BE IN EDITOR MODE");

        foreach(SpriteRenderer rend in renderers)
        {
            rend.color = color;
        }
	}

    
}
