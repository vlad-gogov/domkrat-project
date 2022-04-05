using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozConnector : Selectable
{
    bool isActivated = false;
    DomkratType type;

    public void Start()
    {
        type = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Domkrat>().type;
    }

    public override void Deselect()
    {
        Debug.Log("Deselecting pipka...");
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        if (!isActivated)
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
        if (!isActivated)
        {
            Tormoz.tormoz.gameObject.GetComponent<TormozMoving>().ConnectTo(gameObject, type);
            isActivated = true;
        }
        else
        {
            Tormoz.tormoz.gameObject.GetComponent<TormozMoving>().Disconnect(type);
            isActivated = false;
        }
        
        isSelected = true;
    }
}
