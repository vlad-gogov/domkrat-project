using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Makes
{
    UP = 0,
    DOWN = 1,
    STAY = 3
}

public class Up_part : MonoBehaviour
{
    public UnityEvent my_event;

    Animator animator;
    Domkrat parentDomkrat;
    public GameObject ruchka;
    public Makes curPosition;

    public GameObject TechStand;
    [SerializeField] Animator TPKAnim;


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
        // should always be `DOWN` at Start
        curPosition = Makes.DOWN;
    }

    void HideFinger()
    {
        my_event.Invoke();
    }

    public void Up(bool isOnWeightMode = false)
    {
        UpdateTestingDict(Makes.UP, isOnWeightMode);
        if (parentDomkrat.isAttachedToTPK)
        {
            if (TPK.TPKObj.attachedDomkrats.Count < 4)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установите все домкраты, чтобы поднять ТПК", Weight = ErrorWeight.HIGH });
                return;
            }
            if (!isOnWeightMode)
            {
                // Пытаемся поднять домкрат в режиме "без груза" с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Неправильный режим для подъема ТПК", Weight = ErrorWeight.HIGH });
                return;
            }
            if (Singleton.Instance.StateManager.ruchkaIsUp < 4)
            {
                // Ручки не в исходном положении
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Верните все ручки в исходное положение", Weight = ErrorWeight.MEDIUM });
                return;
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
        UpdateTestingDict(Makes.DOWN, isOnWeightMode);
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isOnWeightMode)
            {
                // Пытаемся опустить домкрат в режиме "без груза" с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Неправильный режим для опускания ТПК", Weight = ErrorWeight.HIGH });
                return;
            }
            if (Singleton.Instance.StateManager.GetState() == NameState.DISABLE_TORMOZ && Singleton.Instance.StateManager.isTormozConnected != 0)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Перед опусканием ТПК сначала отключите тормоза", Weight = ErrorWeight.LOW });
                return;
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

    public void RealUp(bool liftTPK=true)
    {
        curPosition = Makes.UP;
        animator.SetTrigger("Up"); // анимация подъема самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Up"); // анимация вращения ручки
        if (liftTPK)
        {
            TPKAnim.SetTrigger("Up"); // анимация ПОДЪЕМА ТПК БЛЯТЬ (да-да не удивляйтесь)
            TPK.TPKObj.LiftUp();
        }
    }

    public void RealDown(bool liftTPK=true)
    {
        curPosition = Makes.DOWN;
        animator.SetTrigger("Down"); // анимация опускания (по масти) самого домкрата
        ruchka.GetComponent<Animator>().SetTrigger("Down"); // анимация вращения ручки
        if (liftTPK)
        {
            TPKAnim.SetTrigger("Down"); // анимация ОПУСКАНИЯ ТПК БЛЯТЬ (да-да не удивляйтесь)
            TPK.TPKObj.LiftDown();
        }
    }

    public void OnTechStand()
    {
        TechStand.SetActive(true);
    }

    public void OffTechStand()
    {
        TechStand.SetActive(false);
    }
}
