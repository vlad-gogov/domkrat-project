using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoyal : Selectable
{
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;

    private Ruchka ruchka;
    private BoxCollider box;
    private TormozSwitcher tormozSwitcher;

    private bool isSoos = true;
    public bool isAnim = false;

    void Start()
    {
        box = gameObject.GetComponent<BoxCollider>();
        box.enabled = false;
        ruchka = actualDomkratUpPart.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Ruchka>();
        tormozSwitcher = gameObject.transform.parent.GetChild(1).GetComponent<TormozSwitcher>();
    }

    void Update()
    {
        if (actualDomkratDownPart.curPosition == Makes.UP && actualDomkratUpPart.curPosition == Makes.UP && Singleton.Instance.StateManager.GetState() > NameState.CHECK_BREAK_MECHANISM)
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }
    }

    public override void Deselect()
    {
        isSelected = false;
        Singleton.Instance.UIManager.ClearHelperText();
    }

    public override void GetInfoMouse()
    {
        if (ruchka.isUse || tormozSwitcher.isAnim)
        {
            return;
        }
        if (isSoos)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы переключить колеса в рояльное положение.");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы переключить колеса в соосное положение.");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (TPK.TPKObj.state == StateTPK.UP)
        {
            if (ruchka.isUse || tormozSwitcher.isAnim)
            {
                return;
            }
            if (
                    actualDomkratUpPart.curPosition == Makes.UP
                    && actualDomkratDownPart.curPosition == Makes.UP
            )
            {
                if (actualDomkratDownPart.rotation_down_part.currentWheelState == WheelState.SOOS)
                {
                    actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToRoyal");
                    actualDomkratDownPart.rotation_down_part.currentWheelState = WheelState.ROYAL;
                    isSoos = false;
                } 
                else
                {
                    actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToSoos");
                    actualDomkratDownPart.rotation_down_part.currentWheelState = WheelState.SOOS;
                    isSoos = true;
                }
                isAnim = true;
                ruchka.StopInteraption();
                actualDomkratDownPart.rotation_down_part.ChangeDir();
            }
        }
    }
}
