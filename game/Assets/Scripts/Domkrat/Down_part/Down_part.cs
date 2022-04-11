using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_part : MonoBehaviour
{
    Animator animator;
    Domkrat parentDomkrat;
    [HideInInspector] public Down_part_rotation rotation_down_part;
    public GameObject ruchka;
    public Makes curPosition;
    public Rotate_fixator fixator;

    BoxCollider boxFixator;
    BoxCollider stand;

    void Start()
    {
        animator = GetComponent<Animator>();
        parentDomkrat = gameObject.transform.parent.GetComponent<Domkrat>();
        // should always be `DOWN` at Start
        curPosition = Makes.DOWN;
        rotation_down_part = transform.GetChild(transform.childCount - 1).GetComponent<Down_part_rotation>();
        boxFixator = fixator.gameObject.GetComponent<BoxCollider>();
        boxFixator.enabled = false;
        stand = gameObject.transform.parent.GetChild(2).GetComponent<BoxCollider>();
    }

    public bool Up(bool isTechStand = false)
    {
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isTechStand)
            {
                // Пытаемся поднять нижнюю часть домкрата без технологической подставки с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установить технологическую подставку перед тем, как поднимать нижнюю часть домкарата", Weight = ErrorWeight.LOW });
                return false;
            }
            RealUp();
        }
        return true;
    }

    public bool Down(bool isTechStand = false)
    {
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isTechStand)
            {
                // Пытаемся поднять нижнюю часть домкрата без технологической подставки с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установить технологическую подставку перед тем, как поднимать нижнюю часть домкарата", Weight = ErrorWeight.LOW });
                return false;
            }
            RealDown();
        }
        return true;
    }

    void RealUp()
    {
        animator.SetTrigger("Up"); // анимация подъема нижней части домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Up"); // анимация вращения ручки
    }

    void RealDown()
    {
        animator.SetTrigger("Down"); // анимация опускания (по масти) нижней части домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Down"); // анимация вращения ручки
    }

    void SetUp()
    {
        curPosition = Makes.UP;
    }

    void SetDown()
    {
        curPosition = Makes.DOWN;
        
    }

    void EndAnimSwitchRoyal()
    {
        rotation_down_part.EndAnimSwitchRoyal();
    }

    void EndAnimTormozSwitcher()
    {
        rotation_down_part.EndAnimTormozSwitcher();
    }

    void OnStand()
    {
        stand.enabled = true;
    }

    void DeselectRuchka()
    {
        ruchka.GetComponent<Ruchka>().Deselect();
    }

    void Update()
    {
        if (!boxFixator.enabled && Singleton.Instance.StateManager.GetState() == NameState.CHECK_BREAK_MECHANISM)
        {
            boxFixator.enabled = true;
        }

        if (curPosition == Makes.UP)
        {
            if (!rotation_down_part.isRotate) {
                if (Input.GetKey(KeyCode.E)) // 200$ c Vladika
                {
                    if (fixator.isSelected)
                    {
                        rotation_down_part.RotateDownPart(90f);
                    }
                    else
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Выключить фиксатор поворота", Weight = ErrorWeight.LOW });
                    }
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    if (fixator.isSelected)
                    {
                        rotation_down_part.RotateDownPart(-90f);
                    }
                    else
                    {
                        Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Выключить фиксатор поворота", Weight = ErrorWeight.LOW });
                    }
                }
            } 

        }
    }
}
