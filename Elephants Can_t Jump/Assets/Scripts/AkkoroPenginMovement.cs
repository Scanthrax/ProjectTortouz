using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AkkoroPenginMovement : MonoBehaviour {

    // move left / right
    int left, right;
    // bools for determining grounding
    public bool[] raycastGrounding = new bool[4];
    // raycasting
    RaycastHit2D hitBR, hitBL, hitLeft, hitRight;
    // wall layer
    int walls;
    // speed
    float speed = 8f, hor;

    SpriteRenderer rend;

    Animator anim;

    public KeyCode launchButton;

    public Transform aimLaunch;

    Camera cam;

	void Start ()
    {
        anim = GetComponent<Animator>();
        walls = LayerMask.NameToLayer("Walls");
        Physics2D.queriesStartInColliders = false;
        rend = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void DetermineGrounding(RaycastHit2D hit, Grounding ground)
    {
        if (hit.collider != null && hit.collider.gameObject.layer == walls)
        {
            raycastGrounding[(int)ground] = true;
        }
        else
        {
            raycastGrounding[(int)ground] = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        #region Initialize raycasts
        hitLeft = Physics2D.Raycast(transform.position, new Vector2(-1,0), 2.3f);
        hitRight = Physics2D.Raycast(transform.position, new Vector2(1,0), 2.3f);
        hitBL = Physics2D.Raycast(transform.position + new Vector3(3f ,0f ,0f), new Vector2(0,-1),2.2f);
        hitBR = Physics2D.Raycast(transform.position + new Vector3(-3f, 0f, 0f), new Vector2(0,-1),2.2f);
        #endregion
        #region Determine grounding
        DetermineGrounding(hitLeft, 0);
        DetermineGrounding(hitRight, (Grounding)1);
        DetermineGrounding(hitBL, (Grounding)2);
        DetermineGrounding(hitBR, (Grounding)3);
        #endregion

        // Can only move left & right
        left = Input.GetKey(KeyCode.A) ? -1 : 0;
        right = Input.GetKey(KeyCode.D) ? 1 : 0;

        // stop at left wall
        if (raycastGrounding[0])
        {
            left = 0;
        }
        // stop at right wall
        if (raycastGrounding[1])
        {
            right = 0;
        }

        // horizontal movement
        hor = (left + right) * speed * Time.deltaTime;

        #region launch
        if (Input.GetKey(launchButton))
        {
            hor = 0f;
            print("should be preparing for launch!");

            aimLaunch.right = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (Input.GetKey(KeyCode.Q))
            {
                print("Launch!");
                Controller.changeChar = true;
            }
        }
        #endregion

        #region face direction
        if (hor < 0)
        {
            rend.flipX = true;
        }
        else if (hor > 0)
        {
            rend.flipX = false;
        }
        #endregion

        #region set bool in animator
        if (hor == 0)
        {
            // we're not walking
            anim.SetBool("isWalking", false);
        }
        // otherwise we're walking
        else
        {
            anim.SetBool("isWalking", true);
        }
        #endregion

        #region apply horizontal force
        transform.Translate(new Vector2(hor, 0));
        #endregion

        #region detach
        if (Input.GetKeyDown(Variables.detach))
        {
            Controller.changeChar = true;
        }
        #endregion

    }
}
