using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haft : Selectable
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public override void Select()
    {
        isSelected = true;
        animator.SetTrigger("Up");
    }

    public override void Deselect()
    {
        isSelected = false;
        animator.SetTrigger("Down");
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы переключить положение ручки (рабочее/не рабочее)");
    }
}
