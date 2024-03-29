﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozSwitcher : Selectable
{
    Animator downPartAnim;
    BoxCollider boxCol;
    Down_part_rotation down_Part_Rotation;
    private bool isFirst = false;
    public SwitchRoyal switchRoyal;
    public bool isAnim = false;

    public void Start()
    {
        downPartAnim = transform.parent.GetComponent<Animator>();
        boxCol = gameObject.GetComponent<BoxCollider>();
        boxCol.enabled = false;
        down_Part_Rotation = gameObject.transform.parent.GetComponent<Down_part_rotation>();
        switchRoyal = gameObject.transform.parent.GetChild(0).GetComponent<SwitchRoyal>();
    }

    void Update()
    {
        NameState curState = Singleton.Instance.StateManager.GetState();
        if (!isFirst && curState == NameState.CHECK_BREAK_MECHANISM)
        {
            boxCol.enabled = true;
            isFirst = true;
        }
    }

    public void SwitchBoxCollider(bool signal)
    {
        boxCol.enabled = signal;
    }

    public override void Deselect()
    {
        if (isAnim || switchRoyal.isAnim)
        {
            return;
        }
        if (down_Part_Rotation.currentWheelState != WheelState.SOOS)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед тем как взаимодействовать с тормозным механизмом, установите колесный ход в соосное положение", Weight = ErrorWeight.MINOR });
            return;
        }
        isAnim = true;
        downPartAnim.SetTrigger("disableTormozPipka");
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        if (isAnim || switchRoyal.isAnim)
        {
            return;
        }
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы включить тормозной механизм");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы отключить тормозной механзим");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (isAnim || switchRoyal.isAnim)
        {
            return;
        }
        if (down_Part_Rotation.currentWheelState != WheelState.SOOS)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед тем как взаимодействовать с тормозным механизмом, установить колесный ход в соосное положение", Weight = ErrorWeight.MINOR });
            return;
        }
        downPartAnim.SetTrigger("enableTormozPipka");
        isAnim = true;
        isSelected = true;
    }
}
