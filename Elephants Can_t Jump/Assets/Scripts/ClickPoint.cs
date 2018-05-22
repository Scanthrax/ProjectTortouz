using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class ClickPoint : MonoBehaviour {

    Camera cam;

    public delegate void TentacleClick(Tentacle tent, Vector2 aim);
    public static event TentacleClick notifyTentacle;

    void Start()
    {
        cam = Camera.main;
    }

    void Update ()
    {
        Vector2 screenPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown((int)Tentacle.Left))
        {
            notifyTentacle(Tentacle.Left, screenPoint);
        }
        if (Input.GetMouseButtonDown((int)Tentacle.Right))
        {
            notifyTentacle(Tentacle.Right, screenPoint);
        }
    }
}
