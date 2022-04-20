using UnityEngine;
using UnityEngine.SceneManagement;


public class CrossScenesStorage
{
    public static GameMode gameMode;
    public static TypeArea typeArea;
}

public partial class Menu : MonoBehaviour
{
    void Start()
    {
        // Screen.SetResolution(1920, 1080, true);
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

    public void onClickExit()
    {
        Application.Quit();
    }


};
