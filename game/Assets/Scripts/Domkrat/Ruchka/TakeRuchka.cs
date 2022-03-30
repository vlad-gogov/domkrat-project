using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeRuchka : Selectable
{
     [SerializeField] GameObject down_ruchka;
    public override void Deselect()
    {
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы снять ручку с домкрата");
    }

    public override GameObject GetSelectObject()
    {
        return gameObject.transform.parent.gameObject;
    }

    public override void Select()
    {
        isSelected = true;
        down_ruchka.GetComponent<Collider>().enabled = false;
    }

}
