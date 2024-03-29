﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TPKDirection
{
    STAY = 0,
    FORWARD = 1,
    RIGHT = 2,
    LEFT = 3,
    BACK = 4,
    FINISHED = 5
}

public class TPKMoving : MonoBehaviour
{
    TPKDirection curDirection = TPKDirection.STAY;
    bool isMoving = false;
    public bool isStop = false;

    const float EPS = 10e-4f;

    float speedRotation = 100f;
    List<DomkratMoving> domkratMovings = new List<DomkratMoving>();

    bool isFlat;

    void Start()
    {
        isFlat = Singleton.Instance.StateManager.typeArea == TypeArea.FLAT;
    }

    void Update()
    {
        Moving();

        if (domkratMovings.Count == 0 && Singleton.Instance.StateManager.GetState() == NameState.UP_TPK)
        {
            foreach (var domkrat in TPK.TPKObj.attachedDomkrats)
                domkratMovings.Add(domkrat.GetComponent<DomkratMoving>());
        }

        if (Singleton.Instance.StateManager.GetState() == NameState.DOWN_TPK && TPK.TPKObj.state == StateTPK.DOWN)
        {
            Singleton.Instance.StateManager.NextState();
        }
    }

    public void FinishedMoving()
    {
        curDirection = TPKDirection.FINISHED;
    }

