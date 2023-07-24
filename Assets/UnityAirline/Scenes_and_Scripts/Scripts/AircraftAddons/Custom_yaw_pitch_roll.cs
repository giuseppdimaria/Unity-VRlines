using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json.Linq;
using Valve.VR;

public class Custom_yaw_pitch_roll : MonoBehaviour
{
    public float yawSpeed = 1f;
    public float pitchSpeed = 1f;
    public float rollSpeed = 1f;

    private Rigidbody rb;
    private float yaw;
    private float pitch;

    private float roll;

    AirplaneController controller;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GameObject.Find("Aircraft").GetComponent<AirplaneController>();    
    }


    void Update()
    {

        // Get input axes for yaw, pitch, and roll
        float yawInput = Input.GetAxis("Yaw");
        float pitchInput = Input.GetAxis("Vertical");
        float rollInput = Input.GetAxis("Horizontal");
        //SteamVR_Input.FlightSimulator.inActions.{ yourActionName}.GetAxis(SteamVR_Input_Sources.Any);

        // Map AWSD commands to yaw, pitch, and roll variations
        if (Input.GetKey(KeyCode.J))
        {
            rollInput = -1f;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            rollInput = 1f;
        }

        if (Input.GetKey(KeyCode.K))
        {
            pitchInput = -1f;
        }
        else if (Input.GetKey(KeyCode.I))
        {
            pitchInput = 1f;
        }

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    rollInput = -1f;
        //}
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    rollInput = 1f;
        //}

        // Calculate rotation angles based on input and speedZ
        yaw += yawInput * yawSpeed * Time.deltaTime;
        yaw = Mathf.Clamp(yaw, -1, 1);
        controller.setYaw(yaw);

        pitch += pitchInput * pitchSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -1, 1);

        controller.setPitch(pitch);

        roll += rollInput * rollSpeed * Time.deltaTime;
        roll = Mathf.Clamp(roll, -1, 1);

        controller.setRoll(roll);

        //Pitch = Input.GetAxis("Vertical");
        //Roll = Input.GetAxis("Horizontal");
        //Yaw = Input.GetAxis("Yaw");

        //// Create a rotation quaternion based on yaw, pitch, and roll angles
        //Quaternion rotation = Quaternion.Euler(pitch, yaw, roll);

        //// Apply rotation to the Rigidbody
        //rb.MoveRotation(rotation);
    }
}
