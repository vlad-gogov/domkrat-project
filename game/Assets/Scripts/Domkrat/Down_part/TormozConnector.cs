using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozConnector : Selectable
{
    DomkratType type;

    public void Start()
    {
        type = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Domkrat>().type;
    }

    public override void Deselect()
    {
        Debug.Log("Deselecting pipka...");
        Tormoz.tormoz.gameObject.GetComponent<TormozMoving>().Disconnect(type);
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы подключить тормоз");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы отключить тормоз");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        Tormoz.tormoz.gameObject.GetComponent<TormozMoving>().ConnectTo(gameObject, type);
        isSelected = true;
    }
}
