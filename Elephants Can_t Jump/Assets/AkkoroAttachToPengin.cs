using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkkoroAttachToPengin : MonoBehaviour {

    public KeyCode key;
    public GameObject Pengin;
    public GameObject AkkoroAndPengin;


    private void OnEnable()
    {
        
    }

    void Update () {
		if(Input.GetKeyDown(key))
        {
            //Pengin.SetActive(false);
            gameObject.SetActive(false);
            AkkoroAndPengin.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
            AkkoroAndPengin.transform.position = transform.position;
            AkkoroAndPengin.GetComponent<PlayerMovement>().room = GetComponent<PlayerMovement>().room;
            AkkoroAndPengin.SetActive(true);
        }
	}
}
