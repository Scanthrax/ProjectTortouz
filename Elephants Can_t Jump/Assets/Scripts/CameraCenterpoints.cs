using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CameraCenterpoints : MonoBehaviour {


    public Transform[] cameraPoints;
    public Camera cam;
    public int selectPoint;
    float lerp;
    public bool panCam;
    
    public GameObject player;

    void Start ()
    {
        panCam = false;
        cam.transform.position = cameraPoints[selectPoint].position;
    }

    private void Update()
    {
        selectPoint = Mathf.Clamp(selectPoint, 0, cameraPoints.Length-1);

        if(Input.GetKeyDown(KeyCode.P) && !panCam)
        {
            lerp = 0f;
            panCam = true;
        }

        if(panCam && selectPoint < cameraPoints.Length - 1)
        {
            CameraPan(selectPoint, selectPoint + 1);
        }
    }

    void CameraPan(int current, int next)
    {
        cam.transform.position = Vector3.Lerp(cameraPoints[current].position, cameraPoints[next].position,lerp);
        lerp += 0.08f;
        if(lerp > 1f)
        {
            lerp = 1f;
            panCam = false;
            selectPoint++;
            cam.transform.position = cameraPoints[selectPoint].position;
        }
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < cameraPoints.Length; i++)
        {
            Gizmos.color = Color.white;
            if(i != cameraPoints.Length-1)
                Gizmos.DrawLine(cameraPoints[i].position, cameraPoints[i + 1].position);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cameraPoints[i].position - new Vector3(Variables.horzExtent, -Variables.vertExtent, 0f), cameraPoints[i].position + new Vector3(Variables.horzExtent, Variables.vertExtent, 0f));
            Gizmos.DrawLine(cameraPoints[i].position - new Vector3(Variables.horzExtent, Variables.vertExtent, 0f), cameraPoints[i].position + new Vector3(Variables.horzExtent, -Variables.vertExtent, 0f));
            Gizmos.DrawLine(cameraPoints[i].position - new Vector3(Variables.horzExtent, -Variables.vertExtent, 0f), cameraPoints[i].position + new Vector3(-Variables.horzExtent, -Variables.vertExtent, 0f));
            Gizmos.DrawLine(cameraPoints[i].position - new Vector3(-Variables.horzExtent, Variables.vertExtent, 0f), cameraPoints[i].position + new Vector3(Variables.horzExtent, Variables.vertExtent, 0f));


        }
    }
}
