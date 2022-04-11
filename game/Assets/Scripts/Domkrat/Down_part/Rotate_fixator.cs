using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_fixator : Selectable
{
    private float lenght = 0.01f;
    private float step = 0.01f;
    private bool isMove = false;

    IEnumerator MoveFixator(float lenght)
    {
        isMove = true;
        float temp = lenght >= 0 ? 1 : -1;
        for (float t = 0; t <= Mathf.Abs(lenght); t += step * Time.deltaTime)
        {
            gameObject.transform.Translate(step * temp * Time.deltaTime, 0f, 0f);
            yield return null;
        }
        isMove = false;
    }

    public override void Deselect()
    {
        if (!isMove)
        {
            isSelected = false;
            StartCoroutine(MoveFixator(-lenght));
        }
    }

    public override void GetInfoMouse()
    {
        if (isMove)
        {
            return;
        }
        if (!isSelected)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы выключить фиксатор поворота");
        } else
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы включить фиксатор поворота");
        }
    }

    public override GameObject GetSelectObject()
    {
        return null;
    }

    public override void Select()
    {
        if (!isMove)
        {
            isSelected = true;
            StartCoroutine(MoveFixator(lenght));
        }
    }
}
