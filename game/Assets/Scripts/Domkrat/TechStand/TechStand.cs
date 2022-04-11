using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechStand : Selectable
{
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public override void Deselect()
    {
        TPK.TPKObj.SwtichTechStand(true);
        isSelected = false;
        meshRenderer.enabled = false;    
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы установить технологическую подставку");
        else
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы убрать технологическую подставку");
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        isSelected = true;
        meshRenderer.enabled = true;
        TPK.TPKObj.SwtichTechStand(false);
    }
}
