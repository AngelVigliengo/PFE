using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPC : MonoBehaviour
{
    private Rigidbody rigidBody;

    public Camera playerCamera;
    public float mouseSensitivity = 3f;
    public float maxLookAngle = 90f;
    public float moveSpeed = 10f;
    public float maxVelocityChange = 7f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch = pitch - Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0,0);
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity) * moveSpeed;

        Vector3 velocityChange = targetVelocity - rigidBody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, - maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, - maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);

        Vector3 gravity = new Vector3(0, -9.81f,0);
        float gravityMultiplayer = 10f;
        gravity *= gravityMultiplayer;

        if(rigidBody.velocity.y < 0){
            rigidBody.AddForce(gravity,ForceMode.Force);
        }
    }
}
