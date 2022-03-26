using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomkratMoving : MovingSelect
{
    public void Rotate(GameObject Pointer, float angel)
    {
        transform.RotateAround(Pointer.transform.position, Vector3.up, angel);
    }

    public void StartMoving(GameObject Pointer)
    {
        transform.rotation = Pointer.transform.rotation;
    }

    public override void Moving()
    {
        Vector3 position = new Vector3(Pointer.transform.position.x, transform.position.y, Pointer.transform.position.z);
        transform.position = position;
    }
}
