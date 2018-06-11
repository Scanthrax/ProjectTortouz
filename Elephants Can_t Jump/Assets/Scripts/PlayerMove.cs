using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerMove : MonoBehaviour
{
    #region Movement
        /// <summary>
        /// Movement speed
        /// </summary>
        [Header("Movement")]
        public float speed = 10f;
        /// <summary>
        /// Scales the intensity of the gravity
        /// </summary>
        public float gravScale = 4f;
        /// <summary>
        /// is Akkoro gripping?
        /// </summary>    
        public bool gripping = true;
    #endregion
    #region Objects & Components
        /// <summary>
        /// The anchor that is currently in range; it is set to null if none are in range
        /// </summary>
        [Header("Objects & Components")]
        public GameObject anchor = null;
        /// <summary>
        /// The rigid body component
        /// </summary>
        Rigidbody2D rb;
        /// <summary>
        /// The rotation of this transform will be used to calculate the launch direction of Akkoro
        /// </summary>
        public Transform launchDir;
        /// <summary>
        /// Box Collider used for attacking
        /// </summary>
        public BoxCollider2D attackCollider;
    #endregion
    #region Grounding
        /// <summary>
        /// Displays which surfaces Akkoro is being grounded to
        /// </summary>
        [Header("Grounding")]
        public Grounding grounding;
        /// <summary>
        /// Range of the raycasts that project from Akkoro to all sides; used to determine grounding range
        /// </summary>
        float wallDist = 0.86f;
        float wallDist2 = 1.2f;
    /// <summary>
    /// Array of booleans used to identify which surfaces Akkoro is grounded to
    /// </summary>
    public bool[] raycastGrounding = new bool[8];
        /// <summary>
        /// Ints used to determine direction
        /// </summary>
        int left, right, up, down;
        /// <summary>
        /// Floats used to translate Akkoro
        /// </summary>
        float hor, vert;
        /// <summary>
        /// Integer value of layer Walls
        /// </summary>
        int walls;
    /// <summary>
    /// Raycasts to pass into the DetermineGrounding function
    /// </summary>
    RaycastHit2D hitRight, hitLeft, hitTop, hitBottom, hitTR, hitTL, hitBR, hitBL;
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
        /// <summary>
        /// the collider on the Aim tentacle that will collide with surfaces
        /// </summary>
        public CircleCollider2D aimTentacleCol;
        /// <summary>
        /// The rate at chich tentacles will expand/contract
        /// </summary>
        public float rate = 0.01f;
        /// <summary>
        /// The "magic number" is multiplied with the tentacle distance in order to assign the appropriate scale for the tentacle (used to expand the tentacle) 
        /// </summary>
        float magicNumber = 0.064f;
        /// <summary>
        /// Maximum range of the tentacles
        /// </summary>
        public float tentacleRange;
        /// <summary>
        /// The Tentacle class creates the two tentacles that Akkoro will use throughout the game
        /// </summary>
        [System.Serializable]
        public struct Tentacle
        {
            /// <summary>
            /// Scale value of the tencale.  It is incremented over time to extend/retract the tentacle
            /// </summary>
            public float scale;
            /// <summary>
        /// Current state of the tentacle
        /// </summary>
            public Tentacles state;
            /// <summary>
        /// Distance between Akkoro's origin & the end of the tentacle
        /// </summary>
            public float dist;
            /// <summary>
            /// Sprite renderer of the tentacle
            /// </summary>
            public SpriteRenderer rend;
            /// <summary>
        /// Rotation of the tentacle arm
        /// </summary>
            public Transform rot;
            /// <summary>
            /// Anchor position of the tentacle; set to null of there is no position
            /// </summary>
            public Vector3? anchorPos;
        }
    #endregion

    // states of launch
    public Launch launchState = Launch.Launching;

    // delay that determines how many frames should pass before Akkoro can be re-grounded
    int launchDelay = 0;


    Vector3 screenPos = Vector2.zero;
    float l = 0f, d = 0f, X = 0f;
    Vector3 startingPos = Vector3.zero;
    float lerp = 0f, impulse = 0f;

    public List<Vector3> anchorPositions = new List<Vector3>();
    int anchorLayer;

    bool isMoving = false;
    public int stamina = 100;

    void Start ()
    {
        // assign the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        // assign Wall layer to variable
        walls = LayerMask.NameToLayer("Walls");
        anchorLayer = LayerMask.NameToLayer("Anchor");
        // make sure that raycasts don't detect the colliders that they start in
        Physics2D.queriesStartInColliders = false;
	}

    void Update ()
    {
        #region Order nearby anchorpoints by distance
        Functions.OrderByDistance(anchorPositions, transform.position);
        #endregion
        #region Holding spacebar enables grip
        if (Input.GetKey(KeyCode.Space))
            {
                gripping = true;
            }
            else
            {
                gripping = false;
                stamina++;
            }
        #endregion

        

        #region Disable gravity if gripping & grounded to a wall
        if (gripping && grounding != Grounding.None && stamina > 0) EnableGravity(false);
            else EnableGravity(true);
        #endregion
        #region raycasting system
            #region Set up raycasting for 4 directions
                hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallDist);
                hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDist);
                hitBottom = Physics2D.Raycast(transform.position, Vector2.down, wallDist);
                hitTop = Physics2D.Raycast(transform.position, Vector2.up, wallDist);

                hitTR = Physics2D.Raycast(transform.position, new Vector2(1,1), wallDist2);
                hitBR = Physics2D.Raycast(transform.position, new Vector2(1, -1), wallDist2);
                hitBL = Physics2D.Raycast(transform.position, new Vector2(-1,-1), wallDist2);
                hitTL = Physics2D.Raycast(transform.position, new Vector2(-1,1), wallDist2);
        #endregion
        #region Calculate the groundings for all directions
                DetermineGrounding(hitRight,Grounding.Right);
                DetermineGrounding(hitLeft,Grounding.Left);
                DetermineGrounding(hitTop,Grounding.Top);
                DetermineGrounding(hitBottom,Grounding.Bottom);
                DetermineGrounding(hitTR, Grounding.TopRight);
                DetermineGrounding(hitTL, Grounding.TopLeft);
                DetermineGrounding(hitBL, Grounding.BottomLeft);
                DetermineGrounding(hitBR, Grounding.BottomRight);

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
                else if(raycastGrounding[(int)Grounding.TopLeft] ||
                        raycastGrounding[(int)Grounding.TopRight] ||
                        raycastGrounding[(int)Grounding.BottomLeft] ||
                        raycastGrounding[(int)Grounding.BottomRight])
                {
                    grounding = Grounding.Corner;
                }
                else
                {
                    grounding = Grounding.None;
                }
        #endregion
        #endregion

        #region movement
        // The player will only be able to move on walls when gripping
        if (gripping && grounding != Grounding.None && stamina > 0)
        {
            up = Input.GetKey(KeyCode.W) ? 1 : 0;
            down = Input.GetKey(KeyCode.S) ? -1 : 0;
            left = Input.GetKey(KeyCode.A) ? -1 : 0;
            right = Input.GetKey(KeyCode.D) ? 1 : 0;
        }
        else
        {
            up = 0;
            down = 0;
            left = Input.GetKey(KeyCode.A) ? -1 : 0;
            right = Input.GetKey(KeyCode.D) ? 1 : 0;
            
        }

        if(raycastGrounding[(int)Grounding.Left])
        {
            left = 0;
        }
        if (raycastGrounding[(int)Grounding.Right])
        {
            right = 0;
        }
        if (raycastGrounding[(int)Grounding.Top])
        {
            up = 0;
        }
        if (raycastGrounding[(int)Grounding.Bottom])
        {
            down = 0;
            if(!raycastGrounding[(int)Grounding.Right] && !raycastGrounding[(int)Grounding.Left])
            {
                up = 0;
            }
        }




        // horizontal movement with A & D
        hor = (left + right) * speed * Time.deltaTime;
            // vertical movement with W & S
            vert = (up + down) * speed * Time.deltaTime;


        if (hor < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            // flip the horizontal direction, since the gameobject has been rotated 180 degrees
            hor = -hor;
        }
        else if (hor > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        if (hor != 0 || vert != 0)
        {
            isMoving = true;
        }
        else isMoving = false;

        // add force to Akkoro
        transform.Translate(new Vector2(hor, vert));
        #endregion

        #region Stickiness
        if(grounding != Grounding.None && gripping && isMoving)
        {
            print("I should be draining stamina!");
            stamina -= 1;
        }

        #endregion

        TentacleAnchor();
        TentacleAim();

        #region Calculate launch direction
        // if both tentacles are anchored, set the launch direction
        if (aimTentacle.state == Tentacles.Anchored && anchorTentacle.state == Tentacles.Anchored)
        {
            // Visualize launch direction through gameobject transform
            launchDir.right = CalculateLaunchDirection();
        }
        #endregion

        Launching();

        if (Input.GetKeyDown(KeyCode.F))
        {
            attackCollider.enabled = true;
        }
        else attackCollider.enabled = false;

        stamina = Mathf.Clamp(stamina, 0, 100);
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
                    aimTentacle.scale += rate;
                    UpdateTentacleLength(aimTentacle);

                    // rotate anchor
                    aimTentacle.rot.right = screenPos - transform.position;
                #region  only expand to a certain extent
                    if (aimTentacle.scale / magicNumber > tentacleRange)
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
                aimTentacle.rot.right = aimTentacle.anchorPos.Value - transform.position;
                // expand / contract
                aimTentacle.scale = aimTentacle.dist * magicNumber;
                // adjust length
                UpdateTentacleLength(aimTentacle);

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
                aimTentacle = RetractTentacle(aimTentacle);
                break;
                #endregion
        }

        #region calculate tentacle distance
        if(aimTentacle.anchorPos.HasValue)
            aimTentacle.dist = Vector2.Distance(aimTentacle.anchorPos.Value, transform.position);
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
            case Tentacles.None:
                #region Expand the Anchor tentacle when in range of anchor & spacebar is pressed
                if (anchorPositions.Count != 0 && Input.GetKeyDown(KeyCode.E))
                {
                    anchorTentacle.state = Tentacles.Expanding;
                    anchorTentacle.anchorPos = anchorPositions[0];
                }
                #endregion
                break;
            case Tentacles.Expanding:
                #region retract & break if connection is broken
                if (anchorPositions.Count == 0 || anchorTentacle.anchorPos == null)
                {
                    print("Retracting & breaking connection");
                    anchorTentacle.state = Tentacles.Retracting;
                    anchorTentacle.anchorPos = null;
                    break;
                    
                }
                #endregion
                #region calculate distance
                anchorTentacle.rot.right = anchorTentacle.anchorPos.Value - transform.position;
                // find the distance from Akkoro to the anchor point
                anchorTentacle.dist = Vector2.Distance(anchorTentacle.anchorPos.Value, transform.position);
                #endregion
                #region Update Tentacle length
                anchorTentacle.scale += rate;
                UpdateTentacleLength(anchorTentacle);
                #endregion
                #region Set anchor when full distance to anchor is reached
                if (anchorTentacle.scale >= anchorTentacle.dist * magicNumber)
                {
                    anchorTentacle.scale = anchorTentacle.dist * magicNumber;
                    // set state to Anchored
                    anchorTentacle.state = Tentacles.Anchored;
                }
                #endregion
                break;
            case Tentacles.Anchored:
                #region Press E to retract
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        anchorTentacle.state = Tentacles.Retracting;
                        anchorTentacle.anchorPos = null;
                        break;
                    }
                #endregion
                #region Rotate the anchor
                    anchorTentacle.rot.right = anchorTentacle.anchorPos.Value - transform.position;
                #endregion
                #region Adjust tentacle based on distance
                    // find the distance from Akkoro to the anchor point
                    anchorTentacle.dist = Vector2.Distance(anchorTentacle.anchorPos.Value, transform.position);
                    // adjust scale based on distance
                    anchorTentacle.scale = magicNumber * anchorTentacle.dist;
                    UpdateTentacleLength(anchorTentacle);
                #endregion
                #region Keep Akkoro clamped between the range of the tentacles
                    if (anchorTentacle.dist >= tentacleRange)
                    {
                        // find the point on the edge of the radius
                        Vector2 circlePoint = CirclePoint(anchorTentacle.anchorPos.Value, tentacleRange, (Vector2.SignedAngle(Vector2.right, anchorTentacle.rot.right * -1)));
                    // Only adjust the x value of the transform when on ceiling or ground
                    if (grounding == Grounding.Bottom || grounding == Grounding.Top)
                        transform.position = new Vector2(circlePoint.x, transform.position.y);
                    // Only adjust the y value of the transform when on walls
                    else if (grounding == Grounding.Left || grounding == Grounding.Right)
                        transform.position = new Vector2(transform.position.x, circlePoint.y);
                    //Adjust both x & y during any other case
                        else
                            transform.position = new Vector2(circlePoint.x, circlePoint.y);
                    }
                    break;
                #endregion
            case Tentacles.Retracting:
                #region retract Anchor tentacle
                anchorTentacle = RetractTentacle(anchorTentacle);
                break;
            #endregion

        }
        #region calculate tentacle distance
        if(anchorTentacle.anchorPos.HasValue)
            anchorTentacle.dist = Vector2.Distance(anchorTentacle.anchorPos.Value, transform.position);
        #endregion
    }

    void Launching()
    {
        switch (launchState)
        {
            case Launch.Grounded:
                #region Launch if the player presses Q
                    if (Input.GetKeyDown(KeyCode.Q) && aimTentacle.state == Tentacles.Anchored)
                    {
                        // delay sticking back to the surface you just launched from
                        launchDelay = 0;
                        // the player is now being launched
                        launchState = Launch.Contracting;
                        // save impulse information
                        impulse = SpringCalc();
                        // set starting position to set up Lerp
                        startingPos = transform.position;
                        // reset lerp
                        lerp = 0f;
                    }
                    break;
                #endregion
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
                    launchState = Launch.Launching;
                    // launch between both tentacles
                    rb.AddForce(launchDir.right * impulse);
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

    /// <summary>
    /// Retracts the tentacle that is sent into the function
    /// </summary>
    /// <param name="tent"></param>
    /// <returns></returns>
    Tentacle RetractTentacle(Tentacle tent)
    {
        #region Retract tentacle
        tent.scale -= rate;
        #endregion
        // the tentacle is done retracting now
        if (tent.scale <= 0f)
        {
            tent.scale = 0f;
            tent.state = Tentacles.None;
            tent.dist = 0f;
        }
        UpdateTentacleLength(tent);
        return tent;
    }

    /// <summary>
    /// Updates the length of the tentacle based on the scale of the tentacle provided
    /// </summary>
    /// <param name="tent"></param>
    void UpdateTentacleLength(Tentacle tent)
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

    /// <summary>
    /// Spring Equation
    /// </summary>
    /// <returns></returns>
    float SpringCalc()
    {
        if(!aimTentacle.anchorPos.HasValue || !anchorTentacle.anchorPos.HasValue)
        {
            print("NULL VALUE in spring calculations");
            return 0f;
        }
        // k = spring constant
        float k = 650000f;
        // a & b = anchor of aim tentacle
        float a = aimTentacle.anchorPos.Value.x;
        float b = aimTentacle.anchorPos.Value.y;
        // x0 & y0 = Akkoro's position
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        // x1 & y1 = anchor of anchor tentacle
        float x1 = anchorTentacle.anchorPos.Value.x;
        float y1 = anchorTentacle.anchorPos.Value.y;
        // x & y are variables
        float x = Mathf.Abs((a * (x0 - x1)) + (b * (y0 - y1)));
        float y = Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
        // l = distance of anchor tentacle
        l = anchorTentacle.dist;
        // d = x / yis a variable of x / y
        d = x / y;
        // X gets stored for later use
        X = Mathf.Sqrt(Mathf.Pow(l, 2) - Mathf.Pow(d, 2));
        // this is the final result
        return Mathf.Sqrt(k * (Mathf.Pow(l, 2) - Mathf.Pow(d, 2)));
    }

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
    /// Finds the normalized launch direction between the Aim & Anchor tentacles
    /// </summary>
    Vector2 CalculateLaunchDirection()
    {
        Vector2 aimVector = aimTentacle.rot.right;
        Vector2 anchorVector = anchorTentacle.rot.right;
        return (aimVector + anchorVector).normalized;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == anchorLayer)
        {
            if(!anchorPositions.Contains(collision.gameObject.transform.position))
                anchorPositions.Add(collision.gameObject.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == anchorLayer)
        {
            if (anchorPositions.Contains(collision.gameObject.transform.position))
                anchorPositions.Remove(collision.gameObject.transform.position);
        }
    }




    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * wallDist));

        //Gizmos.DrawLine(transform.position, transform.position + (new Vector3(1, 1, 0) * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (new Vector3(-1, 1, 0) * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (new Vector3(-1, -1, 0) * wallDist));
        //Gizmos.DrawLine(transform.position, transform.position + (new Vector3(1, -1, 0) * wallDist));
    }
}