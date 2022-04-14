using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMech : Selectable
{
    bool isUp = true;

    public override void Select()
    {
        isSelected = true;
        gameObject.GetComponent<Animator>().SetTrigger("Down");
        gameObject.GetComponent<BoxCollider>().enabled = false;
        isUp = false;
    }

    public override void Deselect()
    {
        isSelected = false;
    }

    public override GameObject GetSelectObject()
    {
        return gameObject;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы опустить колесо."); // fix string
    }
}
