using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateTPK
{
    UP = 0,
    DOWN = 1
}

public class TPK : MonoBehaviour
{
    // Какие операции синхронить между домкратами:
    // 1. Подъем/опуск
    // 2. Смена режимов на всём (ручка)
    // 3. САСного/рояльные положения ТОЛЬКО МЕЖДУ ПАРАЛЛЕЛЬНЫМИ домкратами
    // 4. Синхронить повороты нижних частей
    public List<Domkrat> attachedDomkrats;
    List<TechStand> techStands = new List<TechStand>();

    public StateTPK state;

    public static TPK TPKObj { get; private set; }

    void Start()
    {
        TPKObj = this;
        attachedDomkrats = new List<Domkrat>();
        state = StateTPK.DOWN;
    }

    void Update()
    {
        if (techStands.Count == 0 && Singleton.Instance.StateManager.GetState() == NameState.UP_TPK)
        {
            foreach(var techSand in attachedDomkrats)
            {
                techStands.Add(techSand.techStand);
            }
        }
    }

    public void LiftUp()
    {
        state = StateTPK.UP;
        Singleton.Instance.StateManager.NextState();
        foreach (var domkrat in attachedDomkrats)
        {
            domkrat.LiftUp(/*liftTPK=*/false);
        }
    }

    public void LiftDown()
    {
        state = StateTPK.DOWN;
        foreach (var domkrat in attachedDomkrats)
        {
            domkrat.LiftDown(/*liftTPK=*/false);
        }
    }

    public void SwitchMovingThings(bool flag)
    {
        var tpkObj = transform.parent.parent.gameObject;
        foreach (var col in tpkObj.GetComponents<BoxCollider>())
        {
            col.enabled = flag;
        }
        tpkObj.GetComponent<Rigidbody>().useGravity = flag;
        // GetComponent<BoxCollider>().enabled = !flag;
        GetComponent<Rigidbody>().useGravity = !flag;
    }

    public int AddDomkrat(Domkrat domkrat)
    {
        attachedDomkrats.Add(domkrat);
        // Вернуть индекс домкрата потребителю, чтобы он потом мог удалить себя по этому индексу
        Debug.Log("Domkrat dobavlen: " + attachedDomkrats.Count.ToString());
        return attachedDomkrats.Count - 1;
    }

    public void RemoveDomkrat(int id)
    {
        attachedDomkrats.RemoveAt(id);
    }

    public void SwtichTechStand(bool Signal)
    {
        foreach (var techStand in techStands)
        {
            if(!techStand.isSelected)
            {
                techStand.gameObject.GetComponent<BoxCollider>().enabled = Signal;
            }
        }
    }
}
