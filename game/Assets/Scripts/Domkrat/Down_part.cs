using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_part : MonoBehaviour
{
    Animator animator;
    Domkrat parentDomkrat;
    public GameObject ruchka;
    public Makes curPosition;
    private float step = 40f;
    private bool isRotate = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        parentDomkrat = gameObject.transform.parent.GetComponent<Domkrat>();
        // should always be `DOWN` at Start
        curPosition = Makes.DOWN;
    }

    public void Up(bool isTechStand = false)
    {
        curPosition = Makes.UP;
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isTechStand)
            {
                // Пытаемся поднять нижнюю часть домкрата без технологической подставки с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установить технологическую подставку перед тем как поднимать нижнюю часть домкарата", Weight = ErrorWeight.HIGH });
                return;
            }
            RealUp();
        }
    }

    public void Down(bool isTechStand = false)
    {
        curPosition = Makes.DOWN;
        if (parentDomkrat.isAttachedToTPK)
        {
            if (!isTechStand)
            {
                // Пытаемся поднять нижнюю часть домкрата без технологической подставки с подключенным ТПК
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Установить технологическую подставку перед тем как поднимать нижнюю часть домкарата", Weight = ErrorWeight.HIGH });
                return;
            }
            RealDown();
        }
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
            if (!isRotate)
            {
                if (Input.GetKeyDown(KeyCode.E)) // 200$ c Vladika
                {
                    //gameObject.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y"), Space.World);
                    
                }
            }

        }
      
    }
}
