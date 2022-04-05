using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TormozConnector : Selectable
{
    DomkratType type;

    [SerializeField] GameObject pointForTormoz;
    [SerializeField] GameObject pointerToAdapter;
    DomkratMoving domkratMove;

    bool isForwardWithoutStop, isBackWithoutStop, isForwardWithStop, isBackdWithStop;

    public void Start()
    {
        type = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Domkrat>().type;
        domkratMove = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<DomkratMoving>();
        Debug.Log("MOVE: " + domkratMove);
    }

    public override void Deselect()
    {
        Debug.Log("Deselecting pipka...");
        Tormoz.tormoz.gameObject.GetComponent<TormozMoving>().Disconnect(type);
        isSelected = false;
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
        GameObject tormz = Tormoz.tormoz.gameObject;
        tormz.GetComponent<TormozMoving>().ConnectTo(pointerToAdapter, type, pointForTormoz);
        isSelected = true;
    }

    void Update()
    {
        if (true)//Singleton.Instance.StateManager.GetState() == NameState.CHECK_BREAK_MECHANISM)
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
            else
            {
                // Подключите тормоз
            }
        }
        if (isBackdWithStop && isForwardWithStop && isBackWithoutStop && isForwardWithoutStop)
        {
            Debug.Log("Проверка тормозного механизма завершена");
        }
    }
}
