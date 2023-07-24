using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;


public class DisableRecentering : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(CenterCamera(framesToSkip: 0));
        //StartCoroutine(CenterCamera(framesToSkip: 20));
        //Debug.Log("Player.instance.transform.position: " + Player.instance.transform.position);
        //Debug.Log("Player.instance.transform.localPosition: " + Player.instance.transform.localPosition);
    }

    private static IEnumerator CenterCamera(int framesToSkip = 0)
    {
        // Wait until SteamVR is Initialized
        while (SteamVR.initializedState == SteamVR.InitializedStates.None || SteamVR.initializedState == SteamVR.InitializedStates.Initializing)
            yield return null;

        // After SteamVR is Initialized, wait for a number of frames
        var frames = 0;
        while (frames < framesToSkip)
        {
            yield return new WaitForEndOfFrame();
            frames += 1;
        }

        // Get the Initial Position of the Player
        var playerPosition = Player.instance.transform.position;
        Debug.Log("playerPosition: " + playerPosition);
        Debug.Log("playerPosition: " + Player.instance.transform.localPosition);

        // Get the Initial Position of the Head Mounted Display (HMD)
        var cameraPosition = Player.instance.hmdTransform.position;


        // Move the Player prefab so that the Camera's position is the same as the Scene View's Player prefab Position
        //Player.instance.transform.position += new Vector3(cameraPosition.x - playerPosition.x, 0f, cameraPosition.z - playerPosition.z) * -1;
        Player.instance.transform.position += new Vector3(cameraPosition.x - playerPosition.x, 0f, cameraPosition.z - playerPosition.z) * -1;

        //Player.instance.transform.position += new Vector3(playerPosition.x, 0f, playerPosition.z);
    }
}
