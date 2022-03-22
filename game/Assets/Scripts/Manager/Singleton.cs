using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public UIManager UIManager { get; private set; }
    public StateManager StateManager { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        UIManager = GetComponentInChildren<UIManager>();
        StateManager = GetComponent<StateManager>();
    }
}
