using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollisionHandler : MonoBehaviour
{

    public Boat boat;
    private void OnCollisionEnter(Collision collision) //Solid collision or trigger? Decide later.
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            boat.Score();
        }

        if (other.gameObject.CompareTag("Land"))
        {
            boat.Crash();
            Vector3 bounceDir = 10 * (boat.transform.position - other.transform.position);
            boat.transform.position += bounceDir;
        }
    }
}
