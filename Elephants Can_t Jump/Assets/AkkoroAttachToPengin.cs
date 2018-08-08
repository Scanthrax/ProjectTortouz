using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkkoroAttachToPengin : MonoBehaviour {

    public KeyCode key;
    public GameObject Pengin;
    public GameObject AkkoroAndPengin;


    private void OnEnable()
    {
        GetComponent<PlayerMovement>().movement = Utility.Movement.Airborne;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (Input.GetKeyDown(key))
            {
                Pengin.SetActive(false);
                gameObject.SetActive(false);
                AkkoroAndPengin.SetActive(true);
                AkkoroAndPengin.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                AkkoroAndPengin.transform.position = transform.position;
                AkkoroAndPengin.GetComponent<PlayerMovement>().room = GetComponent<PlayerMovement>().room;
                
            }
        }
    }
}
