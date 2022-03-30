using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text enter;
    public Text helper;
    public GameObject enterBox;

    void Start()
    {
        if (Singleton.Instance.StateManager.gameMode == GameMode.EXAM)
        {
            helper.GetComponentInParent<Image>().gameObject.SetActive(false);
        }
    }

    public void SetEnterText(string text)
    {
        enterBox.SetActive(true);
        enter.text = text;
    }

    public void ClearEnterText()
    {
        enterBox.SetActive(false);
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
