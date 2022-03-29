using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_fixator : Selectable
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override void Deselect()
    {
        isSelected = false;
        anim.SetTrigger("Fixator_off");
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы разблокировать фиксатор поворота");
        } else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы заблокировать фиксатор поворота");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        isSelected = true;
        anim.SetTrigger("Fixator_on");
    }
}
