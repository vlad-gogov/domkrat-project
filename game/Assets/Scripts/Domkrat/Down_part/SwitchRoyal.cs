using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoyal : Selectable
{
    struct StateProperties
    {
        public bool isValidState;
        public bool hasWeightOn;
        public Makes direction;
    }

    [SerializeField] TechStand TechStand;
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;


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
        // ?????? ????? ?????????
        if (TPK.TPKObj.state == StateTPK.UP)
        {

            if (
                    actualDomkratUpPart.curPosition == Makes.UP
                    && actualDomkratDownPart.curPosition == Makes.UP
            )
            {
                Debug.Log("3");
                isSelected = true;
                actualDomkratDownPart.GetComponent<Animator>().SetTrigger("ToRoyal");
            }
        }

    }
}
