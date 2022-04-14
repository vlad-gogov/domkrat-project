using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPerehodnick : PlaceForSet
{
    GameObject Point;

    void Start()
    {
        Point = gameObject.transform.GetChild(0).gameObject;
    }

    public override void GetInfoMouse(GameObject gameObject)
    {
        if (gameObject.tag == "Perehodnick")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы установить переходник на стойку");
        }
    }

    public override bool SetItem(GameObject gameObject)
    {
        if (gameObject.tag == "Perehodnick")
        {
            gameObject.transform.position = Point.transform.position;
            gameObject.transform.rotation = Point.transform.rotation;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Perehodnick>().isConnect = true;
            return true;
        }
        return false;
    }
}
