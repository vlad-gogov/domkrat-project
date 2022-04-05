using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tormoz : MonoBehaviour
{
    public static Tormoz tormoz { get; private set; }

    public TormozMovingHand tormozMovingHand;

    public bool isUse = false;

    void Start()
    {
        tormoz = this;
        tormoz.gameObject.SetActive(false);
    }
    

}
