using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WireBuilder;

public class TormozMoving : MovingSelect
{
    Vector3 defaultLeftAdapter, defaultRightAdapter;
    GameObject leftConnector, rightConnector;
    GameObject leftAdapter, rightAdapter;
    Wire leftWire, rightWire;

    bool isConnected = false;

    public void Start()
    {
        // мдаааа.....
        leftAdapter = transform.GetChild(0).GetChild(1).gameObject;
        defaultLeftAdapter = leftAdapter.transform.position;
        leftWire = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<WireBuilder.Wire>();

        rightAdapter = transform.GetChild(0).GetChild(2).gameObject;
        defaultRightAdapter = rightAdapter.transform.position;
        rightWire = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<WireBuilder.Wire>();
    }

    void Update()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.CHECK_BREAK_MECHANISM)
        {
            if (leftConnector != null)
            {
                leftAdapter.transform.position = leftConnector.transform.position;
                leftAdapter.transform.rotation = leftConnector.transform.rotation;
                leftWire.UpdateWire(true);
            }
            if (rightConnector != null)
            {
                rightAdapter.transform.position = rightConnector.transform.position;
                rightAdapter.transform.rotation = rightConnector.transform.rotation;
                rightWire.UpdateWire(true);
            }
        }
    }

    public override void Moving()
    {
        transform.position = Pointer.transform.position;
        transform.rotation = Pointer.transform.rotation;
        defaultLeftAdapter = leftAdapter.transform.position;
        defaultRightAdapter = rightAdapter.transform.position;
        if (leftConnector != null)
        {
            leftAdapter.transform.position = leftConnector.transform.position;
            leftAdapter.transform.rotation = leftConnector.transform.rotation;
            leftWire.UpdateWire(true);
        }
        else
        {
            leftAdapter.transform.position = defaultLeftAdapter;
        }

        if (rightConnector != null)
        {
            rightAdapter.transform.position = rightConnector.transform.position;
            rightAdapter.transform.rotation = rightConnector.transform.rotation;
            rightWire.UpdateWire(true);
        }
        else
        {
            rightAdapter.transform.position = defaultRightAdapter;
        }
    }

    public bool ConnectTo(GameObject obj, DomkratType orient, GameObject pointToTormoz)
    {
        // TODO
        if (!isConnected)
        {
            NameState curState = Singleton.Instance.StateManager.GetState();
            if (curState == NameState.CHECK_BREAK_MECHANISM)
            {
                isConnected = true;
                Tormoz.tormoz.gameObject.transform.position = pointToTormoz.transform.position;
                Tormoz.tormoz.gameObject.transform.rotation = pointToTormoz.transform.rotation;
            }
            else
            {
                PlayerRay.playerRay.Add(Tormoz.tormoz.gameObject.GetComponent<TormozMoving>());
            }
        }
        else
        {
            // ЗАНЯТ УЖЕ
        }
        if (orient == DomkratType.LEFT)
        {
            if (leftConnector != null)
            {
                return false;
            }
            leftConnector = obj;
            defaultLeftAdapter = leftAdapter.transform.position;
            leftAdapter.transform.position = leftConnector.transform.position;
            leftWire.UpdateWire(true);
        }
        else if (orient == DomkratType.RIGHT)
        {
            if (rightConnector != null)
            {
                return false;
            }
            rightConnector = obj;
            defaultRightAdapter = rightAdapter.transform.position;
            rightAdapter.transform.position = rightConnector.transform.position;
            rightWire.UpdateWire(true);
        }

        return true;
    }

    public void Disconnect(DomkratType orient)
    {
        if (orient == DomkratType.LEFT)
        {
            leftConnector = null;
            leftAdapter.transform.position = defaultLeftAdapter;
            leftWire.UpdateWire(true);
        }
        else if (orient == DomkratType.RIGHT)
        {
            rightConnector = null;
            rightAdapter.transform.position = defaultRightAdapter;
            rightWire.UpdateWire(true);
        }

        if (leftConnector == null && rightConnector == null)
        {
            PlayerRay.playerRay.Remove(Tormoz.tormoz.gameObject.GetComponent<TormozMoving>());
            isConnected = false;
            Tormoz.tormoz.gameObject.SetActive(false);
        }
    }
}
