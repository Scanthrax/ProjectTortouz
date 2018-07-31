using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public GameObject Background; //shoot variable
    private GameObject Player; //player to be pushed
    private bool shoot; //variable from background
    Vector3 direction; //direction of launch
    public float force; //force of launch
    
    //platform movement
    public Transform StartP; //Start point of platform
    public Transform EndP; //End point of platform
    public float ForwardSpeed; //speed of launch
    public float BackwardSpeed; //speed of retraction
    private bool isAtStart;
    private bool isAtEnd;
    private bool movingBack;

	// Use this for initialization
	void Start ()
    {
        movingBack = false;
        isAtStart = true;
        shoot = false;
        direction = -(this.transform.right).normalized;
	}
	
	// Update is called once per frame
	void Update ()
    {
        shoot = Background.GetComponent<BackgroundRegister>().shoot;
        if (shoot)
        {
            //move forward
            if (isAtStart)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, EndP.position, ForwardSpeed);
                if (this.transform.position == EndP.position)
                {
                    isAtStart = false;
                    isAtEnd = true;
                }
            }
            //move back
            if (isAtEnd)
            {
                movingBack = true;
                this.transform.position = Vector3.MoveTowards(this.transform.position, StartP.position, BackwardSpeed);
                if (this.transform.position == StartP.position)
                {
                    movingBack = false;
                    isAtEnd = false;
                    isAtStart = true;
                    Background.GetComponent<BackgroundRegister>().shoot = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shoot && !movingBack)
        {
            if (collision.transform.CompareTag("Player"))
            {
                Player = collision.transform.gameObject; //store player
                launch(); //launch player
            }
        }
    }

    //method to launch player
    void launch()
    {
        Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x, 0.5f) * force);
    }
}
