using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Controller : MonoBehaviour
{

    #region Delegates
    public delegate void AttachCharacters(Controlling control);
    public static event AttachCharacters attachCharacters;
    #endregion

    public GameObject[] units;
    public static bool changeChar = false;

    private void Start()
    {
        attachCharacters += switchUnits;
    }

    private void LateUpdate()
    {
        if(changeChar)
        {
            switch(Variables.controlling)
            {
                case Controlling.Akkoro:
                    attachCharacters(Controlling.Both);
                    Variables.controlling = Controlling.Both;
                    break;
                case Controlling.Both:
                    attachCharacters(Controlling.Akkoro);
                    Variables.controlling = Controlling.Akkoro;
                    break;
            }
            changeChar = false;
        }
    }

    void switchUnits(Controlling control)
    {
        foreach(GameObject unit in units)
            unit.SetActive(!unit.activeSelf);
        switch(control)
        {
            case Controlling.Both:
                // transitioning to Akkoro + Pengin gameobject; place gameobject where Pengin was
                units[2].transform.position = units[1].transform.position;
                // flip in the appropriate direction
                units[2].gameObject.GetComponent<SpriteRenderer>().flipX = units[1].gameObject.GetComponent<SpriteRenderer>().flipX;
                break;
            case Controlling.Akkoro:
                // transitioning to seperate gameobjects; place gameobjects where Akkoro + Pengin was
                units[1].transform.position = units[2].transform.position;
                // flip in the appropriate direction
                units[1].gameObject.GetComponent<SpriteRenderer>().flipX = units[2].gameObject.GetComponent<SpriteRenderer>().flipX;
                units[0].transform.position = units[2].transform.position;
                break;
            default:
                print("Shouldn't be here!");
                break;
        }
    }
}
