using UnityEngine;

public class TransformAlignment : MonoBehaviour {

    void LateUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
