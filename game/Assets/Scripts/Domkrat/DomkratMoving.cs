using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomkratMoving : MovingSelect
{
    private float speedRotation = 100f;
    [SerializeField] private GameObject LeftWheel;
    [SerializeField] private GameObject RightWheel;
    [SerializeField] private GameObject BackWheel;
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
        RotateWheel(Input.GetAxis("Vertical") * speedRotation * Time.deltaTime);
    }

    public void RotateWheel(float angle, bool backWheel = true)
    {
        LeftWheel.transform.Rotate(Vector3.up, -angle);
        RightWheel.transform.Rotate(Vector3.up, -angle);
        if (backWheel)
        {
            BackWheel.transform.Rotate(Vector3.forward, angle);
        }
    }
}
