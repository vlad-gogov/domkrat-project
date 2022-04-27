using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDialog : MonoBehaviour
{
    public static ExitDialog instance { get; private set; }

    public ExitDialog()
    {
        instance = this;
    }

    Action callbackOnNo;

    public void OnShow(Action callback = null)
    {
        gameObject.SetActive(true);
        callbackOnNo = callback;
    }

    public void OnYes()
    {
        // Чтобы при следующем заходе на сцену паузы не было поумолчанию
        Singleton.Instance.StateManager.Resume();
        SceneManager.LoadScene("Menu");
    }

    public void OnNo()
    {
        gameObject.SetActive(false);
        if (callbackOnNo != null)
        {
            callbackOnNo();
        }
    }
}