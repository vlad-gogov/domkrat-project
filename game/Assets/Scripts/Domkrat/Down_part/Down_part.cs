using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_part : MonoBehaviour
{
    Animator animator;
    Domkrat parentDomkrat;
    public Down_part_rotation rotation_down_part;
    public GameObject ruchka;
    public Makes curPosition;
    public Rotate_fixator fixator;

    void Start()
    {
        animator = GetComponent<Animator>();
        parentDomkrat = gameObject.transform.parent.GetComponent<Domkrat>();
        // should always be `DOWN` at Start
        curPosition = Makes.DOWN;
    }

    public bool Up(bool isTechStand = false)
    {
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isTechStand)
            {
                // Пытаемся поднять нижнюю часть домкрата без технологической подставки с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установить технологическую подставку перед тем как поднимать нижнюю часть домкарата", Weight = ErrorWeight.HIGH });
                return false;
            }
            RealUp();
        }
        // curPosition = Makes.UP;
        return true;
    }

    public bool Down(bool isTechStand = false)
    {
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isTechStand)
            {
                // Пытаемся поднять нижнюю часть домкрата без технологической подставки с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установить технологическую подставку перед тем как поднимать нижнюю часть домкарата", Weight = ErrorWeight.HIGH });
                return false;
            }
            RealDown();
        }
        // curPosition = Makes.DOWN;
        return true;
    }

    void RealUp()
    {
        curPosition = Makes.UP;
        animator.SetTrigger("Up"); // анимация подъема нижней части домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Up"); // анимация вращения ручки
    }

    void RealDown()
    {
        curPosition = Makes.DOWN;
        animator.SetTrigger("Down"); // анимация опускания (по масти) нижней части домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Down"); // анимация вращения ручки
    }

    void Update()
    {
        if (curPosition == Makes.UP)
        {

            if (!rotation_down_part.isRotate && fixator.isSelected)
            {
                if (Input.GetKey(KeyCode.E)) // 200$ c Vladika
                {
                    rotation_down_part.RotateDownPart(90f);
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    rotation_down_part.RotateDownPart(-90f);
                }
            } else
            {
                //Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Разблокируйте фиксатор поворота", Weight = ErrorWeight.LOW });
                
            }

        }
    }
}
