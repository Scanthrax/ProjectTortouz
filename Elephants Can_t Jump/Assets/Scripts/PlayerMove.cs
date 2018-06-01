using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerMove : MonoBehaviour
{
    #region Movement
        [Header("Movement")]
        // movement speed
        public float speed = 10f;
        // scales the intensity of the gravity
        public float gravScale = 4f;
        // is Akkoro gripping?
        public bool gripping = true;
    #endregion
    #region Objects & Components
    [Header("Objects & Components")]
        // anchor that is currently in range; it is set to null if none are in range
        public GameObject anchor = null;
        // rigid body component
        Rigidbody2D rb;
        // The rotation of this transform will be used to calculate the launch direction of Akkoro
        public Transform launchDir;
    #endregion
    #region Grounding
    [Header("Grounding")]
        // enum used to display which surfaces Akkoro is being grounded to
        public Grounding grounding;
        // distance from walls; used to determine grounding range
        float wallDist = 0.8f;
        // array of booleans used to identify which surfaces Akkoro is grounded to
        bool[] raycastGrounding = new bool[4];
        // integers used to determine vertical & horizontal movement
        int left, right, hor, up, down, vert;
        // integer value of layer Walls
        int walls;
        // Raycasts to pass into the DetermineGrounding function
        RaycastHit2D hitRight, hitLeft, hitTop, hitBottom;
        /// <summary>
        /// This function is used to calculate the grounding from the raycast. The RaycastHit param is the raycast to be checked; the Grounding param assigns which side the hit should be checking for.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="ground"></param>
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
        /// <summary>
        /// Tentacle that will latch onto the anchorpoints provided in the levels
        /// </summary>
        [Header("Tentacles")]
        public Tentacle anchorTentacle;
        /// <summary>
        /// The free-aiming tentacle that will latch onto surfaces
        /// </summary>
        public Tentacle aimTentacle;
        // the collider on the aiming tentacle that will collide with surfaces
        public CircleCollider2D aimTentacleCol;
        // rate at chich tentacles will expand / contract
        public float rate = 0.01f;
        // the "magic number" is multiplied with the tentacle distance in order to assign the appropriate scale for the tentacle (used to expand the tentacle) 
        float magicNumber = 0.064f;
        // range of the tentacles
        public float tentacleRange;
        /// <summary>
        /// The Tentacle class creates the two tentacles that Akkoro will use throughout the game
        /// </summary>
        [System.Serializable]
        public struct Tentacle
        {
            public float scale;
            // current state of the tentacle
            public Tentacles state;
            // length of the tentacle
            public float dist;
            // sprite renderer of the tentacle
            public SpriteRenderer rend;
            // rotation of the tentacle arm
            public Transform rot;
            // anchor position of the tentacle; set to null of there is no position
            public Vector2? anchorPos;
        }
    #endregion

    // states of launch
    public Launch launchState = Launch.Launching;

    // delay that determines how many frames should pass before Akkoro can be re-grounded
    int launchDelay = 0;


    Vector2 screenPos = Vector2.zero;
    float l = 0f, d = 0f, X = 0f;
    Vector3 startingPos = Vector3.zero;
    float lerp = 0f, impulse = 0f;



    void Start ()
    {
        // assign the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        // assign Wall layer to variable
        walls = LayerMask.NameToLayer("Walls");
        // make sure that raycasts don't detect the colliders that they start in
        Physics2D.queriesStartInColliders = false;
	}

    void Update ()
    {
        #region Holding spacebar enables grip
            if (Input.GetKey(KeyCode.Space))
            {
                gripping = true;
            }
            else
            {
                gripping = false;
            }
        #endregion
        #region Disable gravity if gripping & grounded to a wall
            if (gripping && grounding != Grounding.None) EnableGravity(false);
            else EnableGravity(true);
        #endregion

        #region raycasting system
            #region Set up raycasting for 4 directions
                hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallDist);
                hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDist);
                hitBottom = Physics2D.Raycast(transform.position, Vector2.down, wallDist);
                hitTop = Physics2D.Raycast(transform.position, Vector2.up, wallDist);
            #endregion
            #region Calculate the groundings for all directions
                DetermineGrounding(hitRight,Grounding.Right);
                DetermineGrounding(hitLeft,Grounding.Left);
                DetermineGrounding(hitTop,Grounding.Top);
                DetermineGrounding(hitBottom,Grounding.Bottom);
            #endregion
            #region Find out which walls Akkoro is currently grounded to
                if (raycastGrounding[(int)Grounding.Bottom])
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
        #endregion

        #region movement
            // The player will always be able to move on the ground
            if (raycastGrounding[(int)Grounding.Bottom])
            {
                left = Input.GetKey(KeyCode.A) ? -1 : 0;
                right = Input.GetKey(KeyCode.D) ? 1 : 0;
            }
            // The player will only be able to move on walls when gripping
            if (gripping)
            {
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
            }
            // horizontal movement with A & D
            hor = (left + right) * 100;
            // vertical movement with W & S
            vert = (up + down) * 100;
            // add force to Akkoro
            rb.AddForce(new Vector2(hor, vert));
        #endregion

        TentacleAnchor();
        TentacleAim();

        #region Calculate launch direction
        // if both tentacles are anchored, set the launch direction
        if (aimTentacle.state == Tentacles.Anchored && anchorTentacle.state == Tentacles.Anchored)
        {
            // Visualize launch direction through gameobject transform
            launchDir.right = CalculateVector();
        }
        #endregion

        Launching();
    }


    void TentacleAim()
    {
        switch (aimTentacle.state)
        {
            case Tentacles.None:
                #region Expand only when Anchor tentacle is anchored & Mouse button down
                    if(anchorTentacle.state == Tentacles.Anchored && Input.GetMouseButtonDown(0))
                    {
                        // this tentacle will now be expanding
                        aimTentacle.state = Tentacles.Expanding;
                        // reset variable; this way, the tentacle is free from any point on a surface
                        aimTentacle.anchorPos = null;
                        // enable the circle collider at the end of the tentacle
                        aimTentacleCol.enabled = true;
                        // capture the coordinates of the mouse click to aim the tentacle
                        screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                    break;
                #endregion
            case Tentacles.Expanding:
                #region Expand the tentacle
                    // expand tentacle
                    aimTentacle.scale += rate;
                    CalculateTentacleLength(aimTentacle);
                    // rotate the anchor
                    aimTentacle.rot.right = new Vector3(screenPos.x, screenPos.y, 0f) - transform.position;
                #region  only expand to a certain extent
                    if (aimTentacle.dist > tentacleRange)
                    {
                        aimTentacle.state = Tentacles.Retracting;
                    }
                #endregion
                #region Tentacle anchors when there is a point of contact
                    if (aimTentacle.anchorPos.HasValue)
                    {
                        aimTentacle.state = Tentacles.Anchored;
                    }
                    break;
                #endregion
            #endregion
            case Tentacles.Anchored:
                #region Keep the tentacle anchored by expanding / contracting
                // rotate the anchor
                aimTentacle.rot.right = new Vector3(aimTentacle.anchorPos.Value.x, aimTentacle.anchorPos.Value.y, 0f) - transform.position;
                // expand / contract
                aimTentacle.scale = aimTentacle.dist * magicNumber;
                // adjust length
                CalculateTentacleLength(aimTentacle);

                if (aimTentacle.dist >= tentacleRange)
                {
                    Vector2 circlePoint = CirclePoint(aimTentacle.anchorPos.Value, tentacleRange, (Vector2.SignedAngle(Vector2.right, aimTentacle.rot.right * -1)));
                    if (grounding == Grounding.Bottom || grounding == Grounding.Top)
                        transform.position = new Vector2(circlePoint.x, transform.position.y);
                    else if (grounding == Grounding.Left || grounding == Grounding.Right)
                        transform.position = new Vector2(transform.position.x, circlePoint.y);
                    else
                        transform.position = new Vector2(circlePoint.x, circlePoint.y);
                }

                break;
            #endregion
            case Tentacles.Retracting:
                #region Retract the tentacle
                aimTentacle = RetractAnchor(aimTentacle);
                break;
                #endregion
        }
        #region calculate tentacle distance
        if (aimTentacle.anchorPos.HasValue)
        {
            aimTentacle.dist = Vector2.Distance(aimTentacle.anchorPos.Value, transform.position);
        }
        #endregion
        #region Retract this tentacle if the anchor tentacle is retracting
        if (anchorTentacle.state == Tentacles.Retracting)
        {
            aimTentacle.state = Tentacles.Retracting;
        }
        #endregion
    }

    void TentacleAnchor()
    {
        switch(anchorTentacle.state)
        {
            case Tentacles.Expanding:
                #region Expand the anchor
                ExpandAnchor();
                #endregion
                #region Set anchor when full distance to anchor is reached
                if (anchorTentacle.scale >= anchorTentacle.dist * magicNumber)
                {
                    anchorTentacle.scale = anchorTentacle.dist * magicNumber;
                    anchorTentacle.state = Tentacles.Anchored;
                }
                #endregion
                break;
            case Tentacles.Anchored:
                #region Keep the tentacle anchored
                AnchorAnchor();
                break;
            #endregion
            case Tentacles.Retracting:
                #region retract Anchor tentacle
                anchorTentacle = RetractAnchor(anchorTentacle);
                break;
            #endregion
            case Tentacles.None:
                #region Expand the Anchor tentacle when in range of anchor & spacebar is pressed
                if (anchor != null && Input.GetKeyDown(KeyCode.E))
                {
                    anchorTentacle.state = Tentacles.Expanding;
                }
                #endregion
                break;
        }
    }

    void Launching()
    {
        switch (launchState)
        {
            case Launch.Grounded:
                // Launch if the player presses Q
                if (Input.GetKeyDown(KeyCode.Q) && aimTentacle.state == Tentacles.Anchored)
                {
                    // delay sticking back to the surface you just launched from
                    launchDelay = 0;
                    // the player is now being launched
                    launchState = Launch.Contracting;

                    impulse = SpringCalc();

                    // set starting position to set up Lerp
                    startingPos = transform.position;
                    lerp = 0f;
                }
                break;
            case Launch.Contracting:
                // increase lerp over time
                lerp += 0.1f;
                transform.position = Vector2.Lerp(startingPos,startingPos + (launchDir.right * X), lerp);
                if(lerp >= 1f)
                {
                    #region retract both tentacles
                    anchorTentacle.state = Tentacles.Retracting;
                    aimTentacle.state = Tentacles.Retracting;
                    #endregion

                    // launch between both tentacles
                    //rb.AddForce(launchDir.right * impulse);
                    launchState = Launch.Launching;
                }
                break;
            case Launch.Launching:
                launchDelay++;
                if(grounding != Grounding.None && launchDelay > 10)
                {
                    launchState = Launch.Grounded;
                }
                break;
        }
    }


    Tentacle RetractAnchor(Tentacle tent)
    {

        // retract tentacle
        tent.scale -= rate;

        // the tentacle is done retracting now
        if (tent.scale <= 0f)
        {
            tent.scale = 0f;
            tent.state = Tentacles.None;
            tent.dist = 0f;
        }

        CalculateTentacleLength(tent);
        return tent;
    }

    void ExpandAnchor()
    {
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
        #region Press E to retract
        if(Input.GetKeyDown(KeyCode.E))
        {
            anchorTentacle.state = Tentacles.Retracting;
        }
        #endregion
        // rotate the anchor
        anchorTentacle.rot.right = new Vector3(anchor.transform.position.x, anchor.transform.position.y, 0f) - transform.position;
        // find the distance from Akkoro to the anchor point
        anchorTentacle.dist = Vector2.Distance(anchor.transform.position, transform.position);
        anchorTentacle.scale = magicNumber * anchorTentacle.dist;
        CalculateTentacleLength(anchorTentacle);

        
        if(anchorTentacle.dist >= tentacleRange)
        {
            Vector2 circlePoint = CirclePoint(anchor.transform.position, tentacleRange, (Vector2.SignedAngle(Vector2.right, anchorTentacle.rot.right * -1)));
            if(grounding == Grounding.Bottom || grounding == Grounding.Top)
                transform.position = new Vector2(circlePoint.x, transform.position.y);
            else if(grounding == Grounding.Left || grounding == Grounding.Right)
                transform.position = new Vector2(transform.position.x, circlePoint.y);
            else
                transform.position = new Vector2(circlePoint.x, circlePoint.y);
        }
    }

    /// <summary>
    /// Updates the length of the tentacle based on the scale of the tentacle provided
    /// </summary>
    /// <param name="tent"></param>
    void CalculateTentacleLength(Tentacle tent)
    {
        tent.rend.transform.localPosition = new Vector3(tent.scale, 0f, 0f);
        tent.rend.size = new Vector2(2 * tent.scale, tent.rend.size.y);
    }

    /// <summary>
    /// Used to determine when gravity should be enabled or disabled
    /// </summary>
    /// <param name="enable"></param>
    void EnableGravity(bool enable)
    {
        if(enable)  rb.gravityScale = gravScale;
        else        rb.gravityScale = 0f;
    }

    float SpringCalc()
    {
        float k = 1000000f;
        float a = aimTentacle.anchorPos.Value.x;
        float b = aimTentacle.anchorPos.Value.y;
        float x0 = transform.position.x;
        float x1 = anchor.transform.position.x;
        float y0 = transform.position.y;
        float y1 = anchor.transform.position.y;
        float x = Mathf.Abs((a * (x0 - x1)) + (b * (y0 - y1)));
        float y = Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
        l = anchorTentacle.dist;
        d = x / y;
        X = Mathf.Sqrt(Mathf.Pow(l, 2) - Mathf.Pow(d, 2));
        float z = Mathf.Sqrt(k * (Mathf.Pow(l, 2) - Mathf.Pow((x / y), 2f)));
        return z;
    }


    //  x = cx + r* cos(a)
    //  y = cy + r* sin(a)
    //  Where r is the radius, cx, cy the origin, and a the angle.
    //   That's pretty easy to adapt into any language with basic trig functions
    //  Note that most languages will use radians for the angle in trig functions,
    //  so rather than cycling through 0..360 degrees, you're cycling through 0..2PI radians.

    /// <summary>
    /// Calculates a point on the edge of a circle when provided an origin point, radius, and angle.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector2 CirclePoint(Vector2 origin, float radius, float angle)
    {
        float x = origin.x + (radius * Mathf.Cos(angle*Mathf.Deg2Rad));
        float y = origin.y + (radius * Mathf.Sin(angle*Mathf.Deg2Rad));
        return new Vector2(x, y);
    }

    /// <summary>
    /// Finds the direction between the Aim & Anchor tentacles
    /// </summary>
    Vector2 CalculateVector()
    {
        Vector2 aimVector = aimTentacle.rot.right;
        Vector2 anchorVector = anchorTentacle.rot.right;
        return (aimVector + anchorVector).normalized;
    }

    // c = Akkoro's arm length
    // b = dist between anchor points
    // k = spring constant
    // F = K(c^2 - b^2)


    //float SpringCalculations()
    //{
    //    // return if either anchor point becomes null
    //    if(anchor == null || !pointOfContact.HasValue)
    //    {
    //        print("A value for the spring calculations is null!");
    //        return 0f;
    //    }

    //    //float c = aimTentacle.dist;
    //    float c = anchorTentacle.dist;
    //    float b = Vector2.Distance(pointOfContact.Value, anchor.transform.position);
    //    float k = 3f;
    //    return k * (Mathf.Pow(c, 2) - Mathf.Pow(b, 2));
    //}





    //float ArcEquation()
    //{
    //    return (Mathf.Pow(SpringCalc(), 2)* Mathf.Sin(2*Vector2.Angle(Vector2.right,launchDir.right)))/9.8f;
    //}


    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * wallDist));
    }
}