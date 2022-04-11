using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    Text erroText;
    Text mistake;

    StateManager stateManager;
    
    void Start()
    {
        gameObject.SetActive(true);
        erroText = transform.GetChild(0).GetComponent<Text>();
        mistake = transform.GetChild(2).GetComponent<Text>();
        stateManager = Singleton.Instance.StateManager;
        gameObject.SetActive(false);
    }

    public void OnShow(Error error)
    {
        erroText.text = error.ErrorText;
        if (error.Weight == ErrorWeight.MINOR)
        {
            gameObject.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 0.81f);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(0.6792453f, 0f, 0f, 0.7f);
        }
        if (stateManager.gameMode == GameMode.EXAM && error.Weight != ErrorWeight.MINOR)
        {
            mistake.text = "Колличество штрафных баллов: " + stateManager.counterMistakes;
        }
        gameObject.SetActive(true);
        Singleton.Instance.StateManager.Pause();
    }

    public void Close()
    {
        Singleton.Instance.StateManager.Resume();
        Singleton.Instance.StateManager.isErrorOpened = false;
        erroText.text = "";
        mistake.text = "";
        gameObject.SetActive(false);
    }
}
