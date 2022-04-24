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

    public Domkrat batya;

    [SerializeField] BoxCollider push;

    // Переменные для проверки поворотного механизма
    private bool isRight = false;
    private bool isLeft = false;
    private bool isCount = false;

    public bool isUse = false;

    void Start()
    {
        batya = gameObject.transform.parent.parent.parent.parent.GetComponent<Domkrat>();
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
                NameState curState = Singleton.Instance.StateManager.GetState();
                if (curState == NameState.CHECK_TURING_MACHANISM)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед подъемом ТПК необходимо проверить поворотный механизм домкрата", Weight = ErrorWeight.MEDIUM });
                    return;
                }

                // if (curState == NameState.CHECK_DOMKRATS || curState == NameState.SET_DOMKRATS)
                if (batya.isAttachedToStoika)
                {
                    if (actualDomkratUpPart.curPosition == Makes.UP)
                    {
                        if (
                            actualDomkratDownPart.curPosition == Makes.UP
                            && state.direction == Makes.UP
                        )
                        {
                            actualDomkratDownPart.Down(false, state.activeSwitcher == ModeSwitch.LOADED);
                            return;
                        }
                        else if (
                            actualDomkratDownPart.curPosition == Makes.DOWN
                            && state.direction == Makes.DOWN
                        )
                        {
                            actualDomkratDownPart.Up(false, state.activeSwitcher == ModeSwitch.LOADED);
                            return;
                        }
                        else if (
                            actualDomkratDownPart.curPosition == Makes.DOWN
                            && state.direction == Makes.UP
                        )
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нижняя часть уже внизу", Weight = ErrorWeight.MINOR });
                            return;
                        }
                        else if (
                            actualDomkratDownPart.curPosition == Makes.UP
                            && state.direction == Makes.DOWN
                        )
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нижняя часть уже вверху", Weight = ErrorWeight.MINOR });
                            return;
                        }
                    }
                }

                // Нижняя часть домкарата
                if (TPK.TPKObj.state == StateTPK.UP)
                {
                    BoxCollider boxCol = TechStand.gameObject.GetComponent<BoxCollider>();
                    if (state.activeSwitcher == ModeSwitch.WITHOUTLOAD)
                    {

                        if (
                                actualDomkratDownPart.curPosition == Makes.DOWN
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
                                actualDomkratDownPart.curPosition == Makes.UP
                                && state.direction == Makes.UP
                        )
                        {
                            if (actualDomkratDownPart.Down(TechStand.isSelected))  // Анимация опускания нижней части домкрата
                            {
                                return;
                            }
                        }
                        else if (
                                actualDomkratDownPart.curPosition == Makes.UP
                                && state.direction == Makes.DOWN
                        )
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нижняя часть домкрата уже вверху", Weight = ErrorWeight.MINOR });
                            return;
                        }
                        else if (
                                actualDomkratDownPart.curPosition == Makes.DOWN
                                && state.direction == Makes.UP
                        )
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нижняя часть домкрата уже внизу", Weight = ErrorWeight.MINOR });
                            return;
                        }
                    }
                    else if (state.activeSwitcher == ModeSwitch.LOADED)
                    {
                        if (
                                actualDomkratDownPart.curPosition == Makes.UP
                                && state.direction == Makes.UP
                        )
                        {
                            if (actualDomkratDownPart.Down(TechStand.isSelected))  // Анимация опускания нижней части домкрата
                            {
                                return;
                            }
                        }
                        else if (
                                actualDomkratDownPart.curPosition == Makes.UP
                                && state.direction == Makes.DOWN
                        )
                        {
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нижняя часть домкрата уже вверху", Weight = ErrorWeight.MINOR });
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
                            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите технологическую подставку перед тем, как опускать ТПК", Weight = ErrorWeight.MEDIUM });
                            return;
                        }
                        actualDomkratUpPart.Down(state.activeSwitcher == ModeSwitch.LOADED); // Анимация опускания верхней части домкрата
                        return;
                    }
                    else if (
                            actualDomkratUpPart.curPosition == Makes.DOWN
                            && state.direction == Makes.DOWN
                    )
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Домкрат уже в нижнем положении", Weight = ErrorWeight.MINOR });
                        return;
                    }
                    else if (
                            actualDomkratUpPart.curPosition == Makes.UP
                            && state.direction == Makes.UP
                    )
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Домкрат уже в верхнем положении", Weight = ErrorWeight.MINOR });
                        return;
                    }
                }
            }
            else if (curPosition == PositionRuchka.DOWN)
            {
                if (
                        TPK.TPKObj.state == StateTPK.DOWN
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
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите ручку в рабочее положение", Weight = ErrorWeight.LOW });
            return;
        }
    }
}
