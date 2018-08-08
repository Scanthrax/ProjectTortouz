using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PenginSlingAkkoro : MonoBehaviour
{
    public KeyCode launchPrep;
    public KeyCode launchButton;
    
    public Transform aimLaunch;

    Camera cam;

    public float launchForce;

    bool onEnable;

    #region Delegates
    public delegate void StartSling();
    public static event StartSling startSling;
    #endregion

    int numOfTrajectoryPoints = 35;
    List<GameObject> trajectoryPoints;
    public GameObject TrajectoryPointPrefab;

    PlayerMovement playerMovement;
    public static bool prepLaunch;


    public GameObject Akkoro;
    public GameObject Pengin;

    PlayerMovement pm;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        Physics2D.queriesStartInColliders = false;
        cam = Camera.main;
        pm = Akkoro.GetComponent<PlayerMovement>();

        trajectoryPoints = new List<GameObject>();
        //   TrajectoryPoints are instatiated
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            GameObject dot = Instantiate(TrajectoryPointPrefab);
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);

        }
        prepLaunch = false;
    }

    private void OnEnable()
    {
        onEnable = true;
    }

    // Update is called once per frame
    void Update ()
    {
        #region launch
        if (Input.GetKey(launchPrep) && (playerMovement.leftTentacle.state == Tentacles.None && playerMovement.rightTentacle.state == Tentacles.None))
        {
            print("should be preparing for launch!");
            prepLaunch = true;
            aimLaunch.right = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            #region trajectory

            
            setTrajectoryPoints(transform.position + new Vector3(pm.faceDir * 2.5f,0), aimLaunch.right.normalized * (launchForce/155f));
            enablePoints(true);


            #endregion
            
            if (Input.GetKeyDown(launchButton))
            {

                enablePoints(false);


                Akkoro.SetActive(true);
                Akkoro.transform.position = transform.position + new Vector3(3,0);
                pm.room = GetComponent<PlayerMovement>().room;
                pm.rb.AddForce(aimLaunch.right * launchForce * 1.5f);
                pm.groundTimer = 0;
                pm.movement = Movement.Launch;
                pm.groundingBoxes[0] = false;
                

                Pengin.transform.position = transform.position;
                Pengin.SetActive(true);
                prepLaunch = false;

                gameObject.SetActive(false);
            }
        }
        else
        {
            enablePoints(false);
            prepLaunch = false;
        }
        #endregion


        onEnable = false;
    }

    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            trajectoryPoints[i].transform.position = pos;
            //trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }
    void enablePoints(bool enable)
    {
        foreach(GameObject point in trajectoryPoints)
        {
            point.GetComponent<Renderer>().enabled = enable;
        }
    }


}
