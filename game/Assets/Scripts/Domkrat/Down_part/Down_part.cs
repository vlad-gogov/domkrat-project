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

    void Start()
    {
        animator = GetComponent<Animator>();
        parentDomkrat = gameObject.transform.parent.GetComponent<Domkrat>();
        curPosition = Makes.UP;
        rotation_down_part = transform.GetChild(transform.childCount - 1).GetComponent<Down_part_rotation>();
        boxFixator = fixator.gameObject.GetComponent<BoxCollider>();
        boxFixator.enabled = false;
        stand = gameObject.transform.parent.GetChild(2).GetComponent<BoxCollider>();
    }

    void UpdateTestingDict(Makes newPosition, bool isOnWeight)
    {
        if (Singleton.Instance.StateManager.GetState() != NameState.CHECK_DOMKRATS)
        {
            return;
        }
        bool wasAllCheckComplete = IsAllCheckComplete();
        doneChecks[newPosition][isOnWeight] = true;
        bool doNowAllCheckComplete = IsAllCheckComplete();

        // Если результатом последней проверки стало то, что весь словарь теперь из true,
        // то репортим в менеджер, что все проверки пройдены
        if (doNowAllCheckComplete && !wasAllCheckComplete)
        {
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

    public bool Up(bool isTechStand = false, bool isOnWeightMode = false)
    {
        UpdateTestingDict(Makes.UP, isOnWeightMode);
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
        else
        {
            TestingUp();
        }
        return true;
    }

    public bool Down(bool isTechStand = false, bool isOnWeightMode = false)
    {
        UpdateTestingDict(Makes.DOWN, isOnWeightMode);
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
        else
        {
            TestingDown();
        }
        return true;
    }

    void TestingUp()
    {
        curPosition = Makes.UP;
        animator.SetTrigger("LittleUp"); // анимация подъема самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("LittleMove"); // анимация вращения ручки
    }

    void TestingDown()
    {
        curPosition = Makes.DOWN;
        animator.SetTrigger("LittleDown"); // анимация опускания самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("LittleMove"); // анимация вращения ручки
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
        NameState curState = Singleton.Instance.StateManager.GetState();
        if (!boxFixator.enabled && curState == NameState.CHECK_BREAK_MECHANISM)
        {
            boxFixator.enabled = true;
        }

        if (curPosition == Makes.UP && curState >= NameState.UP_TPK)
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
