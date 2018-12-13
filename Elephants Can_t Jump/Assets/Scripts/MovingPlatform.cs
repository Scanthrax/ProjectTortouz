using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool canMove;
    public GameObject ButtonSystem;

    public GameObject platform; //moving platform
    public float speed;         //speed
    private Transform currentP; 
    public Transform[] pointArr;//array of points
    public int pointSelection;
    int delay;
    public Button button;


    private void Awake()
    {
        //button.platform = this;
    }

    // Use this for initialization
    void Start()
    {
        currentP = pointArr[pointSelection]; //Start at the first point
        delay = 0;
        canMove = !button.isPressed;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if a button is assigned to the platform
        if (ButtonSystem != null)
        {
            canMove = button.isPressed;
        }
        //if there is no button then platform is always moving
        else
        {
            canMove = true;
        }

        if (canMove)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentP.position, Time.deltaTime * speed); //move the platform towards the next point in the array
            if (platform.transform.position == currentP.position) //once the platform arrives at the point
            {
                if(delay < 60)
                {
                    delay++;
                }

                else
                {
                    pointSelection++; //move to the next point
                    if (pointSelection == pointArr.Length)//if we arrive at the last point
                    {
                        pointSelection = 0;//go back to the first point
                    }

                    currentP = pointArr[pointSelection]; // reset
                    delay = 0;
                }
            }
        }

    }

    //gizmos to help see the path of the platform
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < (pointArr.Length - 1); i++)
        {
            Gizmos.DrawLine(pointArr[i].position, pointArr[i + 1].position);
        }
    }
}
