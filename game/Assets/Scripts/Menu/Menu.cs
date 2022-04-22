using UnityEngine;
using UnityEngine.SceneManagement;


public class CrossScenesStorage
{
    public static GameMode gameMode;
    public static TypeArea typeArea;
    public static Resolution resolution;
    public static bool isAdaptiveResoulution = false;
}

public partial class Menu : MonoBehaviour
{
    GameObject MainMenu;
    GameObject OptionMenu;

    void Start()
    {
        // Screen.SetResolution(1920, 1080, true);
        MainMenu = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        OptionMenu = gameObject.transform.GetChild(1).GetChild(1).gameObject;
    }

    public void onClickTheory()
    {
        CrossScenesStorage.gameMode = GameMode.TRAIN;
        SceneManager.LoadScene("MenuSurface");
    }

    public void onClickExam()
    {
        CrossScenesStorage.gameMode = GameMode.EXAM;
        SceneManager.LoadScene("MenuSurface");
    }

    public void onClickTest()
    {
        SceneManager.LoadScene("Testing");
    }

    public void onClickOption()
    {
        MainMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void onExitOption()
    {
        MainMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void onClickExit()
    {
        Application.Quit();
    }

};
