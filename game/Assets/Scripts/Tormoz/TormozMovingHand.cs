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
        anim.SetTrigger("Stop");
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("������� ���, ���� ������� ����� � �������");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("������� ���, ���� ������ ����� �� �������");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        isSelected = true;
        anim.SetTrigger("Continue");
    }
}
