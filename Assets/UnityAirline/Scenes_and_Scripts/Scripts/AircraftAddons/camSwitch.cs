using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Cinemachine;
using Valve.VR;

public class camSwitch : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    private bool isSteamVRCameraActive = true;
    private bool switch_allowed = true;

    void Start()
    {
        if (!SteamVR.active)
        {
            Debug.Log("SteamVR is not installed or not running.");
            this.isSteamVRCameraActive = false;
            // playerCamera now is the one provided by the fallbackobjects
            this.playerCamera = GameObject.Find("FallbackObjects");
            this.switch_allowed = false; // we don't allow camera switch in first person without VR-compatible device
        }
        else
        {
            var hmd = SteamVR.instance.hmd;
            if (hmd != null && hmd.GetTrackedDeviceClass(0) == ETrackedDeviceClass.HMD)
            {
                Debug.Log("Your Headset is connected.");
            }
            else
            {
                this.isSteamVRCameraActive = false;
                this.switch_allowed = false;
                Debug.Log("Your Headset is not connected.");
            }
        }

        this.playerCamera.SetActive(this.isSteamVRCameraActive);
        this.mainCamera.SetActive(!this.isSteamVRCameraActive);
        this.virtualCamera.gameObject.SetActive(!this.isSteamVRCameraActive);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && this.switch_allowed)
        {
            this.SetSwitchCamera();
        }
    
    }

    public void SetSwitchCamera()
    {

        this.isSteamVRCameraActive = !this.isSteamVRCameraActive;

        if (isSteamVRCameraActive)
        {
            // Disattiva la telecamera di Cinemachine e attiva la telecamera di SteamVR
            this.playerCamera.SetActive(true);
            this.mainCamera.SetActive(false);
            this.virtualCamera.gameObject.SetActive(false);
        }
        else
        {
            // Disattiva la telecamera di SteamVR e attiva la telecamera di Cinemachine
            this.playerCamera.SetActive(false);
            this.mainCamera.SetActive(true);
            this.virtualCamera.gameObject.SetActive(true);
        }
    }
    
}
