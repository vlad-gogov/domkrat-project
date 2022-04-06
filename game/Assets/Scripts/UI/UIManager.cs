using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public Text enter;
    public Text helper;
    public GameObject enterBox;
    public GameObject scrollView;
    TutorialBar tutorial;
    bool finished = false;

    void Awake()
    {
        Debug.Log("starting UI manager...");
        tutorial = new TutorialBar(scrollView);
    }

    void Start()
    {
        if (Singleton.Instance.StateManager.gameMode == GameMode.EXAM)
        {
            helper.GetComponentInParent<UnityEngine.UI.Image>().gameObject.SetActive(false);
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

    public void OpenTutorial(string tutor, bool finished=false)
    {
        scrollView.SetActive(true);
        Debug.Log(tutorial);
        this.finished = finished;
        tutorial.Show(tutor);
        Singleton.Instance.StateManager.Pause();
    }

    public void CloseTutorial()
    {
        if (finished)
        {
            SceneManager.LoadScene("Menu");
        }
        scrollView.SetActive(false);
        tutorial.Hide();
        Singleton.Instance.StateManager.Resume();
        Singleton.Instance.StateManager.InitialStateHack();
    }
}
