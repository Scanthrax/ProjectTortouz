using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public GameObject Credits;
    public Transform endPoint;

    public float speed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Move credits towards end point
        Credits.transform.position = Vector3.MoveTowards(Credits.transform.position, endPoint.position, Time.deltaTime*speed);
	}
}
