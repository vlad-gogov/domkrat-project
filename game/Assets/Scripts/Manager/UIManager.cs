using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text enter;
    public Text helper;

    void Start()
    {
        if (Singleton.Instance.StateManager.gameMode == GameMode.EXAM)
        {
            helper.GetComponentInParent<Image>().gameObject.SetActive(false);
        }
    }

    public void SetEnterText(string text)
    {
        enter.text = text;
    }

    public void ClearEnterText()
    {
        enter.text = "";
    }

    public void SetHelperText(string text)
    {
        helper.text = text;
    }

    public void ClearHelperText()
    {
        enter.text = "";
    }
}
