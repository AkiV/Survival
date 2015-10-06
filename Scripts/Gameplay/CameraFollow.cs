using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public Transform target;

    void LateUpdate()
    {
        if (target)
        {
            Vector3 newPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = newPosition;
        }
    }
}