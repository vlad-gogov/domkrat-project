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

    string VerboseName()
    {
        string result = "";
        switch (type)
        {
            case TypeMode.Off:
                result = "нейтральный";
                break;
            case TypeMode.Podem:
                result = "подъём";
                break;
            case TypeMode.Opusk:
                result = "опускание";
                break;
        }
        return result;
    }

    public override void Deselect()
    {
        Singleton.Instance.UIManager.ClearHelperText();
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText($"Нажмите ЛКМ, чтобы выбрать режим домкрата '{VerboseName()}'.");
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
