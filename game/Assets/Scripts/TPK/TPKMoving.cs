using System;
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
    bool isMoving = false;

    const float EPS = 10e-4f;

    float speedRotation = 100f;
    List<DomkratMoving> domkratMovings = new List<DomkratMoving>();

    void Update()
    {
        Moving();

        if (domkratMovings.Count == 0 && Singleton.Instance.StateManager.GetState() == NameState.UP_TPK)
        {
            foreach (var domkrat in TPK.TPKObj.attachedDomkrats)
                domkratMovings.Add(domkrat.GetComponent<DomkratMoving>());
        }
    }

    void Moving()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_FLAT || Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_UP || Singleton.Instance.StateManager.GetState() == NameState.MOVE_TPK_DOWN)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
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
    }

    void MovingTPK(TPKDirection tpkDir)
    {
        if (tpkDir == TPKDirection.STAY)
        {
            return;
        }
        List<Domkrat> attachedDomkrats = TPK.TPKObj.attachedDomkrats;
        if (attachedDomkrats.Count == 4)
        {
            if (TPK.TPKObj.state == StateTPK.DOWN)
            {
                Singleton.Instance.StateManager.onError(new Error(){ ErrorText = "Поднимите ТПК перед тем как его перемещать", Weight = ErrorWeight.CRITICAL});
            }
            if (isMoving)
            {
                return;
            }

            TypeArea type = Singleton.Instance.StateManager.typeArea;
            NameState curState = Singleton.Instance.StateManager.GetState();
            // Ровная поверхность
            if (type == TypeArea.FLAT)
            {
                if (tpkDir == TPKDirection.FORWARD && isForward(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(tpkDir));
                }
                else if (tpkDir == TPKDirection.RIGHT && isRight(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(tpkDir));
                }
                else if (tpkDir == TPKDirection.LEFT && isLeft(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(tpkDir));
                }
                else if (tpkDir == TPKDirection.BACK && isBack(attachedDomkrats))
                {
                    StartCoroutine(MoveTpk(tpkDir));
                }
            }
            // Закатывание
            else if (type == TypeArea.UP)
            {
                if (tpkDir == TPKDirection.FORWARD)
                {
                    if (curState == NameState.MOVE_TPK_UP && isUp(attachedDomkrats)) //  добавить что тормоз подключен
                    {
                        StartCoroutine(MoveTpk(tpkDir));
                    }
                }
            }


            // Скатывание
            else if (type == TypeArea.DOWN)
            {
                if (tpkDir == TPKDirection.FORWARD)
                {
                    if (curState == NameState.MOVE_TPK_DOWN && isDown(attachedDomkrats)) //  добавить что тормоз подключен
                    {
                        StartCoroutine(MoveTpk(tpkDir));
                    }
                }
            }
        }
        else
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите 4 домкрата перед тем как его перемещать", Weight = ErrorWeight.CRITICAL });
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
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.LOW });
                return false;
            }
            //Debug.Log($"{domkrat.curV} | {domkrat.downPartRotation.currentWheelState} | {domkrat.downPartRotation.dir} | {domkrat.rotateFixator.isSelected}");
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.FORWARD || !domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния передних домкратов для движения вперед", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.BACK || domkrat.rotateFixator.isSelected)
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
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.curH == OrientationHorizontal.Right)
            {
                if (domkrat.downPartRotation.currentWheelState == WheelState.ROYAL && domkrat.rotateFixator.isSelected)
                {
                    if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.LEFT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                        return false;
                    }
                    else if (domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.RIGHT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                        return false;
                    }
                } else
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                    return false;
                }
               
            }
            else if (domkrat.curH == OrientationHorizontal.Left)
            {
                if (domkrat.downPartRotation.currentWheelState == WheelState.SOOS && !domkrat.rotateFixator.isSelected)
                {
                    if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.LEFT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                        return false;
                    }
                    else if (domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.RIGHT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения вправо", Weight = ErrorWeight.LOW });
                        return false;
                    }
                }
                else
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
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.curH == OrientationHorizontal.Left)
            {
                if (domkrat.downPartRotation.currentWheelState == WheelState.ROYAL && domkrat.rotateFixator.isSelected)
                {
                    if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.RIGHT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения влево", Weight = ErrorWeight.LOW });
                        return false;
                    }
                    else if (domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.LEFT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения влево", Weight = ErrorWeight.LOW });
                        return false;
                    }
                }
                else
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния левых домкратов для движения влево", Weight = ErrorWeight.LOW });
                    return false;
                }

            }
            else if (domkrat.curH == OrientationHorizontal.Right)
            {
                if (domkrat.downPartRotation.currentWheelState == WheelState.SOOS && !domkrat.rotateFixator.isSelected)
                {
                    if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.RIGHT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения влево", Weight = ErrorWeight.LOW });
                        return false;
                    }
                    else if (domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.LEFT)
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения влево", Weight = ErrorWeight.LOW });
                        return false;
                    }
                }
                else
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния правых домкратов для движения влево", Weight = ErrorWeight.LOW });
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
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.LOW });
                return false;
            }
            if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.LEFT || !domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния задних домкратов для движения назад", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.RIGHT || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния передних домкратов для движения назад", Weight = ErrorWeight.LOW });
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
            //Debug.Log($"{domkrat.curV} | {domkrat.downPartRotation.currentWheelState} | {domkrat.downPartRotation.dir} | {domkrat.rotateFixator.isSelected}");
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (!domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на передных домкратах должен быть включен", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (!domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нельзя закатывать домкрат по наклонной поверхности без подключенного тормоза", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.BACK || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния передних домкратов для заката вверх", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на задних домкратах должен быть выключен", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть подключен к домкратам по ходу движения", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.FORWARD || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния задних домкратов для заката вверх", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
        }
        // Надо подумать
        //if (Tormoz.tormoz.tormozMovingHand.isSelected)
        //{
        //    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Ручка тормоза не отжата (тпк не тормозит)", Weight = ErrorWeight.LOW });
        //    return false;
        //}
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
            //Debug.Log($"{domkrat.curV} | {domkrat.downPartRotation.currentWheelState} | {domkrat.downPartRotation.dir} | {domkrat.rotateFixator.isSelected}");
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на передных домкратах должен быть выключен", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоза должны быть подключены к задним домкратам", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir == Direction.BACK || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния передних домкратов для заката вверх", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (!domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на задних домкратах должен быть включен", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (!domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нельзя скатывать домкрат по наклонной поверхности без подключенного тормоза", Weight = ErrorWeight.LOW });
                    return false;
                }
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir == Direction.FORWARD || domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Не правильное состояния задних домкратов для заката вверх", Weight = ErrorWeight.LOW });
                    return false;
                }
            }
        }
        // Надо подумать
        if (!Tormoz.tormoz.tormozMovingHand.isSelected)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Ручка тормоза отжата (тпк не может поехать вперед)", Weight = ErrorWeight.LOW });
            return false;
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
        isMoving = true;
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
        float a = 0.01f;
        float maxSpeed = 0.8f;
        delta = 300f;
        for (float i = 0; i <= Mathf.Abs(delta); i += shift * Time.deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                break;
            }
            Debug.Log($"{transform.rotation.x} | {Tormoz.tormoz.tormozMovingHand.isSelected}");
            if (transform.rotation.x > 0.02f) // поднимаемся в горку, тормоз должен быть отжат
            {
                if (Tormoz.tormoz.tormozMovingHand.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть отжат, иначе ТПК не тормозит", Weight = ErrorWeight.LOW });
                    StartCoroutine(FailLift(TPKDirection.BACK));
                    break;
                }
            }
            if (transform.rotation.x < -0.02f) // спускаемся с горки, тормоз должен быть то отжат то нажат
            {
                if (Tormoz.tormoz.tormozMovingHand.isSelected)
                {
                    shift += a;
                    //Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть отжат, иначе ТПК не тормозит", Weight = ErrorWeight.LOW });
                    //StartCoroutine(FailLift(TPKDirection.BACK));
                    //break;
                }
                else
                {
                    shift -= a;
                    if (shift < 0)
                    {
                        shift = 0;
                    }
                }
                if (shift > maxSpeed)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть отжат, иначе ТПК не тормозит", Weight = ErrorWeight.LOW });
                    StartCoroutine(FailLift(TPKDirection.BACK));
                    break;
                }
            }

            foreach (var domkrat in domkratMovings)
            {
                domkrat.RotateWheelForUpdate(speedRotation * Time.deltaTime, false);
            }
            gameObject.transform.Translate(vector * shift * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator FailLift(TPKDirection direction)
    {
        float shift = 0f;
        // right : минус по х
        // left : плюс по х
        // forward : минус по z
        // daun blyat : плюс по z
        isMoving = true;
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

        float a = 0.01f;
        float speed = shift;
        float maxSpeed = 1.2f;

        while(Math.Abs(transform.rotation.x) > EPS)
        {
            foreach (var domkrat in domkratMovings)
            {
                domkrat.RotateWheelForUpdate(speedRotation * Time.deltaTime, false);
            }
            if (speed < maxSpeed)
            {
                speed += a;
            }
            gameObject.transform.Translate(vector * speed * Time.deltaTime);
            yield return null;
        }

        while (speed > 0)
        {
            speed -= a;
            foreach (var domkrat in domkratMovings)
            {
                domkrat.RotateWheelForUpdate(speedRotation * Time.deltaTime, false);
            }
            gameObject.transform.Translate(vector * speed * Time.deltaTime);
            yield return null;
        }
    }
}
