using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;
using Valve.VR;

using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject aircraft;
    private Rigidbody aircraftrb;
    private AirplaneController airplaneController;

    private GameObject menu_tutorial;

    private int init_altitude;

    private bool tutorial_mode;

    [SerializeField]

    private int threshold;


    [SerializeField]
    private GameObject Tutorial_Text;

    [SerializeField]
    private GameObject Metrics_Text;


    // Start is called before the first frame update
    void Start()
    {

        this.aircraftrb = aircraft.GetComponent<Rigidbody>();
        this.airplaneController = aircraft.GetComponent<AirplaneController>();
        this.menu_tutorial = this.gameObject;

        this.init_altitude = (int) this.aircraft.transform.position.y;
        this.tutorial_mode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tutorial_mode)
        {
            int velocity = (int)this.aircraftrb.velocity.magnitude;
            if (velocity == 0)
            {
                //this.menu_tutorial.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "aldo";
                this.Tutorial_Text.GetComponent<TextMeshProUGUI>().text = "Click the left trigger to increase the aircraft velocity!";
            }
            else if (velocity > 0 && velocity < threshold)
            {
                this.Tutorial_Text.GetComponent<TextMeshProUGUI>().text = $"Keep clicking the left trigger until you reach {threshold} m/s!";

            }
            else
            {
                this.Tutorial_Text.GetComponent<TextMeshProUGUI>().text = "Hold the touch in the bottom left trackpad to raise the plane!";

            }

            int curr_altitude = (int)aircraft.transform.position.y;

            if (curr_altitude > this.init_altitude + 20)
            {
                this.menu_tutorial.GetComponentInChildren<TextMeshProUGUI>().text = "Enjoy your flight, orienting with the left trackpad! In any moment, press the right controller Grip !";
                this.tutorial_mode = false;
                StartCoroutine(Destroy_Tutorial(5.0f));
            }
        }

        this.Metrics_Text.GetComponent<Text>().text = "V: " + ((int)this.airplaneController.transform.GetComponent<Rigidbody>().velocity.magnitude).ToString("D3") + " m/s\n";
        this.Metrics_Text.GetComponent<Text>().text += "A: " + ((int)this.airplaneController.transform.position.y).ToString("D4") + " m\n";
        this.Metrics_Text.GetComponent<Text>().text += "T: " + (int)(this.airplaneController.GetThrustPercecent() * 100) + "%\n";
        this.Metrics_Text.GetComponent<Text>().text += this.airplaneController.GetBrakesTorque() > 0 ? "B: ON" : "B: OFF";


    }



    private IEnumerator Destroy_Tutorial(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DestroyImmediate(this.Tutorial_Text.transform.parent.gameObject);

    }


}
