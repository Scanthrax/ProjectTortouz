using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRegister : MonoBehaviour
{
    private Coroutine lastRoutine;

    private GameObject Player;
    public GameObject Light;
    private bool start;
    public float interval;
    public bool shoot;


    // Use this for initialization
    void Start ()
    {
        start = false;
        shoot = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            lastRoutine = StartCoroutine(cntdwn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
