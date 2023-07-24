using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTakeOffAudio : MonoBehaviour
{
    public float takeoffSpeed = 0.25f; // la velocità di decollo desiderata
    public AudioSource audioSource; // il componente AudioSource da far partire

    private bool hasTakenOff = false; // flag per controllare se l'aereo ha già decollato

    void FixedUpdate()
    {
        // Verificare se l'aereo ha già decollato
        if (!hasTakenOff)
        {
            // Verificare se l'aereo ha superato la velocità di decollo
            if (GetComponent<Rigidbody>().velocity.magnitude > takeoffSpeed)
            {
                audioSource.Play(); // far partire l'audio
                hasTakenOff = true; // impostare il flag di decollo
            }
        }
    }
}