    void Moving()
    {
        // Раскоментить для дебага
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    curDirection = TPKDirection.FORWARD;
        //    StartCoroutine(MoveTpk(curDirection));
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    curDirection = TPKDirection.RIGHT;
        //    StartCoroutine(MoveTpk(curDirection));
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    curDirection = TPKDirection.LEFT;
        //    StartCoroutine(MoveTpk(curDirection));
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    curDirection = TPKDirection.BACK;
        //    StartCoroutine(MoveTpk(curDirection));
        //}

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
        if (TPK.TPKObj.state == StateTPK.DOWN) 
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Поднимите ТПК перед тем, как его перемещать", Weight = ErrorWeight.CRITICAL });
            return;
        }
        List<Domkrat> attachedDomkrats = TPK.TPKObj.attachedDomkrats;
        if (attachedDomkrats.Count == 4)
        {
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
                    if (curState == NameState.MOVE_TPK_UP && isUp(attachedDomkrats))
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
                    if (curState == NameState.MOVE_TPK_DOWN && isDown(attachedDomkrats))
                    {
                        StartCoroutine(MoveTpk(tpkDir));
                    }
                }
            }
        }
        else
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите 4 домкрата перед тем, как его перемещать", Weight = ErrorWeight.CRITICAL });
            return;
        }
        curDirection = TPKDirection.STAY;
    }

    bool isForward(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.techStand.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.isTormozConnected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз не нужен для перемещения ТПК по ровной поверхности", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.ROYAL)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы передних домкрат в рояльное положение", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (!domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Разблокируйте поворотный механизм на передних домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.FORWARD)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на передних домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS )
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы задних домкрат в соосное положение", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Заблокируйте поворотный механизм на задних домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.BACK)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на задних домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
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
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.isTormozConnected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз не нужен для перемещения ТПК по ровной поверхности", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы передних домкратов в соосное положение", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Заблокируйте поворотный механизм на передних домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.BACK)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на передних домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.ROYAL)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы задних домкратов в рояльное положение", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (!domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Разблокируйте поворотный механизм на задних домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.FORWARD)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на задних домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
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
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.isTormozConnected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз не нужен для перемещения ТПК по ровной поверхности", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.curH == OrientationHorizontal.Right)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.ROYAL)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы правых домкратов в рояльное положение ", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (!domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Разблокируйте поворотный механизм на правых домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }

                if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.LEFT || domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.RIGHT)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на правых домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
                    return false;
                }
               
            }
            else if (domkrat.curH == OrientationHorizontal.Left)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы левых домкратов в соосное положение ", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Заблокируйте поворотный механизм на левых домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.LEFT || domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.RIGHT)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на левых домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
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
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.isTormozConnected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз не нужен для перемещения ТПК по ровной поверхности", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.tormozSwitch.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Отключите тормозной механизм на всех домкратах", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.curH == OrientationHorizontal.Right)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы правых домкратов в соосное положение ", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Заблокируйте поворотный механизм на правых домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }

                if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.RIGHT || domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.LEFT)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на правых домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
                    return false;
                }

            }
            else if (domkrat.curH == OrientationHorizontal.Left)
            {
                if (domkrat.downPartRotation.currentWheelState != WheelState.ROYAL)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы левых домкратах в рояльное положение ", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (!domkrat.rotateFixator.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Разблокируйте поворотный механизм на левых домкратах", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.curV == OrientationVertical.Up && domkrat.downPartRotation.dir != Direction.RIGHT || domkrat.curV == OrientationVertical.Down && domkrat.downPartRotation.dir != Direction.LEFT)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на левых домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
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
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы всех домкратов в соосное положение ", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.rotateFixator.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Заблокируйте поворотный механизм на всех домкратах ", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (!domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на передных домкратах должен быть включен", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (!domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нельзя закатывать ТПК по наклонной поверхности без подключенного тормоза к передним домкратам", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.FORWARD)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на передних домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на задних домкратах должен быть выключен", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть подключен только к передним домкратам", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.BACK)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на задних домкратах должно быть направлено по ходу движения", Weight = ErrorWeight.HIGH });
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
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Уберите все технологические подставки", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.downPartRotation.currentWheelState != WheelState.SOOS)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите колесные ходы всех домкратов в соосное положение ", Weight = ErrorWeight.HIGH });
                return false;
            }
            if (domkrat.rotateFixator.isSelected)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Заблокируйте поворотный механизм на всех домкратах ", Weight = ErrorWeight.MEDIUM });
                return false;
            }
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на передных домкратах должен быть выключен", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть подключен только к задним домкратам", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.BACK)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на передних домкратах должно быть направлено против хода движения", Weight = ErrorWeight.HIGH });
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (!domkrat.tormozSwitch.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормозной механизм на задних домкратах должен быть включен", Weight = ErrorWeight.MEDIUM });
                    return false;
                }
                if (!domkrat.isTormozConnected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Нельзя закатывать ТПК по наклонной поверхности без подключенного тормоза задним домкратам", Weight = ErrorWeight.HIGH });
                    return false;
                }
                if (domkrat.downPartRotation.dir != Direction.FORWARD)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Направление стрелок на задних домкратах должно быть направлено против хода движения", Weight = ErrorWeight.HIGH });
                    return false;
                }
            }
        }
        // Надо подумать
        if (!Tormoz.tormoz.tormozMovingHand.isSelected)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Ручка тормоза отжата (ТПК не может поехать вперед)", Weight = ErrorWeight.LOW });
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

        if (isFlat)
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите T чтобы остановить движение ТПК");
            Singleton.Instance.UIManager.noOverwrite = true;
        }
        DisableDomkrats(false);
        for (float i = 0; i <= Mathf.Abs(delta); i += shift * Time.deltaTime)
        {
            if (isFlat && isStop)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Куда покатил???", Weight = ErrorWeight.MINOR });
                gameObject.transform.Translate(-vector * 0.3f);
                isStop = false;
                break;
            }
            if ((Input.GetKeyDown(KeyCode.T) && isFlat) || curDirection == TPKDirection.FINISHED)
            {
                break;
            }
            // Debug.Log($"{transform.rotation.x} | {Tormoz.tormoz.tormozMovingHand.isSelected}");
            if (transform.rotation.x > 0.02f) // поднимаемся в горку, тормоз должен быть отжат
            {
                if (Tormoz.tormoz.tormozMovingHand.isSelected)
                {
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть отжат, иначе ТПК не тормозит", Weight = ErrorWeight.UBIL });
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
                    Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Тормоз должен быть отжат, иначе ТПК не тормозит", Weight = ErrorWeight.UBIL });
                    StartCoroutine(FailLift(TPKDirection.BACK));
                    break;
                }
            }

            foreach (var domkrat in domkratMovings)
            {
                if (Singleton.Instance.StateManager.typeArea == TypeArea.DOWN)
                {
                    if (Tormoz.tormoz.tormozMovingHand.isSelected)
                    {
                        domkrat.RotateWheelForUpdate(-speedRotation * Time.deltaTime, false);
                    }
                }
                else
                {
                    domkrat.RotateWheelForUpdate(speedRotation * Time.deltaTime, false);
                }
            }

            gameObject.transform.Translate(vector * shift * Time.deltaTime);
            yield return null;
        }
        Singleton.Instance.UIManager.noOverwrite = false;
        Singleton.Instance.UIManager.ClearEnterText();
        isMoving = false;
        DisableDomkrats(true);
    }

    void DisableDomkrats(bool flag)
    {
        foreach (var domkrat in TPK.TPKObj.attachedDomkrats)
            domkrat.SwitchIntecration(flag);
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
                domkrat.RotateWheelForUpdate(-speedRotation * Time.deltaTime, false);
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
                domkrat.RotateWheelForUpdate(-speedRotation * Time.deltaTime, false);
            }
            gameObject.transform.Translate(vector * speed * Time.deltaTime);
            yield return null;
        }
    }
}
