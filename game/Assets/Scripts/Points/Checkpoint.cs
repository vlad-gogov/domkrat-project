using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
   [SerializeField] TPKMoving movingTPK;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tpk2")
        {
            Singleton.Instance.StateManager.NextState();
            movingTPK.FinishedMoving();
        }
    }
}
