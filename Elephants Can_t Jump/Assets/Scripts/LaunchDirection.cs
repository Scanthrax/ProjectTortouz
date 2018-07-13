using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDirection : MonoBehaviour {

    private void Awake()
    {
        transform.parent.GetComponent<PlayerMovement>().launchDir = transform;
    }
}
