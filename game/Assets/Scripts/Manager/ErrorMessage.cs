using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    Text erroText;
    Text mistake;

    StateManager stateManager;
    
    void Awake()
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
            gameObject.GetComponent<Image>().color = new Color(183, 183, 183, 105);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(200, 0, 0, 105);
        }
        if (stateManager.gameMode == GameMode.EXAM)
        {
            mistake.text = "Количество набранных баллов за ошибки: " + stateManager.counterMistakes;
        }
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
