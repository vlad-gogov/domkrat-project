using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum PauseDialogState
{
    PAUSE,
    SETTINGS,
    EXIT
}

public class PauseDialog : MonoBehaviour
{
    GameObject pauseMenu, settingsMenu;

    PauseDialogState state = PauseDialogState.PAUSE;

    private void Start()
    {
        pauseMenu = gameObject.transform.GetChild(0).gameObject;
        settingsMenu = gameObject.transform.GetChild(1).gameObject;
    }

    public void OnShow()
    {
        gameObject.SetActive(true);
        Singleton.Instance.StateManager.Pause(/*stopTime=*/true);
        state = PauseDialogState.PAUSE;
    }

    public void OnContinue()
    {
        Singleton.Instance.StateManager.Resume();
        gameObject.SetActive(false);
    }

    public void OnSettings()
    {
        state = PauseDialogState.SETTINGS;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<Option>().InitializeValues();
    }

    public void OnCloseSettings()
    {
        state = PauseDialogState.PAUSE;
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void OnExit()
    {
        state = PauseDialogState.EXIT;
        ExitDialog.instance.OnShow(()=> { this.pauseMenu.SetActive(true); state = PauseDialogState.PAUSE; });
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case PauseDialogState.PAUSE:
                    OnContinue();
                    break;
                case PauseDialogState.SETTINGS:
                    OnCloseSettings();
                    break;
                case PauseDialogState.EXIT:
                    ExitDialog.instance.OnNo();
                    break;
            }
        }
    }
}
