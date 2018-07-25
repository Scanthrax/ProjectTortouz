using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlatform : MonoBehaviour {

    Transform storeParent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region Platform Movement
        if (collision.transform.tag == "Platform")
        {
            storeParent = this.transform.parent;
            Debug.Log("parentchange");
            transform.parent = collision.transform;
        }
        #endregion
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        #region Platform Movement
        if (collision.transform.tag == "Platform")
        {
            Debug.Log("parentchange");
            transform.parent = storeParent;
        }
        #endregion
    }

    private void OnEnable()
    {
        #region Platform Movement
            Debug.Log("parentchange");
            transform.parent = storeParent;
        #endregion
    }

    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(ball) and initial velocity of object(ball).
    //---------------------------------------    
    //void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    //{
    //    float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
    //    float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
    //    float fTime = 0;

    //    fTime += 0.1f;
    //    for (int i = 0; i < numOfTrajectoryPoints; i++)
    //    {
    //        float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
    //        float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
    //        Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
    //        trajectoryPoints[i].transform.position = pos;
    //        trajectoryPoints[i].renderer.enabled = true;
    //        trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
    //        fTime += 0.1f;
    //    }
    //}



}
