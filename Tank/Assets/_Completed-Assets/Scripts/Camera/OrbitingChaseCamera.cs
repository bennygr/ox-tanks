using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingChaseCamera : MonoBehaviour {

    private const float Y_ANGLE_MIN = 0f;
    private const float Y_ANGLE_MAX = 50f;
        

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 10.0f;
    //private float sensitivityX = 4.0f;
    //private float sensitivitY = 1.0f;

    private void Start() {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update() {
        //if (Input.GetMouseButtonDown(1)) {
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        //}
    }

    private void LateUpdate() {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }

    /// <summary>
    /// Sets the target to look at
    /// </summary>
    /// <param name="target">Target.</param>
    public void LookAt(Transform target) {
        this.lookAt = target;
    }
}
