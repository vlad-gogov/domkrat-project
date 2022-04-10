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
    private Animator anim;

    [SerializeField] BoxCollider push;

    // Переменные для проверки поворотного механизма
    private bool isRight = false;
    private bool isLeft = false;
    private bool isCount = false;

    private bool isUse = false;

    void Start()
    {
        curPosition = PositionRuchka.UP;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.CHECK_TURING_MACHANISM)
        {
            if (isUse)
            {
                push.enabled = false;
            }
            else
            {
                push.enabled = true;
            }
            if (push.enabled && isLeft && isRight && curPosition == PositionRuchka.UP)
            {
                push.enabled = false;
                Singleton.Instance.StateManager.NextState();
                Singleton.Instance.StateManager.ruchkaIsUp++;
                isCount = true;
            }
        }
        else if (!isCount && Singleton.Instance.StateManager.GetState() > NameState.CHECK_TURING_MACHANISM)
        {
            if (curPosition == PositionRuchka.UP)
            {
                push.enabled = false;
                Singleton.Instance.StateManager.ruchkaIsUp++;
                isCount = true;
            }
        }
    }

    public override void Deselect()
    {
        isUse = false;
    }

    public override void GetInfoMouse()
    {
        if (!isUse)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы взаимодействовать с ручкой.");
        }
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

    public void StopInteraption()
    {
        isUse = true;
    }

    public override void Select()
    {
        if (isUse)
        {
            return;
        }
        var state = ComputeState();
        if (!state.isValidState)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Конфликтующие режимы работы домкрата", Weight = ErrorWeight.LOW });
            Deselect();
            return;
        }
        if (han.isSelected)
        {
            if (curPosition == PositionRuchka.UP)
            {
                if (Singleton.Instance.StateManager.GetState() == NameState.CHECK_TURING_MACHANISM)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед подъемом ТПК необходимо проверить поворотный механизм домкрата", Weight = ErrorWeight.LOW });
                    return;
                }
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
                        if (actualDomkratDownPart.Up(TechStand.isSelected)) // Анимация подъема нижней части домкрата
                        {
                            boxCol.enabled = false;
                            return;
                        }
                    }
                    else if (
                            actualDomkratUpPart.curPosition == Makes.UP
                            && actualDomkratDownPart.curPosition == Makes.UP
                            && state.direction == Makes.UP
                    )
                    {
                        if (actualDomkratDownPart.Down(TechStand.isSelected))  // Анимация опускания нижней части домкрата
                        {
                            boxCol.enabled = true;
                            return;
                        }
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
                        actualDomkratUpPart.Up(state.activeSwitcher == ModeSwitch.LOADED); // Анимация подъема верхней части домкрата
                        return;
                    }
                    else if (
                            actualDomkratUpPart.curPosition == Makes.UP
                            && state.direction == Makes.DOWN
                    )
                    {
                        if (TPK.TPKObj.CheckStand())
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите технологическую подставку перед тем как опускать ТПК", Weight = ErrorWeight.HIGH });
                            return;
                        }
                        actualDomkratUpPart.Down(state.activeSwitcher == ModeSwitch.LOADED); // Анимация опускания верхней части домкрата
                        return;
                    }
                }
            }
            else if (curPosition == PositionRuchka.DOWN)
            {
                if (
                        TPK.TPKObj.state == StateTPK.DOWN 
                        && state.activeSwitcher == ModeSwitch.LOADED 
                        && actualDomkratUpPart.curPosition == Makes.DOWN
                        && actualDomkratDownPart.curPosition == Makes.DOWN
                )
                {
                    if (state.direction == Makes.UP)
                    {
                        anim.SetTrigger("Up");
                        actualDomkratDownPart.rotation_down_part.RotateDownPart(90f, true, 0.5f);
                        isRight = true;
                        return;
                    }
                    else if (state.direction == Makes.DOWN)
                    {
                        anim.SetTrigger("Up");
                        actualDomkratDownPart.rotation_down_part.RotateDownPart(-90f, true, 0.5f);
                        isLeft = true;
                        return;
                    }
                }
            }
        }
        else
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите ручку в рабочее положение", Weight = ErrorWeight.MINOR });
            return;
        }
    }
}
