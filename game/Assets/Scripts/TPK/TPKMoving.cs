using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TPKDirection
{
    STAY = 0,
    FORWARD = 1,
    RIGHT = 2
}

public class TPKMoving : MonoBehaviour
{
    Animator anim;
    TPKDirection curDirection = TPKDirection.STAY;
    public int TEMP;

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
        MovingTPK();
        if (Input.GetKeyDown(KeyCode.R)) {

            if (curDirection == TPKDirection.FORWARD)
            {
                anim.SetTrigger("Moving_front");
            }
            else if (curDirection == TPKDirection.RIGHT) 
            {
                anim.SetTrigger("Valim_bokom");
            }
            curDirection = TPKDirection.STAY;
        }
    }

    void MovingTPK()
    {
        List<Domkrat> attachedDomkrats = TPK.TPKObj.attachedDomkrats;
        if (attachedDomkrats.Count == TEMP)
        {
            // Ровная поверхность
            if (isForward(attachedDomkrats))
            {
                curDirection = TPKDirection.FORWARD;
                Debug.Log("Пиздуй вперед");
            }

            // Закатывание



            // Скатывание



        }
    }

    bool isForward(List<Domkrat> attachedDomkrats)
    {
        foreach (var domkrat in attachedDomkrats)
        {
            if (domkrat.curV == OrientationVertical.Up)
            {
                if (domkrat.currentWheelState != WheelState.ROYAL || domkrat.downPartRotation.dir != Direction.BACK || !domkrat.rotateFixator.isSelected)
                {
                    return false;
                }
            }
            else if (domkrat.curV == OrientationVertical.Down)
            {
                if (domkrat.currentWheelState != WheelState.SOOS || domkrat.downPartRotation.dir != Direction.FORWARD || domkrat.rotateFixator.isSelected)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // isRight
}
