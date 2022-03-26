using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruchka : Selectable
{
    [SerializeField] GameObject LeftSwitch;
    [SerializeField] GameObject RightSwtch;
    [SerializeField] GameObject handle;
    [SerializeField] GameObject tpk_and_packet;

    private Up_part m_part_up;
    private Switch left, right;
    private Selectable han;

    Animator up_part_anim;
    Animator tpk_and_packet_anim;

    void Start()
    {
        tpk_and_packet_anim = tpk_and_packet.GetComponent<Animator>();
        up_part_anim = GetComponent<Animator>();
        left = LeftSwitch.GetComponent<Switch>();
        right = RightSwtch.GetComponent<Switch>();
        han = handle.GetComponent<Selectable>();
        m_part_up = gameObject.GetComponentInParent<Up_part>();
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
        if (((left.curType == TypeMode.Podem && right.curType == TypeMode.Off) || 
        (right.curType == TypeMode.Podem && left.curType == TypeMode.Off)) && han.isSelected)
        {
            isSelected = true;
            m_part_up.Up();
            up_part_anim.SetTrigger("Up");
            tpk_and_packet_anim.SetTrigger("Up");
        }

        if (((right.curType == TypeMode.Opusk && left.curType == TypeMode.Off) || 
        (left.curType == TypeMode.Opusk && right.curType == TypeMode.Off)) && han.isSelected)
        {
            isSelected = true;
            m_part_up.Down();
            up_part_anim.SetTrigger("Down");
        }
    }
}
