using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeRuchka : Selectable
{
    public PositionRuchka curPos;

    [SerializeField] GameObject down_ruchka;
    [SerializeField] RuckaMoving RuckaMoving;

    Animator anim;
    Domkrat domkrat;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        domkrat = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Domkrat>();
    }

    public override void Deselect()
    {
        anim.SetTrigger("Idle");
        isSelected = false;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы снять ручку с домкрата");
    }

    public override GameObject GetSelectObject()
    {
        return gameObject.transform.parent.transform.parent.gameObject;
    }

    public override void Select()
    {
        domkrat.isRuchka = false;
        isSelected = true;
        anim.SetTrigger("Take");
        RuckaMoving.isMoving = false;
        down_ruchka.GetComponent<Collider>().enabled = false;

    }

    public void ChangeIsMoving()
    {
        RuckaMoving.isMoving = true;
    }
}
