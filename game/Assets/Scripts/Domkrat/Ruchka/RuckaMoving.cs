using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuckaMoving : MovingSelect
{
    public bool isMoving = true;
    public override void Moving()
    {
        if (isMoving)
        {
            Vector3 position = new Vector3(Pointer.transform.position.x, Pointer.transform.position.y, Pointer.transform.position.z);
            transform.position = position;
            transform.rotation = Pointer.transform.rotation;
        }
    }
}
