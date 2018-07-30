using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRegister : MonoBehaviour
{
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
            StartCoroutine(cntdwn());
        }
    }

    IEnumerator cntdwn()
    {
        Debug.Log("Red");
        Light.GetComponent<lightControl>().TurnRed();
        yield return new WaitForSeconds(interval);
        Debug.Log("Yellow");
        Light.GetComponent<lightControl>().TurnYellow();
        yield return new WaitForSeconds(interval);
        Debug.Log("Green");
        Light.GetComponent<lightControl>().TurnGreen();
        yield return new WaitForSeconds(interval);
        shoot = true;
    }


}
