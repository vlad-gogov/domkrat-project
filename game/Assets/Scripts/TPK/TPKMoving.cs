using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TPKDirection
{
    STAY = 0,
    FORWARD = 1,
    RIGHT = 2,
    UP = 3,
    DOWN = 4,
    LEFT = 5,
    BACK = 6
}

public class TPKMoving : MonoBehaviour
{
    TPKDirection curDirection = TPKDirection.STAY;

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            curDirection = TPKDirection.FORWARD;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            curDirection = TPKDirection.RIGHT;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curDirection = TPKDirection.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            curDirection = TPKDirection.BACK;
        }
        MovingTPK(curDirection);
    }

    void MovingTPK(TPKDirection tpkDir)
    {
        if (tpkDir == TPKDirection.STAY)
        {
            return;
        }
        List<Domkrat> attachedDomkrats = TPK.TPKObj.attachedDomkrats;
        // Добавить ошибку на кол-во подключенных домкратов
        if (attachedDomkrats.Count != 0)
        {
            if (TPK.TPKObj.state != StateTPK.UP)
            {
                Singleton.Instance.StateManager.onError(new Error(){ ErrorText = "Поднимите ТПК перед тем как его перемещать", Weight = ErrorWeight.CRITICAL});
             }
            TypeArea type = Singleton.Instance.StateManager.typeArea;

            // Ровная поверхность
            if (type == TypeArea.FLAT)
            {
                if (tpkDir == TPKDirection.FORWARD && isForward(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(TPKDirection.FORWARD));
                }
                else if (tpkDir == TPKDirection.RIGHT && isRight(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(TPKDirection.RIGHT));
                }
                else if (tpkDir == TPKDirection.LEFT && isLeft(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(TPKDirection.LEFT));
                }
                else if (tpkDir == TPKDirection.RIGHT && isBack(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(TPKDirection.BACK));
                }
            }

            // Закатывание
            else if (type == TypeArea.UP)
            {
                curDirection = TPKDirection.UP;
            }


            // Скатывание

            else if (type == TypeArea.DOWN)
            {
                curDirection = TPKDirection.DOWN;
            }
        }
        curDirection = TPKDirection.STAY;
    }

    bool isForward(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.BACK || !domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния передних домкратов для движения вперед", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.FORWARD || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния задних домкратов для движения вперед", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
        }
        return true;
    }

    bool isRight(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.curH == OrientationHorizontal.Right)
            {
                if (domkrat.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.LEFT || !domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curH == OrientationHorizontal.Left)
            {
                if (domkrat.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.RIGHT || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
        }
        return true;
    }

    bool isLeft(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.curH == OrientationHorizontal.Left)
            {
                if (domkrat.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.LEFT || !domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curH == OrientationHorizontal.Right)
            {
                if (domkrat.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.RIGHT || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
        }
        return true;
    }

    bool isBack(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.LEFT || !domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.RIGHT || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
        }
        return true;
    }

    bool isUp(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.LOW });
                return false;
            }
        }
        return true;
    }

    bool isDown(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.LOW });
                return false;
            }
        }
        return true;
    }

    IEnumerator MoveTpk(TPKDirection direction)
    {
        float shift = 0.2f;
        // right : минус по х
        // left : плюс по х
        // forward : минус по z
        // daun blyat : плюс по z

        Vector3 vector = new Vector3(0, 0, 0);
        float delta = 3f;

        switch (direction)
        {
            case TPKDirection.RIGHT:
                vector = new Vector3(-delta, 0, 0);
                break;
            case TPKDirection.LEFT:
                vector = new Vector3(delta, 0, 0);
                break;
            case TPKDirection.FORWARD:
                vector = new Vector3(0, 0, -delta);
                break;
            case TPKDirection.BACK:
                vector = new Vector3(0, 0, delta);
                break;
        }
        for (float i = 0; i <= Mathf.Abs(delta); i += shift * Time.deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                break;
            }
            gameObject.transform.Translate(vector * shift * Time.deltaTime);
            yield return null;
        }
    }
}
