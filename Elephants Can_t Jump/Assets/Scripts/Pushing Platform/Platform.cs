using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public GameObject Background;
    private GameObject Player;
    private bool shoot;
    private Rigidbody2D RB;
    Vector3 direction;
    public Transform StartP;
    public Transform EndP;

    public float force;

    //Lerp
    public float ForwardSpeed;
    public float BackwardSpeed;
    private float time1;
    private float time2;

    private bool isAtStart;
    private bool isAtEnd;
	// Use this for initialization
	void Start ()
    {
        isAtStart = true;
        RB = this.GetComponent<Rigidbody2D>();
        shoot = false;
        direction = -(this.transform.right).normalized;
	}
	
	// Update is called once per frame
	void Update ()
    {
        shoot = Background.GetComponent<BackgroundRegister>().shoot;
        if (shoot)
        {
            if (isAtStart)
            {
                Debug.Log("MovingtoEnd");
                this.transform.position = Vector3.MoveTowards(this.transform.position, EndP.position, ForwardSpeed);
                if (this.transform.position == EndP.position)
                {
                    isAtStart = false;
                    isAtEnd = true;
                }
            }
            if (isAtEnd)
            {
                Debug.Log("MovingtoStart");
                this.transform.position = Vector3.MoveTowards(this.transform.position, StartP.position, BackwardSpeed);
                if (this.transform.position == StartP.position)
                {
                    isAtEnd = false;
                    isAtStart = true;
                    Background.GetComponent<BackgroundRegister>().shoot = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shoot)
        {
            Player = collision.transform.gameObject;
            if (collision.transform.CompareTag("Player"))
            {
                launch();
                Debug.Log("Collided");
            }
        }
    }

    void launch()
    {
        Debug.Log("FLY!");
        Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x, 0.5f) * force);
    }
}
