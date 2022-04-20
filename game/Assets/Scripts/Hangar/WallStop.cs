using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStop : MonoBehaviour
{
    TPKMoving movingTPK;

    void Start()
    {
        movingTPK = TPK.TPKObj.transform.root.GetComponent<TPKMoving>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tpk2")
        {
            movingTPK.isStop = true;
        }
    }
}
