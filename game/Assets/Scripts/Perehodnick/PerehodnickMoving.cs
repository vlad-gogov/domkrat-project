using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerehodnickMoving : MovingSelect
{

    public override void Moving()
    {
        transform.position = Pointer.transform.position;
        transform.rotation = Pointer.transform.rotation;
    }
}
