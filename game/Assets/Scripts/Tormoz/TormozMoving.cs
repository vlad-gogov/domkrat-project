using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WireBuilder;

public class TormozMoving : MovingSelect
{
    GameObject leftConnector, rightConnector;
    GameObject leftAdapter, rightAdapter;
    Wire leftWire, rightWire;

    bool isConnected = false;

    public void Start()
    {
        // мдаааа.....
        leftAdapter = transform.GetChild(0).GetChild(1).gameObject;
        leftWire = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<WireBuilder.Wire>();

        rightAdapter = transform.GetChild(0).GetChild(2).gameObject;
        rightWire = transform.GetChild(0).GetChild(3).GetComponent<WireBuilder.Wire>();
    }

    public override void Moving()
    {
        transform.position = Pointer.transform.position;
        transform.rotation = Pointer.transform.rotation;
        if (leftConnector != null)
        {
            leftAdapter.transform.position = leftConnector.transform.position;
            leftWire.UpdateWire(true);
        } else
        {
            leftAdapter.transform.position = Pointer.transform.position;
        }
        if (rightConnector != null)
        {
            rightAdapter.transform.position = rightConnector.transform.position;
            rightWire.UpdateWire(true);
        } else
        {
            rightAdapter.transform.position = Pointer.transform.position;
        }
    }

    public void ConnectTo(GameObject obj, DomkratType orient)
    {
        if (orient == DomkratType.LEFT)
        {
            leftConnector = obj;
        }
        else if (orient == DomkratType.RIGHT)
        {
            rightConnector = obj;
        }
        if (!isConnected)
        {
            PlayerRay.playerRay.Add(Tormoz.tormoz.gameObject.GetComponent<TormozMoving>());
            isConnected = true;
        }
    }

    public void Disconnect(DomkratType orient)
    {
        if (orient == DomkratType.LEFT)
        {
            leftConnector = null;
        }
        else if (orient == DomkratType.RIGHT)
        {
            rightConnector = null;
        }

        if (leftConnector == null && rightConnector == null)
        {
            PlayerRay.playerRay.Remove(Tormoz.tormoz.gameObject.GetComponent<TormozMoving>());
            isConnected = false;
        }
    }
}
