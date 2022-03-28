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

    [SerializeField] TechStand TechStand;
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;

    private Switch left, right;
    private Selectable han;


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
        left = LeftSwitch.GetComponent<Switch>();
        right = RightSwtch.GetComponent<Switch>();
        han = handle.GetComponent<Selectable>();
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

        // Нижняя часть домкарата
        if (TPK.TPKObj.state == StateTPK.UP)
        {

            if (
                    actualDomkratUpPart.curPosition == Makes.UP
                    && actualDomkratDownPart.curPosition == Makes.DOWN
                    && state.direction == Makes.DOWN
                    && han.isSelected
            )
            {
                Debug.Log("1");
                if (actualDomkratDownPart.Up(TechStand.isSelected)) // Анимация подъема нижней части домкрата
                    TechStand.enabled = false;
            }
            if (
                    actualDomkratUpPart.curPosition == Makes.UP
                    && actualDomkratDownPart.curPosition == Makes.UP
                    && state.direction == Makes.UP
                    && han.isSelected
            )
            {
                Debug.Log("2");
                if (actualDomkratDownPart.Down(TechStand.isSelected)) // Анимация подъема нижней части домкрата
                    TechStand.enabled = true;
            }
        }

        // Верхняя часть домкарата
        if (
                actualDomkratUpPart.curPosition == Makes.DOWN
                && actualDomkratDownPart.curPosition == Makes.DOWN
                && state.direction == Makes.UP
                && han.isSelected
        )
        {
            actualDomkratUpPart.Up(state.hasWeightOn); // Анимация подъема верхней части домкрата
        }

        if (
                actualDomkratUpPart.curPosition == Makes.UP
                && actualDomkratDownPart.curPosition == Makes.DOWN
                && state.direction == Makes.DOWN
                && han.isSelected
        )
        {
            if (TechStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите технологическую подставку перед тем как опускать ТПК", Weight = ErrorWeight.HIGH });
            }
            isSelected = true;
            actualDomkratUpPart.Down(state.hasWeightOn); // Анимация опускания верхней части домкрата
        }
    }
}
