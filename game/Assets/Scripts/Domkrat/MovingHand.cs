using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHand : Selectable
{

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
    }

    public override void Select()
    {
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
    }

    public override GameObject GetSelectObject()
    {
        return transform.root.gameObject;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы взять домкрат");
    }
}
