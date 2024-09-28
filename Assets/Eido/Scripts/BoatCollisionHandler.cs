using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollisionHandler : MonoBehaviour
{
    public Boat boat;
    private void OnCollisionEnter(Collision collision) //Solid collision or trigger? Decide later.
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            boat.Crash();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            boat.Score();
        }
    }
}
