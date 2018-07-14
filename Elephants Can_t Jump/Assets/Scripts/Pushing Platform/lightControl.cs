using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightControl : MonoBehaviour {
    public Material Gray;
    public Material Red;
    public Material Yellow;
    public Material Green;

    public Material Current;

    // Use this for initialization
    void Start () {
        Current = Gray;
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<MeshRenderer>().material = Current;
    }

    public void TurnRed()
    {
        Current = Red;
    }
    public void TurnYellow()
    {
        Current = Yellow;
    }
    public void TurnGreen()
    {
        Current = Green;
    }
    public void TurnGray()
    {
        Current = Gray;
    }

}
