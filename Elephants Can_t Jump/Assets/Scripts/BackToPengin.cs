using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class BackToPengin : MonoBehaviour
{

    int walls;
    public Transform pengin;
    int buttons;
    public static bool isSlinging;
    public bool goBack;
    float lerp = 0f;


    void Start()
    {
        walls = LayerMask.NameToLayer("Walls");
        buttons = LayerMask.NameToLayer("Buttons");
        //gameObject.SetActive(false);
    }

    void Update()
    {
        #region go back to pengin
        GoBackToPengin(transform.position, pengin.position);
        #endregion
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region check for wall collision (sling)
        if (isSlinging && collision.gameObject.layer == walls)
        {
            print("impact has been made!");
            goBack = true;
            
            isSlinging = false;
        }
        #endregion
    }

    void GoBackToPengin(Vector3 currentPos, Vector3 penginPos)
    {
        if(goBack)
        {
            lerp += 0.05f;
            transform.position = Vector2.Lerp(currentPos, penginPos, lerp);
            if(lerp >= 1f)
            {
                lerp = 0f;
                goBack = false;
                Controller.switchUnits(Controlling.Both, Vector3.zero);
            }
        }
    }
}
