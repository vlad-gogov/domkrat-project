using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruchka : Selectable
{
    struct StateProperties
    {
        public bool isValidState;
        public ModeSwitch activeSwitcher;
        public Makes direction;
    }

    [SerializeField] TechStand TechStand;
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;

    [SerializeField] private Switch left, right;
    [SerializeField] private Selectable han;


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
        result.activeSwitcher = activeSwitcher.mode;

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

        if (han.isSelected)
        {
            // Нижняя часть домкарата
            if (TPK.TPKObj.state == StateTPK.UP && state.activeSwitcher == ModeSwitch.WITHOUTLOAD)
            {
                BoxCollider boxCol = TechStand.gameObject.GetComponent<BoxCollider>();
                if (
                        actualDomkratUpPart.curPosition == Makes.UP
                        && actualDomkratDownPart.curPosition == Makes.DOWN
                        && state.direction == Makes.DOWN
                )
                {
                    Debug.Log("1");
                    if (actualDomkratDownPart.Up(TechStand.isSelected)) // Анимация подъема нижней части домкрата
                        boxCol.enabled = false;
                }
                if (
                        actualDomkratUpPart.curPosition == Makes.UP
                        && actualDomkratDownPart.curPosition == Makes.UP
                        && state.direction == Makes.UP
                )
                {
                    Debug.Log("2");
                    if (actualDomkratDownPart.Down(TechStand.isSelected)) // Анимация опускания нижней части домкрата
                        boxCol.enabled = true;
                }
            }

            // Верхняя часть домкарата
            if (
                    actualDomkratUpPart.curPosition == Makes.DOWN
                    && actualDomkratDownPart.curPosition == Makes.DOWN
                    && state.direction == Makes.UP
            )
            {
                actualDomkratUpPart.Up(state.activeSwitcher == ModeSwitch.LOADED); // Анимация подъема верхней части домкрата
            }

            if (
                    actualDomkratUpPart.curPosition == Makes.UP
                    && actualDomkratDownPart.curPosition == Makes.DOWN
                    && state.direction == Makes.DOWN
            )
            {
                if (TechStand.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите технологическую подставку перед тем как опускать ТПК", Weight = ErrorWeight.HIGH });
                    return;
                }
                isSelected = true;
                actualDomkratUpPart.Down(state.activeSwitcher == ModeSwitch.LOADED); // Анимация опускания верхней части домкрата
            }
        }
        else
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите ручку в рабочее положение", Weight = ErrorWeight.MINOR });
            return;
        }
    }
}
