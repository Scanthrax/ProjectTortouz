using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BreakableWallEditMode : MonoBehaviour
{
    public BoxCollider2D bc;
    public SpriteRenderer sr;

    void Update ()
    {
        bc.size = sr.size;
	}
}
