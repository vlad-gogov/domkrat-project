using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruchka : Selectable
{
    [SerializeField] GameObject LeftSwitch;
    [SerializeField] GameObject RightSwtch;
    [SerializeField] GameObject handle;

    private Switch left, right;
    private Selectable han;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        left = LeftSwitch.GetComponent<Switch>();
        right = RightSwtch.GetComponent<Switch>();
        han = handle.GetComponent<Selectable>();
    }
    public override void Deselect()
    {
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        return;
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (left.curType == TypeMode.Podem && han.isSelected)
        {
            isSelected = true;
            anim.SetTrigger("Up");
        }
    }
}
