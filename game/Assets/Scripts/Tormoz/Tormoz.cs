using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tormoz : MonoBehaviour
{
    public static Tormoz tormoz { get; private set; }

    public TormozMovingHand tormozMovingHand;

    public bool isUse = false;
    public bool isSet = false;

    void Awake()
    {
        tormoz = this;
        // ����� ��� ��� ��������������� � ���� ���� ��Ш�!!!
        // tormoz.gameObject.SetActive(false);
    }

}
