using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;
using Valve.VR;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    List<AeroSurface> controlSurfaces = null;
    [SerializeField]
    List<WheelCollider> wheels = null;
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;

    [SerializeField]
    float thrustControlSensitivity = 0.05f;

    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;

    [SerializeField]
    Text displayText = null;

    float thrustPercent;
    float brakesTorque;

    bool inputActive = false;

    GameObject RyanairPlane;
    AircraftPhysics aircraftPhysics;
    Rigidbody rb;

    //public SteamVR_Action_Boolean Thrust = SteamVR_Input.GetBooleanAction("Thrust");

    private void Start()
    {
        RyanairPlane = GameObject.Find("3DRyanairPlane");
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();

        //while(GameObject.Find("VRCamera").GetComponent<TrackedPoseDriver>() == null)
        //{

        //}
        //GameObject.Find("VRCamera").GetComponent<TrackedPoseDriver>().trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
        //GameObject.Find("VRCamera").GetComponent<TrackedPoseDriver>().trackingType = TrackedPoseDriver.TrackingType.RotationOnly;


        //GameObject.Find("Player").transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void Update()
    {

        //bool all_good = true;

        // Check if the VIVE controller's trackpad input is active
        inputActive = SteamVR_Actions.Nuovo_flight_simulator.Driving.GetAxis(SteamVR_Input_Sources.Any).magnitude > 0;

        // Update the input values based on VIVE controller's trackpad position
        if (inputActive)
        {
            Vector2 touchpadPos = SteamVR_Actions.Nuovo_flight_simulator.Driving.GetAxis(SteamVR_Input_Sources.Any);
            Roll = touchpadPos.x * rollControlSensitivity;
            Pitch = touchpadPos.y * pitchControlSensitivity;
        }
        else
        {
            // Reset the input values when the trackpad input is inactive
            //Roll = 0;
            //Pitch = 0;
            Pitch = Input.GetAxis("Vertical");
            Roll = Input.GetAxis("Horizontal");
            Yaw = Input.GetAxis("Yaw");
        }

        if (!IsSteamVRActive())//(!SteamVR.active)
        {
            //UnityEngine.Debug.Log(SteamVR.active);
            Pitch = Input.GetAxis("Vertical");
            Roll = Input.GetAxis("Horizontal");
            Yaw = Input.GetAxis("Yaw");

        }


        //UnityEngine.Debug.Log("this is trust " + Thrust);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //thrustPercent = thrustPercent > 0 ? 0 : 1f;
            SetThrust();
        }

        if (Input.GetKey(KeyCode.F))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        if (Input.GetKey(KeyCode.B))
        {
            //brakesTorque = brakesTorque > 0 ? 0 : 100f;
            SetBrake();
        }

        if (Input.GetKey(KeyCode.P))
        {
            Animator AircraftAnimator = RyanairPlane.GetComponent<Animator>();
            //AircraftAnimator.SetBool("canRunAnimation", true);
            AircraftAnimator.SetTrigger("RunAnimation");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetThrustControlSensitivity();
            //thrustPercent -= 0.1f * Time.deltaTime;
            //thrustPercent = Mathf.Clamp(thrustPercent, 0, 1f);
        }

        displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
        displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
        displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
        displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";
    }

    public void SetThrust()
    {
        float percent = thrustPercent + thrustControlSensitivity;
        thrustPercent = Mathf.Clamp01(percent);
    }


    public void SetBrake()
    {
       brakesTorque = brakesTorque > 0 ? 0 : 100f;
    }

    public void SetThrustControlSensitivity()
    {
        thrustControlSensitivity *= -1;
    }

    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);

        aircraftPhysics.SetThrustPercent(thrustPercent);
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = brakesTorque;
            // small torque to wake up wheel collider
            wheel.motorTorque = 0.01f;
        }
    }

    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(flap * surface.InputMultiplyer);
                    break;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }

    public void setPitch(float novel_pitch)
    {
        Pitch = novel_pitch;
    }

    public void setYaw(float novel_yaw)
    {
        Yaw = novel_yaw;
    }

    public void setFlap(float novel_flap)
    {
        Flap = novel_flap;
    }

    public void setRoll(float novel_roll)
    {
        Roll = novel_roll;
    }

    public float GetThrustPercecent()
    {
        return this.thrustPercent;
    }

    public float GetBrakesTorque()
    {
        return this.brakesTorque;
    }


    private bool IsSteamVRActive()
    {
        GameObject player = GameObject.Find("Player");
        return player != null;
    }

}
