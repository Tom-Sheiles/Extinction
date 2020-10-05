using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            other.SendMessage("CompleteObjective", checkpointNumber);
        }
    }
}
