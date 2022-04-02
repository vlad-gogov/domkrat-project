using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PositionRuchka
{
    UP = 0,
    DOWN = 1
}

public class Ruchka : Selectable
{
    struct StateProperties
    {
        public bool isValidState;
        public ModeSwitch activeSwitcher;
        public Makes direction;
    }

    public PositionRuchka curPosition;

    [SerializeField] TechStand TechStand;
    public Up_part actualDomkratUpPart;
    public Down_part actualDomkratDownPart;

    [SerializeField] private Switch left, right;
    [SerializeField] private Selectable han;

    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        curPosition = PositionRuchka.UP;
    }

    void Update()
    {
        if (!boxCollider.enabled && Singleton.Instance.StateManager.GetState() == State.UP_TPK)
        {
            boxCollider.enabled = true;
        }
        if (boxCollider.enabled && Singleton.Instance.StateManager.GetState() == State.UP_TPK)
        {

        }
    }

    public override void Deselect()
    {
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы взаимодействовать с ручкой.");
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

        if (activeSwitcher.curType == TypeMode.Podem)
        {
            result.direction = Makes.UP;
        }
        else if (activeSwitcher.curType == TypeMode.Opusk)
        {
            result.direction = Makes.DOWN;
        }
        else
        {
            result.direction = Makes.STAY;
        }


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
            if (curPosition == PositionRuchka.UP)
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
                    else if (
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
                if (actualDomkratDownPart.curPosition == Makes.DOWN)
                {
                    if (
                          actualDomkratUpPart.curPosition == Makes.DOWN
                          && state.direction == Makes.UP
                    )
                    {
                        Debug.Log("Up part UP");
                        actualDomkratUpPart.Up(state.activeSwitcher == ModeSwitch.LOADED); // Анимация подъема верхней части домкрата
                    }
                    else if (
                            actualDomkratUpPart.curPosition == Makes.UP
                            && state.direction == Makes.DOWN
                    )
                    {
                        Debug.Log("Up part DOWN");
                        if (TechStand.isSelected)
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите технологическую подставку перед тем как опускать ТПК", Weight = ErrorWeight.HIGH });
                            return;
                        }
                        isSelected = true;
                        actualDomkratUpPart.Down(state.activeSwitcher == ModeSwitch.LOADED); // Анимация опускания верхней части домкрата
                    }
                    //else if (actualDomkratUpPart.curPosition == Makes.DOWN && state.direction == Makes.DOWN)
                    //{
                    //    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нельзя опустить домкрат: он и так в нижнем положении.", Weight = ErrorWeight.MINOR });
                    //    return;
                    //}
                    //else if (actualDomkratUpPart.curPosition == Makes.UP && state.direction == Makes.UP)
                    //{
                    //    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нельзя поднять домкрат: он и так в верхнем положении.", Weight = ErrorWeight.MINOR });
                    //    return;
                    //}
                }
            }
            else if (curPosition == PositionRuchka.DOWN)
            {

            }
        }
        else
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите ручку в рабочее положение", Weight = ErrorWeight.MINOR });
            return;
        }
    }
}
