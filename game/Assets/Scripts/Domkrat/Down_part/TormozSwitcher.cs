using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozSwitcher : Selectable
{
    Animator downPartAnim;
    BoxCollider boxCol;
    Down_part_rotation down_Part_Rotation;

    public void Start()
    {
        downPartAnim = transform.parent.GetComponent<Animator>();
        boxCol = gameObject.GetComponent<BoxCollider>();
        boxCol.enabled = false;
        down_Part_Rotation = gameObject.transform.parent.GetComponent<Down_part_rotation>();
    }

    void Update()
    {
        NameState curState = Singleton.Instance.StateManager.GetState();
        if (!boxCol.enabled && curState == NameState.CHECK_BREAK_MECHANISM)
        {
            boxCol.enabled = true;
        }
    }

    public override void Deselect()
    {
        if (down_Part_Rotation.currentWheelState != WheelState.SOOS)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед тем как взаимодействовать с тормозным механизмом, установить колесный ход в соосное положение" });
            return;
        }
        downPartAnim.SetTrigger("disableTormozPipka");
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
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
        if (down_Part_Rotation.currentWheelState != WheelState.SOOS)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед тем как взаимодействовать с тормозным механизмом, установить колесный ход в соосное положение" });
            return;
        }
        downPartAnim.SetTrigger("enableTormozPipka");
        isSelected = true;
    }
}
