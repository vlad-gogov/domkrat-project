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
        Time.timeScale = 0;
    }

    public void Close()
    {
        Time.timeScale = 1;
        erroText.text = "";
        gameObject.SetActive(false);
    }
}
