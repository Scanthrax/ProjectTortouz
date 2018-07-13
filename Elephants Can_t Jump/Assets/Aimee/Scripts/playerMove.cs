using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public int moveSpeed;
    public GameObject prefabAkkoro;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Move left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector2(-moveSpeed, 0) * Time.deltaTime);
        }

        //Move right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
        }

        //Jumping
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector2(0, (moveSpeed*2)) * Time.deltaTime);
        }

        //Go down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector2(0, -moveSpeed) * Time.deltaTime);
        }

        //Instantiate Akkoro
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefabAkkoro, transform.position, transform.rotation);
        }
    }
}
