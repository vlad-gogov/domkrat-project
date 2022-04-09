using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    TPKMoving movingTPK;

    void Start()
    {
        movingTPK = TPK.TPKObj.transform.parent.parent.GetComponent<TPKMoving>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tpk2")
        {
            Singleton.Instance.StateManager.NextState();
            movingTPK.FinishedMoving();
        }
    }
}
