using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoyal : Selectable
{
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;

    private BoxCollider box;

    void Start()
    {
        box = gameObject.GetComponent<BoxCollider>();
        box.enabled = false;
    }

    void Update()
    {
        if (actualDomkratDownPart.curPosition == Makes.UP && actualDomkratUpPart.curPosition == Makes.UP)
        {
            box.enabled = true;
        }
    }

    public override void Deselect()
    {
        isSelected = false;
        Singleton.Instance.UIManager.ClearHelperText();
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы переключить соосно/рояльное положение колёс.");
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (TPK.TPKObj.state == StateTPK.UP)
        {

            if (
                    actualDomkratUpPart.curPosition == Makes.UP
                    && actualDomkratDownPart.curPosition == Makes.UP
            )
            {
                if (actualDomkratDownPart.rotation_down_part.currentWheelState == WheelState.SOOS)
                {
                    actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToRoyal");
                    actualDomkratDownPart.rotation_down_part.currentWheelState = WheelState.ROYAL;
                } else
                {
                    actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToSoos");
                    actualDomkratDownPart.rotation_down_part.currentWheelState = WheelState.SOOS;
                }
                actualDomkratDownPart.rotation_down_part.ChangeDir();
            }
        }

    }
}
