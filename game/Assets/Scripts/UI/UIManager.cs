using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public Text enter;
    public Text helper;
    public GameObject enterBox;
    public GameObject scrollView;
    TutorialBar tutorial;

    void Start()
    {
        if (Singleton.Instance.StateManager.gameMode == GameMode.EXAM)
        {
            helper.GetComponentInParent<UnityEngine.UI.Image>().gameObject.SetActive(false);
        }
        tutorial = new TutorialBar(scrollView);
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

    public void OpenTutorial(string tutor)
    {
        scrollView.SetActive(true);
        tutorial.Show(tutor);
        Singleton.Instance.StateManager.Pause();
    }

    public void CloseTutorial()
    {
        scrollView.SetActive(false);
        tutorial.Hide();
        Singleton.Instance.StateManager.Resume();
        Singleton.Instance.StateManager.InitialStateHack();
    }
}
