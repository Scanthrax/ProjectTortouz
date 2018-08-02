using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class GroundSlam : MonoBehaviour {

    public KeyCode key;
    PlayerMovement pm;
    Animator anim;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(key) && !pm.action && pm.movement == Movement.Ground)
        {
            anim.SetTrigger("GroundSlam");
        }
    }
}
