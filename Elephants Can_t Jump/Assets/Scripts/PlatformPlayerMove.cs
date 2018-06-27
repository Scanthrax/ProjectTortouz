using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerMove : MonoBehaviour {
    public Transform SaveParent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Platform")
        {
            SaveParent = this.transform.parent;
            Debug.Log("parentchange");
            transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            Debug.Log("parentchange");
            transform.parent = SaveParent;
        }
    }
}
