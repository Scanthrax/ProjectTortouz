using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkkoroDetach : MonoBehaviour {

    public KeyCode key;
    public GameObject Akkoro;
    public GameObject Pengin;


    private void OnEnable()
    {
        
    }

    void Update ()
    {
		if(Input.GetKeyDown(key) && !PenginSlingAkkoro.prepLaunch)
        {
            print("Should be detaching!");
            gameObject.SetActive(false);
            Akkoro.transform.position = transform.position;
            Akkoro.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
            Akkoro.GetComponent<PlayerMovement>().room = GetComponent<PlayerMovement>().room;
            Akkoro.SetActive(true);
            Pengin.transform.position = transform.position;
            Pengin.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
            Pengin.SetActive(true);
        }
	}
}
