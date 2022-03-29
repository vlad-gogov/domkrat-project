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
    Animator anim;
    TPKDirection curDirection = TPKDirection.STAY;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        Debug.Log("Ya pidoras: Moving()");
        if (Input.GetKeyDown(KeyCode.R)) {
            MovingTPK();
            Debug.Log("curDirection: " + curDirection.ToString());
            if (curDirection == TPKDirection.FORWARD)
            {
                Singleton.Instance.StateManager.NextState();
                StartCoroutine(MoveTpk(TPKDirection.FORWARD));
                // anim.SetTrigger("Moving_front");
            }
            else if (curDirection == TPKDirection.RIGHT) 
            {
                // anim.SetTrigger("Valim_bokom");
                StartCoroutine(MoveTpk(TPKDirection.RIGHT));
            }
            else if (curDirection == TPKDirection.UP)
            {
                //anim.SetTrigger("Valim_bokom");
            }
            else if (curDirection == TPKDirection.DOWN)
            {
                //anim.SetTrigger("Valim_bokom");
            }
            curDirection = TPKDirection.STAY;
        }
    }

    void MovingTPK()
    {
        List<Domkrat> attachedDomkrats = TPK.TPKObj.attachedDomkrats;
        if (attachedDomkrats.Count == 1)
        {
            TypeArea type = Singleton.Instance.StateManager.typeArea;

            // Ровная поверхность
            if (type == TypeArea.FLAT)
            {
                if (isForward(attachedDomkrats))
                {
                    curDirection = TPKDirection.FORWARD;
                }
                else if (isRight(attachedDomkrats))
                {
                    curDirection = TPKDirection.RIGHT;
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
            Debug.Log(domkrat.currentWheelState.ToString() + " " + domkrat.downPartRotation.dir.ToString() + " " + domkrat.rotateFixator.isSelected.ToString());
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
                Debug.Log("Ya pidoras!!! (isRight)");
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
        Debug.Log("Ya pidoras: MoveTPK()");
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
        Debug.Log("ALO " + vector.ToString());
        for (float i = 0; i <= Mathf.Abs(delta); i += shift * Time.deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                break;
            }
            Debug.Log("ALO2 " + (vector * shift).ToString());
            gameObject.transform.Translate(vector * shift * Time.deltaTime);
            yield return null;
        }
    }
}
