using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozSwitcher : Selectable
{
    Animator downPartAnim;
    GameObject tormozConnector;
    bool isActivated = false;

    public void Start()
    {
        downPartAnim = transform.parent.GetComponent<Animator>();
        tormozConnector = transform.parent.GetChild(3).GetChild(0).GetChild(0).gameObject;
    }

    public override void Deselect()
    {
        Debug.Log("Deselecting pipka...");
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        if (!isActivated)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы активировать пипку тормоза");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы деактивировать пипку тормоза");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (!isActivated)
        {
            Debug.Log("enable PIPKA!!!");
            downPartAnim.SetTrigger("enableTormozPipka");
            tormozConnector.GetComponent<BoxCollider>().enabled = true;
            isActivated = true;
        }
        else
        {
            Debug.Log("disable PIPKA!!!");
            downPartAnim.SetTrigger("disableTormozPipka");
            tormozConnector.GetComponent<BoxCollider>().enabled = false;
            isActivated = false;
        }
        isSelected = true;
    }
}
