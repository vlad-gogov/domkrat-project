using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO:
/// Условие для кручения колес
/// </summary>

public class DomkratMoving : MovingSelect
{
    private float SpeedRotation = 80f;
    private Vector3 prev;
    [SerializeField] private GameObject LeftWheel;
    [SerializeField] private GameObject RightWheel;
    [SerializeField] private GameObject BackWheel;
    public override void Moving()
    {
        Vector3 position = new Vector3(Pointer.transform.position.x, transform.position.y, Pointer.transform.position.z);
        transform.position = position;
        transform.rotation = Pointer.transform.rotation;
        float temp = position.x - prev.x;
        if (temp != 0)
        {
            float rot = (temp > 0 ? SpeedRotation : -SpeedRotation) * Time.deltaTime;
            LeftWheel.transform.Rotate(0f, rot, 0f);
            RightWheel.transform.Rotate(0f, rot, 0f);
            BackWheel.transform.Rotate(0f, 0f, -rot);
        }
        prev = position;
    }
}
