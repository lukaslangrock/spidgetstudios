using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform targetCamera;

    public float smoothSpeed = 0.1f;

    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, targetCamera.position, smoothSpeed);
        transform.LookAt(target);
    }
}
