﻿using System.Collections;
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
    public float gravScale = 5f;
    /// <summary>
    /// is Akkoro gripping?
    /// </summary>    
    public bool gripping;
    /// <summary>
    /// Maximum amount of stamina Akkoro has for wallclimbing
    /// </summary>
    public int maxStamina = 20;
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
    public Grounding groundingCorners;
    /// <summary>
    /// Range of the raycasts that project from Akkoro to all sides; used to determine grounding range
    /// </summary>
    public float wallDist = 0.2f;
    public float wallDistCorner = 0.2f;
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
    public float hor, vert;
    /// <summary>
    /// Integer value of layer Walls
    /// </summary>
    int walls;
    /// <summary>
    /// Raycasts to pass into the DetermineGrounding function
    /// </summary>
    RaycastHit2D hitTRDiag, hitTLDiag, hitBLDiag, hitBRDiag, hitTRU, hitTLU, hitLTL, hitLBL, hitBLD, hitBRD, hitRTR, hitRBR;
    /// <summary>
    /// This function is used to calculate the grounding from the raycast. The RaycastHit param is the raycast to be checked; the Grounding param assigns which side the hit should be checking for.
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="ground"></param>
    void DetermineGrounding(RaycastHit2D hit, int i)
    {
        if (hit.collider != null && hit.collider.gameObject.layer == walls)
        {
            raycastGrounding[i] = true;
        }
        else
        {
            raycastGrounding[i] = false;
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

    public bool switchTentacles;

    public Sling slingshot;

    public bool isSlinging;
    public bool goBack;
    float lerp = 0f;
    public int buttons;
    Animator anim;
    BoxCollider2D bc;
    public Grounding onWall;
    public float maxSpeed = 300f;
    public Room room;
    public bool isSliding;
    PhysicsMaterial2D pmLR = null, pmTB = null;
    public Movement movement;
    bool launch;
    int groundTimer;
    bool clampTentacles;
    public int faceDir;
    

    void FindCurrentRoom()
    {
           
    }

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

    void Update()
    {
        #region Order nearby anchorpoints by distance
        Functions.OrderByDistance(anchorPositions, transform.position);
        #endregion

        #region raycasting system
        #region Set up raycasting for 4 directions

        var offset = transform.position + new Vector3(bc.offset.x, bc.offset.y);


        
        //diagonals
        hitTRDiag = Physics2D.Raycast(offset + new Vector3(bc.size.x,bc.size.y) * 0.5f, Vector2.right + Vector2.up, wallDistCorner);
        hitTLDiag = Physics2D.Raycast(offset + new Vector3(-bc.size.x, bc.size.y * 0.5f), Vector2.left + Vector2.up, wallDistCorner);
        hitBLDiag = Physics2D.Raycast(offset + new Vector3(-bc.size.x, -bc.size.y) * 0.5f, Vector2.down + Vector2.left, wallDistCorner);
        hitBRDiag = Physics2D.Raycast(offset + new Vector3(bc.size.x, bc.size.y) * 0.5f, Vector2.down + Vector2.right, wallDistCorner);

        //tops
        hitTRU = Physics2D.Raycast(offset + new Vector3(bc.size.x, bc.size.y) * 0.5f, Vector2.up, wallDist);
        hitTLU = Physics2D.Raycast(offset + new Vector3(-bc.size.x, bc.size.y) * 0.5f, Vector2.up, wallDist);
        //lefts
        hitLTL = Physics2D.Raycast(offset + new Vector3(-bc.size.x, bc.size.y) * 0.5f, Vector2.left, wallDist);
        hitLBL = Physics2D.Raycast(offset + new Vector3(-bc.size.x, -bc.size.y) * 0.5f, Vector2.left, wallDist);
        //bottoms
        hitBLD = Physics2D.Raycast(offset + new Vector3(-bc.size.x, -bc.size.y) * 0.5f, Vector2.down, wallDist);
        hitBRD = Physics2D.Raycast(offset + new Vector3(bc.size.x, -bc.size.y) * 0.5f, Vector2.down, wallDist);
        //rights
        hitRTR = Physics2D.Raycast(offset + new Vector3(bc.size.x, bc.size.y) * 0.5f, Vector2.right, wallDist);
        hitRBR = Physics2D.Raycast(offset + new Vector3(bc.size.x, -bc.size.y) * 0.5f, Vector2.right, wallDist);


        #endregion
        #region Calculate the groundings for all directions
        DetermineGrounding(hitTRDiag,0);
        DetermineGrounding(hitTLDiag, 1);
        DetermineGrounding(hitBLDiag, 2);
        DetermineGrounding(hitBRDiag, 3);
        DetermineGrounding(hitTRU, 4);
        DetermineGrounding(hitTLU, 5);
        DetermineGrounding(hitLTL, 6);
        DetermineGrounding(hitLBL, 7);
        DetermineGrounding(hitBLD, 8);
        DetermineGrounding(hitBRD, 9);
        DetermineGrounding(hitRTR, 10);
        DetermineGrounding(hitRBR, 11);
        #endregion
        #region Find out which walls Akkoro is currently grounded to
        
        if (raycastGrounding[10] || raycastGrounding[11])
        {
            groundingLR = Grounding.Right;
        }
        else if (raycastGrounding[6] || raycastGrounding[7])
        {
            groundingLR = Grounding.Left;
        }
        else
        {
            groundingLR = Grounding.None;
        }

        if (raycastGrounding[4] || raycastGrounding[5])
        {
            groundingTB = Grounding.Top;
        }
        else if (raycastGrounding[8] || raycastGrounding[9])
        {
            groundingTB = Grounding.Bottom;
        }
        else
        {
            groundingTB = Grounding.None;
        }

        if(raycastGrounding[0])
        {
            groundingCorners = Grounding.TopRight;
        }
        else if(raycastGrounding[1])
        {
            groundingCorners = Grounding.TopLeft;
        }
        else if (raycastGrounding[2])
        {
            groundingCorners = Grounding.BottomLeft;
        }
        else if (raycastGrounding[3])
        {
            groundingCorners = Grounding.BottomRight;
        }
        else
        {
            groundingCorners = Grounding.None;
        }

        if(groundingLR != Grounding.None || groundingTB != Grounding.None || groundingCorners != Grounding.None)
        {
            grounding = Grounding.Grounded;
        }
        else
        {
            grounding = Grounding.None;
        }


        #endregion
        #endregion

        #region Disable gravity if gripping & grounded to a wall
        //if (gripping && grounding == Grounding.Grounded) EnableGravity(false);
        //else EnableGravity(true);
        #endregion

        #region airborne
        if(grounding == Grounding.None)
        {
            //anim.Play("Airborne");
        }
        #endregion

        #region movement
        // The player will only be able to move on walls when gripping
        if (gripping && grounding == Grounding.Grounded && stamina > 0)
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
        }

        // horizontal movement with A & D
        hor = (left + right) * speed * Time.deltaTime;
        // vertical movement with W & S
        vert = (up + down) * speed * Time.deltaTime;


        var i = (left + right);
        if (i != 0)
        {
            if (i != faceDir)
            {
                faceDir = i;
                rend.flipX = !rend.flipX;
                bc.offset = new Vector2(-bc.offset.x, bc.offset.y);
            }
        }
        

        #region Holding spacebar enables grip
        if (Input.GetKey(Variables.wallGrip))
        {
            if (groundingLR != Grounding.None || groundingTB == Grounding.Top)
            {
                gripping = true;
                if (groundingTB != Grounding.Bottom)
                {
                    movement = Movement.Wallclimb;
                    print(movement);
                }
            }
            else gripping = false;
        }
        else
        {
            gripping = false;
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



        #endregion

        if(grounding == Grounding.None)
        {
            movement = Movement.Airborne;
        }

        #region Stickiness
        if (grounding != Grounding.None && gripping && !(raycastGrounding[8] || raycastGrounding[9]) && Time.timeScale != 0f)
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

        #region Recover Stamina
        if ((raycastGrounding[8] || raycastGrounding[9]) && !gripping)
        {
            stamina += 5;
        }
        #endregion

        #region clamp stamina
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        #endregion

        /*
        #region go back to pengin
        BackToPengin(transform.position, pengin.position);
        #endregion
        */

        if (movement == Movement.Wallclimb)
        {
            EnableGravity(false);
        }
        else
            EnableGravity(true);
        
    }



    private void FixedUpdate()
    {
        if (movement == Movement.Airborne)
        {
            rb.AddForce(new Vector2(hor * 1000f * 0.25f, 0f));
        }
        else
        {
            rb.AddForce(new Vector2(hor * 1000, vert * 1000));
        }


        if (launch)
        {
            rb.AddForce(launchDir.right * SpringCalc2());
            launch = false;
        }

        if (movement != Movement.Airborne)
        {
            if (hor == 0)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            if(vert == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }


        if (groundTimer < 10)
        {
            groundTimer++;
        }
        else
        {
            if (groundingTB == Grounding.Bottom)
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
                if (thisTentacle.anchorPos.HasValue)
                    thisTentacle.dist = Vector2.Distance(thisTentacle.anchorPos.Value, transform.position);
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
        tent.rend.transform.localPosition = new Vector3(tent.scale * 8, 0f, 0f);
        tent.rend.size = new Vector2(16 * tent.scale, tent.rend.size.y);
    }

    // 2.8442
    // 5.688429



    void ClampTentacles(Tentacle tent)
    {
        // find the point on the edge of the radius
        Vector2 circlePoint = CirclePoint(tent.anchorPos.Value, tentacleRange, (Vector2.SignedAngle(Vector2.right, tent.rot.right * -1)));
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
