using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeMode
{
    Off = 0,
    Opusk = 1,
    Podem = 2
}

public class Mode : Selectable
{
    [SerializeField] public TypeMode type = TypeMode.Off;
    [SerializeField] private GameObject Switch;
    private Switch sw;

    void Start()
    {
        sw = Switch.GetComponent<Switch>();
    }

    public override void Deselect()
    {
        return;
    }

    public override void GetInfoMouse()
    {
        return;
    }

    public override GameObject GetSelectObject()
    {
        return gameObject;
    }

    public override void Select()
    {
        sw.ChangeState(type);
    }
}
