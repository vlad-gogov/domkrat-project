using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField] Text erroText;

    public void OnShow(string textError)
    {
        erroText.text = textError;
        gameObject.SetActive(true);
        Singleton.Instance.StateManager.Pause();
    }

    public void Close()
    {
        Singleton.Instance.StateManager.Resume();
        erroText.text = "";
        gameObject.SetActive(false);
    }
}
