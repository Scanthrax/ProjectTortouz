using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public GameObject Background;
    private bool shoot;
    private Rigidbody2D RB;
    Vector3 direction;
    public Transform StartP;
    public Transform EndP;

    //Lerp
    public float ForwardSpeed;
    public float backwardSpeed;
    private float time1;
    private float time2;

    private bool isAtStart;
    private bool isAtEnd;

	// Use this for initialization
	void Start ()
    {
        RB = this.GetComponent<Rigidbody2D>();
        shoot = false;
        direction = -(this.transform.right);
	}
	
	// Update is called once per frame
	void Update ()
    {
        shoot = Background.GetComponent<BackgroundRegister>().shoot;

        if(shoot)
        {
            





        }

        if(this.transform.position == EndP.position)
        {
            Debug.Log("here");
            this.transform.position = EndP.position;
        }
    }

    void addForce()
    {
        //RB.AddForce(direction * speed);
    }

}
