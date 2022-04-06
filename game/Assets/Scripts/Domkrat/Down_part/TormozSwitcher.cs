using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        NameState curState = Singleton.Instance.StateManager.GetState();
        if (curState == NameState.CHECK_BREAK_MECHANISM || curState == NameState.SET_TORMOZ)
        {
            boxCol.enabled = true;
        }
        else
        {
            boxCol.enabled = false;
        }
    }

    public override void Deselect()
    {
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
        downPartAnim.SetTrigger("enableTormozPipka");
        isSelected = true;
    }
}
