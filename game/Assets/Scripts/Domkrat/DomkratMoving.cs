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
        RotateWheelForUpdate(Input.GetAxis("Vertical") * speedRotation * Time.deltaTime);
    }

    public void RotateWheelForUpdate(float angle, bool backWheel = true)
    {
        angle = -angle;
        LeftWheel.transform.Rotate(Vector3.up, angle);
        RightWheel.transform.Rotate(Vector3.up, angle);
        if (backWheel)
        {
            BackWheel.transform.Rotate(Vector3.forward, angle);
        }
    }

    public IEnumerator RotateWheel(float angle, float speed, bool isBack = false)
    {
        float signed = angle >= 0 ? 1 : -1;

        for (float t = 0; t <= Mathf.Abs(angle); t += speed * Time.deltaTime)
        {
            float temp = speed * signed * Time.deltaTime;
            RotateWheelForUpdate(temp, false);
            yield return null;
        }
        if (isBack)
        {
            for (float t = 0; t <= Mathf.Abs(angle); t += speed * Time.deltaTime)
            {
                float temp = speed * signed * Time.deltaTime;
                RotateWheelForUpdate(-temp, false);
                yield return null;
            }
        }
    }

    public void OffCooliderWheel()
    {
        LeftWheel.GetComponent<SphereCollider>().enabled = false;
        RightWheel.GetComponent<SphereCollider>().enabled = false;
        BackWheel.GetComponent<SphereCollider>().enabled = false;
    }
}
