using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO:
/// ������� ��� �������� �����
/// </summary>

public class DomkratMoving : MovingSelect
{
    public override void Moving()
    {
        Vector3 position = new Vector3(Pointer.transform.position.x, transform.position.y, Pointer.transform.position.z);
        transform.position = position;
        transform.rotation = Pointer.transform.rotation;
    }
}
