using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozSwitcher : Selectable
{
    Animator downPartAnim;

    public void Start()
    {
        downPartAnim = transform.parent.GetComponent<Animator>();
    }

    public override void Deselect()
    {
        Debug.Log("disable PIPKA!!!");
        downPartAnim.SetTrigger("disableTormozPipka");
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы активировать пипку тормоза");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы деактивировать пипку тормоза");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        Debug.Log("enable PIPKA!!!");
        downPartAnim.SetTrigger("enableTormozPipka");
        isSelected = true;
    }
}
