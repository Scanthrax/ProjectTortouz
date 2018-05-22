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
    public Vector2 gravity;

    public delegate void WallClimb(Tentacle side);
    public static event WallClimb wallClimb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        walls = LayerMask.NameToLayer("Walls");
        Physics2D.queriesStartInColliders = false;
        gravity = Vector2.down * 9.8f;
        wallClimb += Climb;
	}
	
    void Climb(Tentacle side)
    {
        wallClimbing = true;

        int up = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)) ? 1 : 0;
        int down = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) ? -1 : 0;
        int vert = up + down;
        rb.velocity = (Vector2.up * vert * speed);
    }

	void Update ()
    {
        wallClimbing = false;
        if(Functions.GripButton())
        {
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallDist);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDist);
            if (hitRight.collider != null)
            {
                if (hitRight.collider.gameObject.layer == walls)
                {
                    print("I am gripping Right!");
                    wallClimb(Tentacle.Right);
                }
            }
            else if (hitLeft.collider != null)
            {
                if (hitLeft.collider.gameObject.layer == walls)
                {
                    print("I am gripping Left!");
                    wallClimb(Tentacle.Left);
                }
            }

        }




        
        int left = Input.GetKey(KeyCode.A) ? -1 : 0;
        int right = Input.GetKey(KeyCode.D) ? 1 : 0;

        int hor = left + right;

        if(!wallClimbing) rb.velocity = (Vector2.right*hor*speed + gravity);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * wallDist));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * wallDist));
    }
}
