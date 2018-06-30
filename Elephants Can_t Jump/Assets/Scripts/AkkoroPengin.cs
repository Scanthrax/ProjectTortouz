using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AkkoroPengin : MonoBehaviour {



    public KeyCode launchPrep;
    public KeyCode launchButton;
    public KeyCode detach;
    
    public Transform aimLaunch;

    Camera cam;

    public float launchForce;

    bool onEnable;

    #region Delegates
    public delegate void StartSling();
    public static event StartSling startSling;
    #endregion


    void Start ()
    {
        Physics2D.queriesStartInColliders = false;
        cam = Camera.main;
    }

    private void OnEnable()
    {
        onEnable = true;
    }
    // Update is called once per frame
    void Update ()
    {
        #region launch
        if (Input.GetKey(launchPrep))
        {
            print("should be preparing for launch!");

            aimLaunch.right = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            if (Input.GetKeyDown(launchButton))
            {
                Controller.switchUnits(Controlling.Akkoro, aimLaunch.right.normalized * launchForce);
                BackToPengin.isSlinging = true;
            }
        }
        #endregion


        #region detach
        if (Input.GetKeyDown(detach) && !onEnable)
        {
            Controller.switchUnits(Controlling.Akkoro, Vector3.zero);
        }
        #endregion
        onEnable = false;
    }
}
