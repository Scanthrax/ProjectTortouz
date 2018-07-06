using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerMovement : MonoBehaviour
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
    public float gravScale = 7f;
    /// <summary>
    /// is Akkoro gripping?
    /// </summary>    
    public bool gripping;
    /// <summary>
    /// Maximum amount of stamina Akkoro has for wallclimbing
    /// </summary>
    public int maxStamina = 120;
    /// <summary>
    /// Stamina is drained while Akkoro is climbing walls
    /// </summary>
    public int stamina;
    /// <summary>
    /// Platform code ||| Stores the parent of the character so it can be returned when the platform is exited.
    /// </summary>
    public Transform storeParent;
    #endregion
    #region Objects & Components
    /// <summary>
    /// The rigid body component for Akkoro
    /// </summary>
    [Header("Objects & Components")]
    Rigidbody2D rb;
    /// <summary>
    /// The rotation of this transform will be used to calculate the launch direction of Akkoro
    /// </summary>
    [HideInInspector]public Transform launchDir;
    /// <summary>
    /// Box Collider used for attacking
    /// </summary>
    public BoxCollider2D attackCollider;
    /// <summary>
    /// Akkoro's sprite renderer
    /// </summary>
    SpriteRenderer rend;
    public Transform pengin;
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
    float wallDistVert = 0.86f;
    float wallDistHor = 0.86f;
    float wallDistDiag = 1.2f;
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
    #region Tentacles
    /// <summary>
    /// The first tentacle that will latch onto the anchorpoints provided in the levels
    /// </summary>
    [Header("Tentacles")]
    public Tentacle leftTentacle = new Tentacle();
    /// <summary>
    /// The second tentacle that will latch onto the anchorpoints provided in the levels
    /// </summary>
    public Tentacle rightTentacle = new Tentacle();

    /// <summary>
    /// the collider on the Aim tentacle that will collide with surfaces
    /// </summary>
    //public CircleCollider2D aimTentacleCol;

    /// <summary>
    /// The rate at chich tentacles will expand/contract
    /// </summary>
    public float rate = 0.02f;
    /// <summary>
    /// The "magic number" is multiplied with the tentacle distance in order to assign the appropriate scale for the tentacle (used to expand the tentacle) 
    /// </summary>
    float magicNumber = 0.064f;
    /// <summary>
    /// Maximum range of the tentacles
    /// </summary>
    public float tentacleRange = 6f;
    /// <summary>
    /// The Tentacle class creates the two tentacles that Akkoro will use throughout the game
    /// </summary>
    [System.Serializable]
    public class Tentacle
    {
        /// <summary>
        /// Scale value of the tencale.  It is incremented over time to extend/retract the tentacle
        /// </summary>
        public float scale;
        /// <summary>
        /// Current state of the tentacle
        /// </summary>
        public Tentacles state = Tentacles.None;
        /// <summary>
        /// Distance between Akkoro's origin & the end of the tentacle
        /// </summary>
        public float dist;
        /// <summary>
        /// Sprite renderer of the tentacle
        /// </summary>
        [HideInInspector]public SpriteRenderer rend;
        /// <summary>
        /// Rotation of the tentacle arm
        /// </summary>
        [HideInInspector]public Transform rot;
        /// <summary>
        /// Anchor position of the tentacle; set to null of there is no position
        /// </summary>
        public Vector3? anchorPos;
        /// <summary>
        /// The keycode that performs actions for this tentacle
        /// </summary>
        public KeyCode key;

        public int mouseButton;
        /// <summary>
        /// Contructor
        /// </summary>
        //public Tentacle()
        //{
        //    scale = 0f;
        //    state = Tentacles.None;
        //    dist = 0f;
        //    rend = null;
        //    rot = null;
        //    anchorPos = null;
        //    key = KeyCode.None;
        //}
    }
    #endregion

    /// <summary>
    /// List of nearby anchor positions; Keeps track of which anchors are available for Akkoro to latch onto
    /// </summary>
    public List<Vector3> anchorPositions = new List<Vector3>();
    /// <summary>
    /// Layer for anchor points
    /// </summary>
    int anchorLayer;


    /// <summary>
    /// Used to determine which character(s) the player has control over
    /// </summary>
    public Controlling control;
    /// <summary>
    /// used in Launch equation
    /// </summary>
    public float launchConst = 580f, launchPow = 1.55f;
    // 580 1.55

    public bool switchTentacles;

    public Sling slingshot;

    public bool isSlinging;
    public bool goBack;
    float lerp = 0f;
    public int buttons;
    Animator anim;
    BoxCollider2D bc;

    void Start()
    {


        // assign the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        // assign Wall layer to variable
        walls = LayerMask.NameToLayer("Walls");
        anchorLayer = LayerMask.NameToLayer("Anchor");
        buttons = LayerMask.NameToLayer("Button");
        // make sure that raycasts don't detect the colliders that they start in
        Physics2D.queriesStartInColliders = false;
        stamina = maxStamina;
        rend = GetComponent<SpriteRenderer>();
        leftTentacle.anchorPos = null;
        rightTentacle.anchorPos = null;
        switchTentacles = false;
        slingshot = Sling.None;
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        wallDistHor = bc.size.x * transform.lossyScale.x * 0.6f;
        wallDistVert = bc.size.y * transform.lossyScale.x * 0.6f;
        wallDistDiag = Vector2.Distance(transform.position, transform.position + (new Vector3(bc.size.x, bc.size.y) * transform.lossyScale.x)) * 1.2f;
        //gameObject.SetActive(false);
    }

    void Update()
    {
        #region Order nearby anchorpoints by distance
        Functions.OrderByDistance(anchorPositions, transform.position);
        #endregion

        #region Holding spacebar enables grip
        if (Input.GetKey(Variables.wallGrip))
        {
            gripping = true;
        }
        else
        {
            gripping = false;
        }
        #endregion

        #region Recover Stamina
        if (raycastGrounding[(int)Grounding.Bottom])
        {
            stamina += 5;
        }
        #endregion

        #region Disable gravity if gripping & grounded to a wall
        if (gripping && grounding != Grounding.None && stamina > 0) EnableGravity(false);
        else EnableGravity(true);
        #endregion

        #region raycasting system
        #region Set up raycasting for 4 directions
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallDistHor);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDistHor);
        hitBottom = Physics2D.Raycast(transform.position, Vector2.down, wallDistVert);
        hitTop = Physics2D.Raycast(transform.position, Vector2.up, wallDistVert);

        
        hitTR = Physics2D.Raycast(transform.position, new Vector2(1, 1), wallDistDiag);
        hitBR = Physics2D.Raycast(transform.position, new Vector2(1, -1), wallDistDiag);
        hitBL = Physics2D.Raycast(transform.position, new Vector2(-1, -1), wallDistDiag);
        hitTL = Physics2D.Raycast(transform.position, new Vector2(-1, 1), wallDistDiag);
        #endregion
        #region Calculate the groundings for all directions
        DetermineGrounding(hitRight, Grounding.Right);
        DetermineGrounding(hitLeft, Grounding.Left);
        DetermineGrounding(hitTop, Grounding.Top);
        DetermineGrounding(hitBottom, Grounding.Bottom);
        DetermineGrounding(hitTR, Grounding.TopRight);
        DetermineGrounding(hitTL, Grounding.TopLeft);
        DetermineGrounding(hitBL, Grounding.BottomLeft);
        DetermineGrounding(hitBR, Grounding.BottomRight);

        #endregion
        #region Find out which walls Akkoro is currently grounded to
        if (raycastGrounding[(int)Grounding.Bottom])
        {
            if (raycastGrounding[(int)Grounding.Right])
            {
                grounding = Grounding.BottomRight;
            }
            else if (raycastGrounding[(int)Grounding.Left])
            {
                grounding = Grounding.BottomLeft;
            }
            else
            {
                grounding = Grounding.Bottom;
            }
        }
        else if (raycastGrounding[(int)Grounding.Top])
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
        else if (raycastGrounding[(int)Grounding.Left])
        {
            grounding = Grounding.Left;
        }
        else if (raycastGrounding[(int)Grounding.Right])
        {
            grounding = Grounding.Right;
        }
        else if (raycastGrounding[(int)Grounding.TopLeft] ||
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

        if (raycastGrounding[(int)Grounding.Left])
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
            if (!raycastGrounding[(int)Grounding.Right] && !raycastGrounding[(int)Grounding.Left])
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
            rend.flipX = true;
        }
        else if (hor > 0)
        {
            rend.flipX = false;
        }

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


        // add force to Akkoro
        transform.Translate(new Vector2(hor, vert));


        if(grounding != Grounding.None && gripping)
        {
            rb.velocity = Vector2.zero;
        }
        #endregion

        #region Stickiness
        if (grounding != Grounding.None && gripping && !raycastGrounding[(int)Grounding.Bottom] && Time.timeScale != 0f)
        {
            stamina--;
        }

        #endregion

        #region Tentacle logic
        TentacleGrab(ref leftTentacle, ref rightTentacle);
        TentacleGrab(ref rightTentacle, ref leftTentacle);

        if(leftTentacle.rot.rotation.z < rightTentacle.rot.rotation.z && switchTentacles)
        {
            SwitchTentacle(ref leftTentacle, ref rightTentacle);

            switchTentacles = false;
        }
        #endregion

        #region Calculate launch direction
        // if both tentacles are anchored, set the launch direction
        if (rightTentacle.state == Tentacles.Anchored && leftTentacle.state == Tentacles.Anchored)
        {
            // Visualize launch direction through gameobject transform
            launchDir.right = CalculateLaunchDirection();
        }
        #endregion

        /*
        #region attack the walls
        if (Input.GetKey(KeyCode.F))
        {
            attackCollider.enabled = true;

            if (hor < 0)
                attackCollider.transform.eulerAngles = new Vector3(0, -180, 0);
            else if (hor > 0) attackCollider.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else attackCollider.enabled = false;
        #endregion
        */

        #region clamp stamina
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        #endregion

        /*
        #region go back to pengin
        BackToPengin(transform.position, pengin.position);
        #endregion
        */
    }

    /// <summary>
    /// The logic that drives a given tentacle to grab anchorpoints; considers the behavior of the other tentacle
    /// </summary>
    /// <param name="thisTentacle">The tentacle that the behavior is being applied to</param>
    /// <param name="otherTentacle">The other tentacle to take into consideration</param>
    void TentacleGrab(ref Tentacle thisTentacle, ref Tentacle otherTentacle)
    {


        switch (thisTentacle.state)
        {
            case Tentacles.None:

                #region set anchor to null
                thisTentacle.anchorPos = null;
                #endregion

                #region Expand the Anchor tentacle when in range of anchor & key is pressed
                if ((Input.GetKey(thisTentacle.key) || Input.GetMouseButton(thisTentacle.mouseButton)) && !AkkoroPengin.prepLaunch)
                {
                    // there is only a single point
                    if (anchorPositions.Count == 1)
                    {
                        // if the other tentacle is not anchored
                        if (otherTentacle.anchorPos.HasValue)
                        {
                            if (otherTentacle.anchorPos != anchorPositions[0])
                            {
                                thisTentacle.anchorPos = anchorPositions[0];
                                thisTentacle.state = Tentacles.Expanding;
                            }
                        }
                        else
                        {
                            thisTentacle.anchorPos = anchorPositions[0];
                            thisTentacle.state = Tentacles.Expanding;
                        }
                    }
                    // there are at least 2 anchorpoints
                    else if (anchorPositions.Count >= 2)
                    {
                        // is the other tentacle attached?
                        if (otherTentacle.anchorPos.HasValue)
                        {
                            if (otherTentacle.anchorPos == anchorPositions[0])
                            {
                                thisTentacle.anchorPos = anchorPositions[1];
                                thisTentacle.state = Tentacles.Expanding;
                            }
                            else
                            {
                                thisTentacle.anchorPos = anchorPositions[0];
                                thisTentacle.state = Tentacles.Expanding;
                            }
                        }
                        // select the second nearest anchorpoint
                        else
                        {
                            thisTentacle.anchorPos = anchorPositions[0];
                            thisTentacle.state = Tentacles.Expanding;
                        }
                    }

                }
                #endregion

                break;
            case Tentacles.Expanding:

                #region retract & break if connection is broken
                if (anchorPositions.Count == 0 || thisTentacle.anchorPos == null)
                {
                    print("Retracting & breaking connection");
                    thisTentacle.state = Tentacles.Retracting;
                    thisTentacle.anchorPos = null;
                    break;
                }
                if(thisTentacle.anchorPos.HasValue)
                {
                    if(Vector2.Distance(transform.position, thisTentacle.anchorPos.Value) > tentacleRange)
                    {
                        print("Retracting & breaking connection");
                        thisTentacle.state = Tentacles.Retracting;
                        thisTentacle.anchorPos = null;
                        break;
                    }
                }
                #endregion

                #region calculate distance
                thisTentacle.rot.right = thisTentacle.anchorPos.Value - transform.position;
                
                #endregion

                #region Update Tentacle length
                thisTentacle.scale += rate;
                
                #endregion

                #region Set anchor when full distance to anchor is reached
                if (thisTentacle.scale >= thisTentacle.dist * magicNumber)
                {
                    thisTentacle.scale = thisTentacle.dist * magicNumber;
                    // set state to Anchored
                    UpdateTentacleLength(thisTentacle);
                    thisTentacle.state = Tentacles.Anchored;

                    #region
                    
                    if (otherTentacle.state == Tentacles.Anchored)
                    {
                        switchTentacles = true;
                    }
                    
                    #endregion
                }
                #endregion
                UpdateTentacleLength(thisTentacle);
                break;
            case Tentacles.Anchored:

                #region Aim at anchor
                thisTentacle.rot.right = thisTentacle.anchorPos.Value - transform.position;
                #endregion

                #region Press key to retract
                if (!Input.GetKey(thisTentacle.key) && !Input.GetMouseButton(thisTentacle.mouseButton))
                {
                    thisTentacle.state = Tentacles.Retracting;
                    thisTentacle.anchorPos = null;
                    break;
                }
                #endregion

                if (Input.GetKey(Variables.launch) && otherTentacle.state == Tentacles.Anchored)
                {
                    #region retract both tentacles
                    thisTentacle.state = Tentacles.Retracting;
                    otherTentacle.state = Tentacles.Retracting;
                    #endregion
                    rb.AddForce(launchDir.right * SpringCalc2());
                }

                

                #region Adjust tentacle based on distance
                thisTentacle.scale = magicNumber * thisTentacle.dist;
                UpdateTentacleLength(thisTentacle);
                #endregion

                #region Keep Akkoro clamped between the range of the tentacles
                #region tentacle distance
                if (thisTentacle.anchorPos.HasValue)
                    thisTentacle.dist = Vector2.Distance(thisTentacle.anchorPos.Value, transform.position);
                #endregion
                if (thisTentacle.dist >= tentacleRange)
                {
                    // find the point on the edge of the radius
                    Vector2 circlePoint = CirclePoint(thisTentacle.anchorPos.Value, tentacleRange, (Vector2.SignedAngle(Vector2.right, thisTentacle.rot.right * -1)));
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
                #endregion

                UpdateTentacleLength(thisTentacle);
                break;
            case Tentacles.Retracting:

                #region retract Anchor tentacle
                RetractTentacle(thisTentacle);
                #endregion

                break;
        }

        #region tentacle distance
        if (thisTentacle.anchorPos.HasValue)
            thisTentacle.dist = Vector2.Distance(thisTentacle.anchorPos.Value, transform.position);
        #endregion


    }

    /// <summary>
    /// Retracts the tentacle that is sent into the function
    /// </summary>
    /// <param name="tent"></param>
    /// <returns></returns>
    void RetractTentacle(Tentacle tent)
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
        if (enable) rb.gravityScale = gravScale;
        else rb.gravityScale = 0f;
    }

    /// <summary>
    /// Super complicated spring equation
    /// </summary>
    /// <returns></returns>
    float SpringCalc()
    {
        return 0f;
        /*
        if (!rightTentacle.anchorPos.HasValue || !leftTentacle.anchorPos.HasValue)
        {
            print("NULL VALUE in spring calculations");
            return 0f;
        }
        // k = spring constant
        float k = 0.4f;
        // a & b = anchor of aim tentacle
        float a = rightTentacle.anchorPos.Value.x;
        float b = rightTentacle.anchorPos.Value.y;
        // x0 & y0 = Akkoro's position
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        // x1 & y1 = anchor of anchor tentacle
        float x1 = leftTentacle.anchorPos.Value.x;
        float y1 = leftTentacle.anchorPos.Value.y;


        // x & y are variables
        float xx = Mathf.Abs((a * (x0 - x1)) + (b * (y0 - y1)));
        float yy = Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
        // l = distance of anchor tentacle
        l = rightTentacle.dist > leftTentacle.dist ? rightTentacle.dist : leftTentacle.dist;
        // d = x / y is a variable of x / y
        d = xx / yy;
        // X gets stored for later use
        print("d: " + d);

        Vector2 temp = betweenAnchors.position + (betweenAnchors.forward * d);
        xPoint.position = temp;
        xPoint.LookAt(transform);

        X = Vector2.Distance(transform.position, temp);
        //X = Mathf.Sqrt(Mathf.Pow(l, 2) - Mathf.Pow(d, 2));
        //print(X);
        // this is the final result
        return Mathf.Clamp(Mathf.Sqrt(k * ((Mathf.Pow(l, 2) - Mathf.Pow(d, 2)))) * 4300, 0f, 7800f);
        */
    }

    /// <summary>
    /// Simplified launch calculation
    /// </summary>
    /// <returns></returns>
    float SpringCalc2()
    {
        // find the average distance, raise to a power & multiply result by a constant
        return Mathf.Pow((leftTentacle.dist + rightTentacle.dist) / 2f, launchPow) * launchConst;
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
        float x = origin.x + (radius * Mathf.Cos(angle * Mathf.Deg2Rad));
        float y = origin.y + (radius * Mathf.Sin(angle * Mathf.Deg2Rad));
        return new Vector2(x, y);
    }

    /// <summary>
    /// Finds the normalized launch direction between the Aim & Anchor tentacles
    /// </summary>
    Vector2 CalculateLaunchDirection()
    {
        Vector2 aimVector = rightTentacle.rot.right;
        Vector2 anchorVector = leftTentacle.rot.right;
        return (aimVector + anchorVector).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Add anchorpoint to the list if Akkoro enters range
        if (collision.gameObject.layer == anchorLayer)
        {
            if (!anchorPositions.Contains(collision.gameObject.transform.position))
                anchorPositions.Add(collision.gameObject.transform.position);
        }
        #endregion

        if (collision.gameObject.layer == buttons)
        {
            collision.GetComponent<Button>().press = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        #region Remove anchorpoint from the list if Akkoro leaves range
        if (collision.gameObject.layer == anchorLayer)
        {
            if (anchorPositions.Contains(collision.gameObject.transform.position))
                anchorPositions.Remove(collision.gameObject.transform.position);
        }
        #endregion
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region calculate collision impact
        if (collision.relativeVelocity.magnitude > 40)
            // we have a hit!
        #endregion

        #region check for wall collision (sling)
        if (isSlinging && collision.gameObject.layer == walls)
        {
            print("impact has been made!");
            goBack = true;
            
            isSlinging = false;
        }
        #endregion
    }

    /// <summary>
    /// Tentacles are switched so that the leftmost tentacle will release when the left button is released; rightmost tentacle with right button
    /// </summary>
    /// <param name="left">Left tentacle</param>
    /// <param name="right">Right tentacle</param>
    void SwitchTentacle(ref Tentacle left, ref Tentacle right)
    {
        #region switch keys & mouse buttons
        var tempKey = left.key;
        left.key = right.key;
        right.key = tempKey;

        var tempMB = left.mouseButton;
        left.mouseButton = right.mouseButton;
        right.mouseButton = tempMB;
        #endregion
        #region switch tentacles
        var temp2 = left;
        left = right;
        right = temp2;
        #endregion
    }

    void BackToPengin(Vector3 currentPos, Vector3 penginPos)
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
