using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerMove : MonoBehaviour {

    #region Movement
    [Header("Movement")]
    public float speed;
    public float gravityMult = 0;
    Vector2 gravity = Vector2.down * 9.8f;
    #endregion
    #region Objects & Components
    [Header("Objects & Components")]
    public GameObject anchor = null;
    Rigidbody2D rb;
    #endregion
    #region Grounding
    [Header("Grounding")]
    public Grounding grounding;
    float wallDist = 0.8f;
    bool[] raycastGrounding = new bool[4];
    int left, right, hor, up, down, vert;
    int walls;
    RaycastHit2D hitRight, hitLeft, hitTop, hitBottom;
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
    #endregion
    #region Delegates
    //public delegate void WallClimb(Tentacle side);
    //public static event WallClimb wallClimb;

    


    #endregion
    #region Tentacles
    public float rate = 0.01f;
    float magicNumber = 0.0666f;
    public Tentacle anchorTentacle;
    public Tentacle aimTentacle;
    public Tentacle temp;
    #endregion

    [System.Serializable]
    public struct Tentacle
    {
        public float scale;
        public Tentacles state;
        public float dist;
        public SpriteRenderer rend;
        public Transform rot;
    }

    Vector2 screenPoint;
    public static Vector2? pointOfContact = null;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        walls = LayerMask.NameToLayer("Walls");
        Physics2D.queriesStartInColliders = false;

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
        #region movement
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
        rb.velocity = (Vector2.right * hor * speed) + (Vector2.up * vert * speed) + (gravity * gravityMult);
        #endregion
        TentacleAnchor();
        TentacleAim();
    }


    void TentacleAim()
    {
        if(pointOfContact != null)
        {
            aimTentacle.dist = Vector2.Distance(pointOfContact.Value, transform.position);
        }
        switch(aimTentacle.state)
        {
            case Tentacles.None:
                if(anchorTentacle.state == Tentacles.Anchored && Input.GetMouseButtonDown(0))
                {
                    print("I am launching my aiming tentacle!");
                    aimTentacle.state = Tentacles.Expanding;
                    screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;
            case Tentacles.Expanding:
                ExpandAim();
                break;
            case Tentacles.Anchored:
                // rotate the anchor
                aimTentacle.rot.right = new Vector3(pointOfContact.Value.x, pointOfContact.Value.y, 0f) - transform.position;
                aimTentacle.scale = magicNumber * aimTentacle.dist;
                CalculateTentacleLength(anchorTentacle);
                break;
            case Tentacles.Retracting:
                print("Aim tentacle should be retracting!");
                break;
        }
        if(anchorTentacle.state == Tentacles.Retracting)
        {
            aimTentacle.state = Tentacles.Retracting;
        }
    }
 
    void TentacleAnchor()
    {
        switch(anchorTentacle.state)
        {
            case Tentacles.Expanding:
                ExpandAnchor();
                #region set anchor
                // the tentacle is now anchored
                if (anchorTentacle.scale >= anchorTentacle.dist * magicNumber)
                {
                    anchorTentacle.state = Tentacles.Anchored;
                }
                #endregion
                break;
            case Tentacles.Anchored:
                AnchorAnchor();
                break;
            case Tentacles.Retracting:
                RetractAnchor(anchorTentacle);
                break;
            case Tentacles.None:
                #region trigger anchor tentacle expansion
                if (anchor != null && Input.GetKeyDown(KeyCode.Space))
                {
                    print("I want to latch my tentacle at the anchor!");
                    
                    anchorTentacle.state = Tentacles.Expanding;
                }
                #endregion
                break;
        }
    }



    void RetractAnchor(Tentacle tent)
    {
        print("I should be retracting " + tent.ToString());

        // retract tentacle
        tent.scale -= rate;

        // the tentacle is done retracting now
        if (tent.scale <= 0f)
        {
            tent.scale = 0f;
            tent.state = Tentacles.None;
        }

        CalculateTentacleLength(tent);
    }
    void ExpandAnchor()
    {
        print("I should be expanding my tentacle!");
        if (anchor != null)
        {
            // rotate the anchor
            anchorTentacle.rot.right = new Vector3(anchor.transform.position.x, anchor.transform.position.y, 0f) - transform.position;
            // find the distance from Akkoro to the anchor point
            anchorTentacle.dist = Vector2.Distance(anchor.transform.position, transform.position);
        }
        else
        {
            anchorTentacle.state = Tentacles.Retracting;
        }
        // expand tentacle
        anchorTentacle.scale += rate;
        // expand the tentacle
        CalculateTentacleLength(anchorTentacle);
    }
    void AnchorAnchor()
    {
        print("I should be anchored right now!");
        if (anchor == null)
        {
            anchorTentacle.state = Tentacles.Retracting;
        }
        else
        {
            // rotate the anchor
            anchorTentacle.rot.right = new Vector3(anchor.transform.position.x, anchor.transform.position.y, 0f) - transform.position;
            // find the distance from Akkoro to the anchor point
            anchorTentacle.dist = Vector2.Distance(anchor.transform.position, transform.position);
        }
        anchorTentacle.scale = magicNumber * anchorTentacle.dist;
        CalculateTentacleLength(anchorTentacle);
    }



    void ExpandAim()
    {
        print("I should be expanding my aiming tentacle!");

        // expand tentacle
        aimTentacle.scale += rate;
        CalculateTentacleLength(aimTentacle);

        // rotate the anchor
        aimTentacle.rot.right = new Vector3(screenPoint.x, screenPoint.y, 0f) - transform.position;


        if(pointOfContact != null)
        {
            print("I've hit a wall with my tentacle!");
            aimTentacle.state = Tentacles.Anchored;
        }

    }




    void CalculateTentacleLength(Tentacle tent)
    {
        tent.rend.transform.localPosition = new Vector3(tent.scale, 0f, 0f);
        tent.rend.size = new Vector2(2 * tent.scale, tent.rend.size.y);
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
