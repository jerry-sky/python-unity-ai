using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Rigidbody rb;

    float lookSensitivity = 50;
    float speed = 700f;
    float acceleration = 2f;
    private float actualSpeed = 0;

    float rotationX = 0.0f;
    float rotationY = 0.0f;


    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void LateUpdate () {
        rotationX += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp (rotationY, -90, 90);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            actualSpeed = speed*acceleration;
        }
        else
        {
            actualSpeed = speed;
        }
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        rb.velocity = Vector3.zero;
        rb.velocity += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * actualSpeed;
        rb.velocity += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * actualSpeed;

    }
}
