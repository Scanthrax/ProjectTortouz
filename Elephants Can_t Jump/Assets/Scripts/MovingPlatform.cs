using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;

    public float speed;

    private Transform currentP;

    public Transform[] pointArr;

    public int pointSelection;
    // Use this for initialization
    void Start()
    {
        currentP = pointArr[pointSelection];

        //Gizmos from nodes

	}
	
	// Update is called once per frame
	void Update ()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentP.position, Time.deltaTime * speed);
        if(platform.transform.position == currentP.position)
        {
            pointSelection++;
            if (pointSelection == pointArr.Length)
            {
                pointSelection = 0;
            }

            currentP = pointArr[pointSelection];
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < (pointArr.Length - 1); i++)
        {
            Gizmos.DrawLine(pointArr[i].position, pointArr[i + 1].position);
        }
    }
}
