using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeRuchka : Selectable
{
    public override void Deselect()
    {
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы снять ручку");
    }

    public override GameObject GetSelectObject()
    {
        return gameObject.transform.parent.gameObject;
    }

    public override void Select()
    {
        isSelected = true;
    }

}
