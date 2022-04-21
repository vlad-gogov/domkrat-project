using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDialog : MonoBehaviour
{
    public void OnShow()
    {
        gameObject.SetActive(true);
        Singleton.Instance.StateManager.Pause();
    }

    public void OnYes()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnNo()
    {
        Singleton.Instance.StateManager.Resume();
        gameObject.SetActive(false);
    }
}
