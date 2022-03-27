﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Makes
{
    UP = 0,
    DOWN = 1
}

public class Up_part : MonoBehaviour
{
    public UnityEvent my_event;
    Animator animator;
    Domkrat parentDomkrat;
    public GameObject TPK;
    public GameObject ruchka;

    public Makes curPosition;

    // Словарь по факту содержит матрицу всех возможных проверок: doneCheck[Makes][bool1] -> bool2
    //      - Makes: направление в котором проверяли (вверх-вниз)
    //      - bool1: в режиме "с грузом"/"без груза"
    //      - bool2: проверили этот режим или нет
    // Когда весь словарь полностью заполнится true репортим об этом в StateManager
    Dictionary<Makes, Dictionary<bool, bool>> doneChecks = new Dictionary<Makes, Dictionary<bool, bool>>()
    {
        {Makes.UP, new Dictionary<bool, bool> {{true, false}, { false, false} } },
        {Makes.DOWN, new Dictionary<bool, bool> {{true, false}, { false, false} } },
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        parentDomkrat = gameObject.GetComponentInParent<Domkrat>();
        // should always be `DOWN` at Start
        curPosition = Makes.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideFinger()
    {
        my_event.Invoke();
    }

    public void Up(bool isOnWeightMode = false)
    {
        curPosition = Makes.UP;
        UpdateTestingDict(curPosition, isOnWeightMode);
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isOnWeightMode)
            {
                // Пытаемся поднять домкрат в режиме "без груза" с подключенным ТПК
                Debug.Log("Debil ne v tom rezhime podnimaesh!!!");
            }
            RealUp();
        }
        else
        {
            TestingUp();
        }
    }

    public void Down(bool isOnWeightMode = false)
    {
        curPosition = Makes.DOWN;
        UpdateTestingDict(curPosition, isOnWeightMode);
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isOnWeightMode)
            {
                // Пытаемся опустить домкрат в режиме "без груза" с подключенным ТПК
                Debug.Log("Debil ne v tom rezhime opuskaet!!!");
            }
            RealDown();
        }
        else
        {
            TestingDown(); // кто down?? сам ты down!
        }
        
    }

    void UpdateTestingDict(Makes newPosition, bool isOnWeight)
    {
        bool wasAllCheckComplete = IsAllCheckComplete();
        doneChecks[newPosition][isOnWeight] = true;
        bool doNowAllCheckComplete = IsAllCheckComplete();

        // Если результатом последней проверки стало то, что весь словарь теперь из true,
        // то репортим в менеджер, что все проверки пройдены
        if (doNowAllCheckComplete && !wasAllCheckComplete)
        {
            Debug.Log("Krasava, vse proverki s domkratom sdelal!!!");
            // TODO: надо делать NextState() или что=то другое?
            Singleton.Instance.StateManager.NextState();
        }
    }

    bool IsAllCheckComplete()
    {
        bool isAllCheckComplete = true;
        foreach (var value in doneChecks)
        {
            foreach (var isCheckDone in value.Value)
            {
                if (!isCheckDone.Value)
                {
                    isAllCheckComplete = false;
                    break;
                }
            }
        }
        return isAllCheckComplete;
    }

    void TestingUp()
    {
        animator.SetTrigger("LittleUp"); // анимация подъема самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("LittleMove"); // анимация вращения ручки
    }

    void TestingDown()
    {
        animator.SetTrigger("LittleDown"); // анимация опускания самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("LittleMove"); // анимация вращения ручки
    }

    void RealUp()
    {
        animator.SetTrigger("Up"); // анимация подъема самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Up"); // анимация вращения ручки
        TPK.GetComponent<Animator>().SetTrigger("Up"); // анимация ПОДЪЕМА ТПК БЛЯТЬ (да-да не удивляйтесь)
    }

    void RealDown()
    {
        animator.SetTrigger("Down"); // анимация опускания (по масти) самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Down"); // анимация вращения ручки
        TPK.GetComponent<Animator>().SetTrigger("Down"); // анимация ОПУСКАНИЯ ТПК БЛЯТЬ (да-да не удивляйтесь)
    }
}
