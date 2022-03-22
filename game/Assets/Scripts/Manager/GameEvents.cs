using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onRuchkaClickUp;
    public void RuchkaClickUp()
    {
        onRuchkaClickUp?.Invoke();
    }

    public event Action onRuchkaClickDown;
    public void RuchkaClickDown()
    {
        onRuchkaClickDown?.Invoke();
    }

}
