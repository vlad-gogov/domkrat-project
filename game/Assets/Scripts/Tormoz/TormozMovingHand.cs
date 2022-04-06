using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozMovingHand : Selectable
{
    Animator anim;

    void Start()
    {
        anim = gameObject.transform.parent.gameObject.GetComponent<Animator>();
    }

    public override void Deselect()
    {
        isSelected = false;
        Debug.Log("Triggering 'Stop'");
        anim.SetTrigger("Stop");
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтоб прижать ручку к корпусу");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтоб отжать ручку от корпуса");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        isSelected = true;
        Debug.Log("Triggering 'Continue'");
        anim.SetTrigger("Continue");
    }
}
