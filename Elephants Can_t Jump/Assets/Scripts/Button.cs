using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public bool isPressed;

    private bool press;
    private bool release;
    public Transform StartP;
    public Transform EndP;
    private float time1 = 0f;
    public float speed1 = .5f;
    private float time2 = 0f;
    public float speed2 = .5f;

    // Use this for initialization
    void Start () {
        isPressed = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(press == true)
        {
            time1 += speed1 * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, EndP.position, time1);
            if(this.transform.position == EndP.position)
            {
                time1 = 0f;
                isPressed = true;
                press = false;
            }

        }
        if (release == true)
        {
            time2 += speed2 * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, StartP.position, time2);
            if (this.transform.position == StartP.position)
            {
                time2 = 0f;
                isPressed = false;
                release = false;
            }

        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
       if(other.CompareTag("Player"))
       {
            Debug.Log("Collide");
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isPressed == false)
                {
                    Debug.Log("pressing");
                    press = true;
                }
                else if (isPressed == true)
                {
                    Debug.Log("de-pressing");
                    release = true;
                }
            }
        
       }
    }
}
