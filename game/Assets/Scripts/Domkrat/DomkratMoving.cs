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
        //transform.rotation.Set(Pointer.transform.rotation.x, Pointer.transform.rotation.y, transform.rotation.z, Pointer.transform.rotation.w);
    }

    public override void Moving()
    {
        Vector3 position = new Vector3(Pointer.transform.position.x, transform.position.y, Pointer.transform.position.z);
        transform.position = position;
    }
}
