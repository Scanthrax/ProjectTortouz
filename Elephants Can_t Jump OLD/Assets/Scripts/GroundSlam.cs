using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : MonoBehaviour {

    [SerializeField] KeyCode keypress;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update ()
    {
		if(Input.GetKeyDown(keypress))
        {
            anim.SetTrigger("Groundslam");
        }
	}
}
