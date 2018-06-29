using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Controller : MonoBehaviour
{

    #region Delegates
    public delegate void AttachCharacters(Controlling control, Vector3 launch);
    public static event AttachCharacters attachCharacters;
    #endregion

    public GameObject[] publicUnits;
    public static GameObject[] units;
    //public static bool changeChar = false;

    private void Start()
    {
        attachCharacters += switchUnits;
        units = publicUnits;
    }

    //private void LateUpdate()
    //{
    //    if(changeChar)
    //    {
    //        switch(Variables.controlling)
    //        {
    //            case Controlling.Akkoro:
    //                attachCharacters(Controlling.Both);
    //                Variables.controlling = Controlling.Both;
    //                break;
    //            case Controlling.Both:
    //                attachCharacters(Controlling.Akkoro);
    //                Variables.controlling = Controlling.Akkoro;
    //                break;
    //        }
    //        changeChar = false;
    //    }
    //}

    public static void switchUnits(Controlling control, Vector3 launch)
    {
        switch(control)
        {
            case Controlling.Both:

                units[2].SetActive(true);

                // transitioning to Akkoro + Pengin gameobject; place gameobject where Pengin was
                units[2].transform.position = units[1].transform.position;
                // flip in the appropriate direction
                units[2].gameObject.GetComponent<SpriteRenderer>().flipX = units[0].gameObject.GetComponent<SpriteRenderer>().flipX;

                units[0].SetActive(false);
                units[1].SetActive(false);
                break;
            case Controlling.Akkoro:

                units[0].SetActive(true);
                units[1].SetActive(true);

                // transitioning to seperate gameobjects; place gameobjects where Akkoro + Pengin was
                units[1].transform.position = units[2].transform.position;
                // flip in the appropriate direction
                units[0].gameObject.GetComponent<SpriteRenderer>().flipX = units[2].gameObject.GetComponent<SpriteRenderer>().flipX;
                units[0].transform.position = units[2].transform.position;

                if(launch != Vector3.zero) units[1].GetComponent<Rigidbody2D>().AddForce(launch);

                units[2].SetActive(false);

                break;
            default:
                print("Shouldn't be here!");
                break;
        }
    }
}
