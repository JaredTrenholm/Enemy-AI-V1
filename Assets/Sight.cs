using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public bool CanSee = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CanSee = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CanSee = false;
        }
    }
}
