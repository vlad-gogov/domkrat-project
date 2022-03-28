using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoyal : Selectable
{
    [SerializeField] TechStand TechStand;
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;
    public Domkrat domkrat;

    public override void Deselect()
    {
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        return;
    }

    void Start()
    {
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
                if (domkrat.currentWheelState == WheelState.SOOS)
                {
                    Debug.Log("3");
                    actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToRoyal");
                    domkrat.currentWheelState = WheelState.ROYAL;
                } else
                {
                    Debug.Log("4");
                    actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToSoos");
                    domkrat.currentWheelState = WheelState.SOOS;
                }
            }
        }

    }
}
