using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPK : MonoBehaviour
{
    // Какие операции синхронить между домкратами:
    // 1. Подъем/опуск
    // 2. Смена режимов на всём (ручка)
    // 3. САСного/рояльные положения ТОЛЬКО МЕЖДУ ПАРАЛЛЕЛЬНЫМИ домкратами
    // 4. Синхронить повороты нижних частей
    public List<Domkrat> attachedDomkrats;

    public static TPK TPKObj { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TPKObj = this;
        attachedDomkrats = new List<Domkrat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LiftUp()
    {
        foreach (var domkrat in attachedDomkrats)
        {
            domkrat.LiftUp(/*liftTPK=*/false);
        }
    }

    public void LiftDown()
    {
        foreach (var domkrat in attachedDomkrats)
        {
            domkrat.LiftDown(/*liftTPK=*/false);
        }
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
}
