using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRegister : MonoBehaviour
{
    private Coroutine lastRoutine; //keeps track of coroutine
    public GameObject Light; //Light object
    public float interval; //time between change of lights from red to yellow to green
    public bool shoot; //keeps track of when platform shoots


    // Use this for initialization
    void Start ()
    {
        shoot = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //start countdown when player is on platform
        if(collision.CompareTag("Player"))
        {
            lastRoutine = StartCoroutine(cntdwn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //stops everything if the player exits area
        if (collision.CompareTag("Player"))
        {
            Light.GetComponent<lightControl>().TurnGray();
            StopCoroutine(lastRoutine);
        }
    }

    IEnumerator cntdwn()
    {
        //red
        Light.GetComponent<lightControl>().TurnRed();
        yield return new WaitForSeconds(interval);
        //yellow
        Light.GetComponent<lightControl>().TurnYellow();
        yield return new WaitForSeconds(interval);
        //green
        Light.GetComponent<lightControl>().TurnGreen();
        yield return new WaitForSeconds(interval);
        //shoot
        shoot = true;
    }


}
