using System.Collections;
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

    TormozMoving tormozMoving;
    Tormoz tormoz;

    bool isForwardWithoutStop = false, isBackWithoutStop = false, isForwardWithStop = false, isBackdWithStop = false;

    public void Start()
    {
        batya = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Domkrat>();
        type = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Domkrat>().type;
        domkratMove = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<DomkratMoving>();
        tormozMoving = Tormoz.tormoz.gameObject.GetComponent<TormozMoving>();
        tormoz = Tormoz.tormoz.GetComponent<Tormoz>();
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false;
        down_part = gameObject.transform.parent.parent.parent.parent.GetComponent<Down_part>();
    }

    public override void Deselect()
    {
        Debug.Log("Deselecting pipka...");
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
                    tormoz.gameObject.SetActive(true);
                    tormozMoving.ConnectTo(pointerToAdapter, type, pointForTormoz);
                    isSelected = true;
                    batya.isTormozConnected = true;
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
        else if (Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_UP || Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_DOWN)
        {
            tormoz.gameObject.SetActive(true);
            tormozMoving.ConnectTo(pointerToAdapter, type, pointForTormoz);
            isSelected = true;
            batya.isTormozConnected = true;
        }
    }

    void Update()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.CHECK_BREAK_MECHANISM)
        {
            if (isSelected) 
            {
                // Ручка у тормоза опущена (вращение в любую сторону)
                if(Tormoz.tormoz.tormozMovingHand.isSelected)
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        StartCoroutine(domkratMove.RotateWheel(5f, 10f));
                        isForwardWithoutStop = true;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        StartCoroutine(domkratMove.RotateWheel(-5f, 10f));
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
                // boxCol.enabled = false;
            }
        }
    }
}
