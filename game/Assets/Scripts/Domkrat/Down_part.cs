using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_part : MonoBehaviour
{
    Animator animator;
    Domkrat parentDomkrat;
    public GameObject ruchka;
    public Makes curPosition;

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
                // �������� ������� ������ ����� �������� ��� ��������������� ��������� � ������������ ���
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "���������� ��������������� ��������� ����� ��� ��� ��������� ������ ����� ���������", Weight = ErrorWeight.HIGH });
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
                // �������� ������� ������ ����� �������� ��� ��������������� ��������� � ������������ ���
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "���������� ��������������� ��������� ����� ��� ��� ��������� ������ ����� ���������", Weight = ErrorWeight.HIGH });
                return;
            }
            RealDown();
        }
    }

    void RealUp()
    {
        curPosition = Makes.UP;
        animator.SetTrigger("Up"); // �������� ������� ������ ����� ��������
        ruchka.GetComponent<Animator>().SetTrigger("Up"); // �������� �������� �����
    }

    void RealDown()
    {
        curPosition = Makes.DOWN;
        animator.SetTrigger("Down"); // �������� ��������� (�� �����) ������ ����� ��������
        ruchka.GetComponent<Animator>().SetTrigger("Down"); // �������� �������� �����
    }
}
