using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public UIManager UIManager { get; private set; }
    public StateManager StateManager { get; private set; }
    public static Singleton Timer { get; private set; }
    private void Awake()
    {
        Timer = this;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        UIManager = GetComponentInChildren<UIManager>();
        StateManager = GetComponent<StateManager>();
    }

    public void SetTimer(float seconds, Action func)
    {
        StartCoroutine(Delay(seconds, func));
    }

    public IEnumerator Delay(float seconds, Action func)
    {
        yield return new WaitForSeconds(seconds);
        func();
    }
}
