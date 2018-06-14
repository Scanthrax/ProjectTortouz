using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public GameObject[] units;


    private void Start()
    {
        AttachBothCharacters.attachCharacters += switchUnits;
    }

    void switchUnits()
    {
        foreach(GameObject unit in units)
            unit.SetActive(!unit.activeSelf);
    }
}
