using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using static UnityEngine.UI.Image;


public class ViveController : MonoBehaviour
{
        [SerializeField]
        float thrustControlSensitivity = 0.1f;
        float thrustPercent;
        public GameObject right_controller;
        public GameObject left_controller;


        private Hand right_controller_hand;
        private Hand left_controllerr_hand;

        public SteamVR_Action_Boolean Thrust;
        public SteamVR_Action_Boolean Gears;
        public SteamVR_Action_Boolean Brake;
        public SteamVR_Action_Boolean ThrustControlSensitivity;

        public SteamVR_Action_Vector2 Driving;

        public SteamVR_Action_Boolean SwitchCamera;

        AirplaneController airplanecontroller;

        camSwitch switcher;

        private float yaw;

        private float pitch;

        private float roll;

        public float yawSpeed = 0.001f;
        public float pitchSpeed = 0.001f;
        public float rollSpeed = 0.001f;

    private void Start()
                {

                GameObject airplane = GameObject.Find("Aircraft");
                airplanecontroller = airplane.GetComponent<AirplaneController>();

                GameObject SwitchCam = GameObject.Find("switchCam");
                switcher = SwitchCam.GetComponent<camSwitch>();

                right_controller_hand = right_controller.GetComponent<Hand>();
                left_controllerr_hand = left_controller.GetComponent<Hand>();

                if (Thrust == null)
                {
                    UnityEngine.Debug.LogError("<b>[Stea,VR Interaction]</b>, DIO SANTO!!!");
                    return;
                }

                Thrust.AddOnStateDownListener(OnTrustActionChange, left_controllerr_hand.handType);
                Brake.AddOnStateDownListener(OnBrakeActionChange, left_controllerr_hand.handType);

                Driving.AddOnChangeListener(OnDrivingActionChange, left_controllerr_hand.handType);

                ThrustControlSensitivity.AddOnStateDownListener(OnSetthrustControlSensitivityActionChange, right_controller_hand.handType);
                SwitchCamera.AddOnStateDownListener(SwitchCameraActionChange, right_controller_hand.handType);

            }
 
            private void OnTrustActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource)
            {

                airplanecontroller.SetThrust();
                airplanecontroller.setPitch(1.0f);

            }

            private void OnBrakeActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource)
            {

                airplanecontroller.SetBrake();

            }

            private void OnSetthrustControlSensitivityActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource)
            {

                airplanecontroller.SetThrustControlSensitivity();

            }

            private void SwitchCameraActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource)
            {

                switcher.SetSwitchCamera();

            }

            private void OnDrivingActionChange(SteamVR_Action_Vector2 actionIn, SteamVR_Input_Sources inputSource, Vector2 axis, Vector2 delta)
            {

                float yawInput = Input.GetAxis("Yaw");
                float pitchInput = Input.GetAxis("Vertical");
                float rollInput = Input.GetAxis("Horizontal");

                Debug.Log(axis);
                   
                
                if (Mathf.Abs(axis.x) > Mathf.Abs(axis.y) && Mathf.Abs(axis.x) > 0.5)
                {
                    rollInput = axis.x;
            // Calculate rotation angles based on input and speed
                    roll += rollInput * rollSpeed * Time.deltaTime;
                    roll = Mathf.Clamp(roll, -1, 1);

                    airplanecontroller.setRoll(roll);
           

                }
                else if(Mathf.Abs(axis.y) > Mathf.Abs(axis.x) && Mathf.Abs(axis.y) > 0.5)
                {
                    pitchInput = axis.y;
                    pitch += pitchInput * pitchSpeed * Time.deltaTime;
                    pitch = Mathf.Clamp(pitch, -1, 1);
                    airplanecontroller.setPitch(pitch);

                }

        //rollInput = axis.x;
        //        pitchInput = axis.y;

                //// Calculate rotation angles based on input and speed
                //yaw += yawInput * yawSpeed * Time.deltaTime;
                //yaw = Mathf.Clamp(yaw, -1, 1);
                //airplanecontroller.setYaw(yaw);

                //pitch += pitchInput * pitchSpeed * Time.deltaTime;
                //pitch = Mathf.Clamp(pitch, -1, 1);

                // airplanecontroller.setPitch(pitch);
                   

                // ROLL REMAINS FIXED
                //roll += rollInput * rollSpeed * Time.deltaTime;
                //roll = Mathf.Clamp(roll, -1, 1);

                //airplanecontroller.setRoll(roll);

    }

    }