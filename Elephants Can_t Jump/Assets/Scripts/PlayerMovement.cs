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
    public float speed = 2f;
    /// <summary>
    /// Scales the intensity of the gravity
    /// </summary>
    public float gravScale = 15f;
    /// <summary>
    /// is Akkoro gripping?
    /// </summary>    
    public bool gripping;
    /// <summary>
    /// Maximum amount of stamina Akkoro has for wallclimbing
    /// </summary>
    public int maxStamina = 25;
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
    public Grounding groundingLR;
    public Grounding groundingTB;


    /// <summary>
    /// Ints used to determine direction
    /// </summary>
    int left, right, up, down;
    /// <summary>
    /// Floats used to translate Akkoro
    /// </summary>
    public float hor, vert;
    /// <summary>
    /// Integer value of layer Walls
    /// </summary>
    int walls;
    
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
        public Transform anchorPos;
        /// <summary>
        /// The keycode that performs actions for this tentacle
        /// </summary>
        public KeyCode key;

        public int mouseButton;

        public bool clampTent;
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
    public List<Transform> anchorPositions = new List<Transform>();
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
    public float launchConst = 350f, launchPow = 1.55f;

    public bool switchTentacles;

    public Sling slingshot;

    public bool isSlinging;
    public bool goBack;
    float lerp = 0f;
    public int buttons;
    Animator anim;
    BoxCollider2D bc;
    public Grounding onWall;
    public float maxSpeed = 10f;
    public Room room;
    public bool isSliding;
    PhysicsMaterial2D pmLR = null, pmTB = null;
    public Movement movement;
    bool launch;
    int groundTimer;
    bool clampTentacles;
    public int faceDir;
    public bool[] groundingBoxes = new bool[4];
    public BoxCollider2D wallbreakCol;
    public bool action;
    public Sprite[] anchorSprites;
    RaycastHit2D[] hits = new RaycastHit2D[10];

    void Start()
    {
        // assign the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        // assign Wall layer to variable
        walls = LayerMask.NameToLayer("Walls");
        anchorLayer = LayerMask.NameToLayer("Anchors");
        buttons = LayerMask.NameToLayer("Buttons");
        // make sure that raycasts don't detect the colliders that they start in
        Physics2D.queriesStartInColliders = false;
        stamina = maxStamina;
        rend = GetComponentInChildren<SpriteRenderer>();
        leftTentacle.anchorPos = null;
        rightTentacle.anchorPos = null;
        switchTentacles = false;
        slingshot = Sling.None;
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        //gameObject.SetActive(false);
        movement = Movement.Ground;
        faceDir = 1;
    }

    #region anim functions
    public void enableAttack()
    {
        wallbreakCol.enabled = true;
    }
    public void disableAttack()
    {
        wallbreakCol.enabled = false;
    }
    public void enableWallBreak()
    {
        action = true;
    }
    public void disableWallBreak()
    {
        action = false;
    }
    public void GroundSlam()
    {
        // find all items within 10 unit radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
        // go through each item
        foreach(Collider2D hit in colliders)
        {
            // can the item be dropped? i.e. does the item have the Droppable component?
            Droppable canBeDropped = hit.GetComponent<Droppable>();
            // item can be dropped if the variable has a value other than null
            if (canBeDropped != null)
            {
                // set gravity for droppable object
                hit.GetComponent<Rigidbody2D>().gravityScale = 3f;
            }
        }
    }
    #endregion

    void Update()
    {
        #region Order nearby anchorpoints by distance
        Functions.OrderByDistance(anchorPositions, transform.position);
        #endregion

        #region Find out which walls Akkoro is currently grounded to

        if(groundingBoxes[0])
        {
            groundingTB = Grounding.Bottom;
        }
        else if(groundingBoxes[3])
        {
            groundingTB = Grounding.Top;
        }
        else
        {
            groundingTB = Grounding.None;
        }

        if(groundingBoxes[1])
        {
            groundingLR = Grounding.Left;
        }
        else if(groundingBoxes[2])
        {
            groundingLR = Grounding.Right;
        }
        else
        {
            groundingLR = Grounding.None;
        }

        if (groundingTB != Grounding.Bottom)
        {
            movement = Movement.Airborne;
        }
        else if (groundingTB == Grounding.Bottom)
        {
            movement = Movement.Ground;
        }
        #endregion

        #region Holding spacebar enables grip
        if (Input.GetKey(Variables.wallGrip))
        {
            if (groundingLR != Grounding.None || groundingTB == Grounding.Top)
            {
                gripping = true;
                movement = Movement.Wallclimb;
            }
            else gripping = false;
        }
        else
        {
            gripping = false;
        }
        #endregion

        #region movement
        // The player will only be able to move on walls when gripping
        if (gripping  && stamina > 0)
        {
            up = Input.GetKey(KeyCode.W) ? 1 : 0;
            down = Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else
        {
            up = 0;
            down = 0;
        }
        left = Input.GetKey(KeyCode.A) ? -1 : 0;
        right = Input.GetKey(KeyCode.D) ? 1 : 0;

        if(stamina <= 0)
        {
            if(groundingLR == Grounding.Left)
            {
                left = 0;
            }
            else if(groundingLR == Grounding.Right)
            {
                right = 0;
            }
        }

        int i = (left + right);
        int j = (up + down);

        // horizontal movement with A & D
        hor = (left + right) * speed;
        // vertical movement with W & S
        vert = (up + down) * speed;


        
        if (i != 0)
        {
            if (i != faceDir)
            {
                faceDir = i;
                rend.flipX = !rend.flipX;
                bc.offset = new Vector2(-bc.offset.x, bc.offset.y);
            }
        }
        

        if(action)
        {
            hor = 0f;
        }


        if( (groundingLR == Grounding.Left && i == -1) || (groundingLR == Grounding.Right && i == 1) || i == 0)
        {
            anim.SetBool("walkHor", false);
        }
        else if (i != 0)
        {
            anim.SetBool("walkHor", true);
        }

        print(j);
        if ((groundingTB == Grounding.Top && j == 1) || (groundingTB == Grounding.Bottom && j == -1) || j == 0)
        {
            anim.SetBool("walkVert", false);
        }
        else if (j != 0)
        {
            anim.SetBool("walkVert", true);
        }

        #region set bool in animator



        if (movement == Movement.Airborne)
        {
            anim.SetBool("inAir", true);
        }
        else
        {
            anim.SetBool("inAir", false);
        }

        if(groundingTB != Grounding.Bottom && groundingLR != Grounding.None && gripping)
        {
            anim.SetBool("onWall", true);
        }
        else
        {
            anim.SetBool("onWall", false);
        }

        if (groundingTB == Grounding.Top)
        {
            anim.SetBool("onCeil", true);
        }
        else
        {
            anim.SetBool("onCeil", false);
        }

        if(groundingTB == Grounding.Bottom)
        {
            anim.SetBool("onGround", true);
        }
        else
        {
            anim.SetBool("onGround", false);
        }

        if (movement == Movement.Wallclimb)
        {
            if (vert < 0)
            {
                rend.flipY = true;
            }
            else if (vert > 0)
            {
                rend.flipY = false;
            }
        }
        else
        {
            rend.flipY = false;
        }
        

        
        #endregion



        #endregion

        #region Stickiness
        if (false)
        {
            stamina--;
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

        #region Tentacle logic
        TentacleGrab(ref leftTentacle, ref rightTentacle);
        TentacleGrab(ref rightTentacle, ref leftTentacle);

        if(leftTentacle.rot.rotation.z < rightTentacle.rot.rotation.z && switchTentacles)
        {
            SwitchTentacle(ref leftTentacle, ref rightTentacle);

            switchTentacles = false;
        }
        #endregion

        #region Recover Stamina
        if (false)
        {
            stamina += 5;
        }
        #endregion

        #region clamp stamina
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        #endregion

        if(movement == Movement.Wallclimb)
        {
            if (gripping)
            {
                EnableGravity(false);
            }
            else
            {
                EnableGravity(true);
            }
        }
        else
        {
            EnableGravity(true);
        }


    }



    private void FixedUpdate()
    {
        // only move horizontally at reduced rate while airborne
        if (movement == Movement.Airborne)
        {
            rb.AddForce(new Vector2(hor * 100f * 0.25f, 0f));
        }
        // otherwise move vertically & horizontally
        else
        {
            rb.AddForce(new Vector2( hor * 100, vert * 100));
        }

        // launch
        if (launch)
        {
            rb.AddForce(launchDir.right * SpringCalc2());
            launch = false;
        }

        // while grounded to any surface
        if (movement != Movement.Airborne)
        {
            // set horizontal velocity to 0
            if (hor == 0)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            // set vertical velocity to 0
            if(vert == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            // make sure we do not exceed max speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        // give a small window of time where we cannot ground ourselves, used immediately when launching
        if (groundTimer < 10)
        {
            groundTimer++;
        }
        else
        {
            // detect when we hit the ground
            if (groundingTB == Grounding.Bottom && movement == Movement.Airborne)
            {
                movement = Movement.Ground;
            }
        }

        if (leftTentacle.clampTent)
        {
            ClampTentacles(leftTentacle);
            leftTentacle.clampTent = false;
        }
        else if (rightTentacle.clampTent)
        {
            ClampTentacles(rightTentacle);
            rightTentacle.clampTent = false;
        }

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
                        if (otherTentacle.anchorPos != null)
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
                        if (otherTentacle.anchorPos != null)
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
                if(thisTentacle.anchorPos != null)
                {
                    if(Vector2.Distance(transform.position, thisTentacle.anchorPos.position) > tentacleRange)
                    {
                        print("Retracting & breaking connection");
                        thisTentacle.state = Tentacles.Retracting;
                        thisTentacle.anchorPos = null;
                        break;
                    }
                }
                #endregion

                #region calculate distance
                thisTentacle.rot.right = thisTentacle.anchorPos.position - transform.position;
                
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
                thisTentacle.rot.right = thisTentacle.anchorPos.position - transform.position;
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
                    launch = true;
                    groundTimer = 0;
                    movement = Movement.Airborne;
                }

                

                #region Adjust tentacle based on distance
                thisTentacle.scale = magicNumber * thisTentacle.dist;
                UpdateTentacleLength(thisTentacle);
                #endregion

                #region Keep Akkoro clamped between the range of the tentacles
                #region tentacle distance
                if (thisTentacle.anchorPos != null)
                    thisTentacle.dist = Vector2.Distance(thisTentacle.anchorPos.position, transform.position);
                #endregion
                if (thisTentacle.dist >= tentacleRange)
                {
                    thisTentacle.clampTent = true;
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
        if (thisTentacle.anchorPos != null)
            thisTentacle.dist = Vector2.Distance(thisTentacle.anchorPos.position, transform.position);
        #endregion

        
        // for each anchor nearby
        foreach (Transform trans in anchorPositions)
        {
            // linecast from A+P to anchorpoint
            var j = Physics2D.LinecastAll(transform.position, trans.position);

            // for each object hit with linecast
            foreach (RaycastHit2D hit in j)
            {
                // if I hit a wall...
                if (hit.transform.gameObject.layer == walls)
                {
                    print("there is a wall in the way!");
                    // if this tentacle is targeting the anchorpoint, retract it
                    if(thisTentacle.anchorPos == trans)
                        thisTentacle.state = Tentacles.Retracting;
                    // dim the anchorpoint
                    trans.GetComponent<SpriteRenderer>().sprite = anchorSprites[0];
                    break;
                }
            }
        }

        if(thisTentacle.dist > 7f)
        {
            thisTentacle.state = Tentacles.Retracting;
        }
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
            tent.anchorPos = null;
        }
        UpdateTentacleLength(tent);
    }

    /// <summary>
    /// Updates the length of the tentacle based on the scale of the tentacle provided
    /// </summary>
    /// <param name="tent"></param>
    void UpdateTentacleLength(Tentacle tent)
    {
        tent.rend.transform.localPosition = new Vector3(tent.scale * 8, 0f, 0f);
        tent.rend.size = new Vector2(16 * tent.scale, tent.rend.size.y);
    }

    // 2.8442
    // 5.688429



    void ClampTentacles(Tentacle tent)
    {
        // find the point on the edge of the radius
        Vector2 circlePoint = CirclePoint(tent.anchorPos.position, tentacleRange, (Vector2.SignedAngle(Vector2.right, tent.rot.right * -1)));
        // Only adjust the x value of the transform when on ceiling or ground
        if (groundingTB != Grounding.None)
            transform.position = new Vector2(circlePoint.x, transform.position.y);
        // Only adjust the y value of the transform when on walls
        else if (groundingLR != Grounding.None)
            transform.position = new Vector2(transform.position.x, circlePoint.y);
        //Adjust both x & y during any other case
        else
            transform.position = new Vector2(circlePoint.x, circlePoint.y);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        #region Add anchorpoint to the list if Akkoro enters range
        if (collision.gameObject.layer == anchorLayer)
        {
            if (!anchorPositions.Contains(collision.gameObject.transform))
            {
                anchorPositions.Add(collision.gameObject.transform);
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = anchorSprites[1];
            }
        }
        #endregion

        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        #region Remove anchorpoint from the list if Akkoro leaves range
        if (collision.gameObject.layer == anchorLayer)
        {
            if (anchorPositions.Contains(collision.gameObject.transform))
            {
                anchorPositions.Remove(collision.gameObject.transform);
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = anchorSprites[0];
            }
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
