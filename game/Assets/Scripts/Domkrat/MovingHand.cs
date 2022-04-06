using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHand : Selectable
{
    GameObject root;

    void Awake()
    {
        root = gameObject.transform.parent.transform.parent.transform.parent.gameObject;
    }
    public override void Select()
    {
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
    }

    public override GameObject GetSelectObject()
    {
        return root;
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы взять домкрат");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы отпустить домкрат");
        }
    }
}
