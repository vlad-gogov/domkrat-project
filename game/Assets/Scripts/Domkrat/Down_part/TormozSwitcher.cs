using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Это надо опрокинуть в домкрат и проверять при перемещении
/// </summary>

public class TormozSwitcher : Selectable
{
    Animator downPartAnim;
    BoxCollider boxCol;

    public void Start()
    {
        downPartAnim = transform.parent.GetComponent<Animator>();
        boxCol = gameObject.GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }

    void Update()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.CHECK_BREAK_MECHANISM)
        {
            boxCol.enabled = true;
        }
    }

    public override void Deselect()
    {
        Debug.Log("disable PIPKA!!!");
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
        Debug.Log("enable PIPKA!!!");
        downPartAnim.SetTrigger("enableTormozPipka");
        isSelected = true;
    }
}
