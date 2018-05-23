using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerMove : MonoBehaviour {

    public float speed;
    Rigidbody2D rb;
    public float wallDist;
    public int walls;
    public BoxCollider2D bc;
    public bool wallClimbing = false;
    public Vector2 gravity = Vector2.down * 9.8f;
    public float gravityMult = 0;
    public bool[] raycastGrounding = new bool[4];
    public Grounding grounding;
    RaycastHit2D hitRight, hitLeft, hitTop, hitBottom;

    public delegate void WallClimb(Tentacle side);
    public static event WallClimb wallClimb;

    int left, right, hor, up, down, vert;

    public GameObject anchor = null;
    public Transform rot;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        walls = LayerMask.NameToLayer("Walls");
        Physics2D.queriesStartInColliders = false;
        gravity = Vector2.down * 9.8f;
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


    void Update ()
    {
        #region raycasting system
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallDist);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDist);
        hitBottom = Physics2D.Raycast(transform.position, Vector2.down, wallDist);
        hitTop = Physics2D.Raycast(transform.position, Vector2.up, wallDist);
        
        DetermineGrounding(hitRight,Grounding.Right);
        DetermineGrounding(hitLeft,Grounding.Left);
        DetermineGrounding(hitTop,Grounding.Top);
        DetermineGrounding(hitBottom,Grounding.Bottom);

        if(raycastGrounding[(int)Grounding.Bottom])
        {
            if(raycastGrounding[(int)Grounding.Right])
            {
                grounding = Grounding.BottomRight;
            }
            else if(raycastGrounding[(int)Grounding.Left])
            {
                grounding = Grounding.BottomLeft;
            }
            else
            {
                grounding = Grounding.Bottom;
            }
        }
        else if(raycastGrounding[(int)Grounding.Top])
        {
            if (raycastGrounding[(int)Grounding.Right])
            {
                grounding = Grounding.TopRight;
            }
            else if (raycastGrounding[(int)Grounding.Left])
            {
                grounding = Grounding.TopLeft;
            }
            else
            {
                grounding = Grounding.Top;
            }
        }
        else if(raycastGrounding[(int)Grounding.Left])
        {
            grounding = Grounding.Left;
        }
        else if(raycastGrounding[(int)Grounding.Right])
        {
            grounding = Grounding.Right;
        }
        else
        {
            grounding = Grounding.None;
        }
        #endregion

        if(raycastGrounding[(int)Grounding.Bottom])
        {
            left = Input.GetKey(KeyCode.A) ? -1 : 0;
            right = Input.GetKey(KeyCode.D) ? 1 : 0;
        }
        if (raycastGrounding[(int)Grounding.Left])
        {
            up = (Input.GetKey(KeyCode.W)) ? 1 : 0;
            down = (Input.GetKey(KeyCode.S)) ? -1 : 0;
        }
        if (raycastGrounding[(int)Grounding.Right])
        {
            up = (Input.GetKey(KeyCode.W)) ? 1 : 0;
            down = (Input.GetKey(KeyCode.S)) ? -1 : 0;
        }
        if (raycastGrounding[(int)Grounding.Top])
        {
            left = (Input.GetKey(KeyCode.D)) ? 1 : 0;
            right = (Input.GetKey(KeyCode.A)) ? -1 : 0;
        }

        if (grounding == Grounding.None) gravityMult = 1f;
        else gravityMult = 0f;

        hor = left + right;
        vert = up + down;
        rb.velocity = (Vector2.right*hor*speed) + (Vector2.up * vert * speed) + (gravity * gravityMult);


        if(anchor != null && Input.GetKeyDown(KeyCode.Space))
        {
            print("I want to latch my tentacle at the anchor!");
            // rotate the anchor
            rot.right = -new Vector3(anchor.transform.position.x, anchor.transform.position.y,0f) + transform.position;
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * wallDist));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * wallDist));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * wallDist));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * wallDist));
    }
}
