using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruchka : Selectable
{
    struct StateProperties
    {
        public bool isValidState;
        public bool hasWeightOn;
        public Makes direction;
    }

    [SerializeField] GameObject LeftSwitch;
    [SerializeField] GameObject RightSwtch;
    [SerializeField] GameObject handle;
    [SerializeField] GameObject tpk_and_packet;

    public GameObject actualDomkratUpPart;

    private Up_part m_part_up;
    private Switch left, right;
    private Selectable han;

    Animator up_part_anim;
    Animator tpk_and_packet_anim;

    void Start()
    {
        tpk_and_packet_anim = tpk_and_packet.GetComponent<Animator>();
        up_part_anim = actualDomkratUpPart.GetComponent<Animator>();
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

    StateProperties ComputeState()
    {
        var result = new StateProperties();

        result.isValidState = (
            left.curType == TypeMode.Off || right.curType == TypeMode.Off
        );

        var activeSwitcher = left.curType == TypeMode.Off ? right : left;
        result.direction = activeSwitcher.curType == TypeMode.Podem ? Makes.UP : Makes.DOWN;
        // Левая ручка без груза, правая с грузом
        result.hasWeightOn = left.curType == TypeMode.Off ? true : false;

        return result;
    }

    public override void Select()
    {
        var state = ComputeState();
        if (!state.isValidState)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Конфликтующие режимы работы домкрата", Weight = ErrorWeight.LOW });
            return;
        }

        if (
                actualDomkratUpPart.GetComponent<Up_part>().curPosition == Makes.DOWN
                && state.direction == Makes.UP
                && han.isSelected
        )
        {
            isSelected = true;
            m_part_up.Up(state.hasWeightOn); // Анимация подъема верхней части домкрата
        }

        if (
                actualDomkratUpPart.GetComponent<Up_part>().curPosition == Makes.UP 
                && state.direction == Makes.DOWN
                && han.isSelected
        )
        {
            isSelected = true;
            m_part_up.Down(state.hasWeightOn); // Анимация опускания верхней части домкрата
        }
    }
}
