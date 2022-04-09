﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozConnector : Selectable
{
    DomkratType type;

    [SerializeField] GameObject pointForTormoz;
    [SerializeField] GameObject pointerToAdapter;
    Down_part down_part;

    DomkratMoving domkratMove;
    Domkrat batya;
    BoxCollider boxCol;

    bool isForwardWithoutStop = false, isBackWithoutStop = false, isForwardWithStop = false, isBackdWithStop = false;

    public void Start()
    {
        batya = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Domkrat>();
        type = batya.type;
        domkratMove = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<DomkratMoving>();
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false;
        tormozMoving = Tormoz.tormoz.gameObject.GetComponent<TormozMoving>();
        tormoz = Tormoz.tormoz.GetComponent<Tormoz>();
        if (tormozMoving == null)
        {
            Debug.LogError("blyat!!!");
        }
        if (tormoz == null)
        {
            Debug.Log("SUKA!!!!!");
        }
        down_part = gameObject.transform.parent.parent.parent.parent.GetComponent<Down_part>();
        if (down_part == null)
        {
            Debug.LogError("pizdec");
        }
    }

    // так надо: инача в некоторых сценах ломается порядок конструирования объектов
    // TormozMoving tormozMoving { get => Tormoz.tormoz.gameObject.GetComponent<TormozMoving>(); }
    // Tormoz tormoz { get => Tormoz.tormoz.GetComponent<Tormoz>(); }
    TormozMoving tormozMoving;
    Tormoz tormoz;

    public override void Deselect()
    {
        tormozMoving.Disconnect(type);
        isSelected = false;
        batya.isTormozConnected = false;
    }

    public override void GetInfoMouse()
    {
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы подключить тормоз");
        }
        else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы отключить тормоз");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.CHECK_BREAK_MECHANISM)
        {
            if (down_part.curPosition == Makes.UP)
            {
                if (!tormoz.isUse)
                {
                    ConnectTormoz();
                }
                else
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм уже занят, отключите его от другого домкрата", Weight = ErrorWeight.MINOR });
                }
            }
            else
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Необходимо вывесить домкрат перед подключением тормозного механизма", Weight = ErrorWeight.LOW });
            }
        }
        else
        // else if (Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_UP || Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_DOWN)
        {
            ConnectTormoz();
        }
    }

    void ConnectTormoz()
    {
        var result = tormozMoving.ConnectTo(pointerToAdapter, type, pointForTormoz);
        if (!result)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Сначала отключить тормоз от другого домкрата", Weight = ErrorWeight.LOW });
            return;
        }
        tormoz.gameObject.SetActive(true);
        isSelected = true;
        batya.isTormozConnected = true;
    }

    void Update()
    {
        NameState curState = Singleton.Instance.StateManager.GetState();
        if (curState == NameState.CHECK_BREAK_MECHANISM)
        {
            if (isSelected) 
            {
                // Ручка у тормоза опущена (вращение в любую сторону)
                if(Tormoz.tormoz.tormozMovingHand.isSelected)
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        StartCoroutine(domkratMove.RotateWheel(-5f, 10f));
                        isForwardWithoutStop = true;
                    }
                    else if (Input.GetKey(KeyCode.UpArrow))
                    {
                        StartCoroutine(domkratMove.RotateWheel(5f, 10f));
                        isBackWithoutStop = true;
                    }
                }
                // Ручка у тормоза поднята (запрещает вращение в против стрелок на колесах)
                else
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        StartCoroutine(domkratMove.RotateWheel(5f, 10f));
                        isForwardWithStop = true;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        StartCoroutine(domkratMove.RotateWheel(-1f, 10f, true));
                        isBackdWithStop = true;
                    }
                }
            }
            if (isBackdWithStop && isForwardWithStop && isBackWithoutStop && isForwardWithoutStop && !isSelected)
            {
                Singleton.Instance.StateManager.NextState();
                tormozMoving.Disconnect(type);
                tormoz.gameObject.SetActive(false);
            }
        }
    }
}
