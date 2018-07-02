using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AkkoroPengin : MonoBehaviour
{
    public KeyCode launchPrep;
    public KeyCode launchButton;
    public KeyCode detach;
    
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

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        cam = Camera.main;

        trajectoryPoints = new List<GameObject>();
        //   TrajectoryPoints are instatiated
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            GameObject dot = Instantiate(TrajectoryPointPrefab);
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);

        }
    }

    private void OnEnable()
    {
        onEnable = true;
    }
    // Update is called once per frame
    void Update ()
    {
        #region launch
        if (Input.GetKey(launchPrep))
        {
            print("should be preparing for launch!");

            aimLaunch.right = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            #region trajectory

            enablePoints(true);
            setTrajectoryPoints(transform.position, aimLaunch.right.normalized * (launchForce/144));


            #endregion

            if (Input.GetKeyDown(launchButton))
            {
                Controller.switchUnits(Controlling.Akkoro, aimLaunch.right.normalized * launchForce);
                BackToPengin.isSlinging = true;
                enablePoints(false);
            }
        }
        else
        {
            enablePoints(false);
        }
        #endregion


        #region detach
        if (Input.GetKeyDown(detach) && !onEnable)
        {
            Controller.switchUnits(Controlling.Akkoro, Vector3.zero);
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
