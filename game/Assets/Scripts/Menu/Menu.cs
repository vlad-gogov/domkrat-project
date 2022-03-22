using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public void onClickTheory()
    {
        SceneManager.LoadScene(1);
    }

    public void onClickExam()
    {
        SceneManager.LoadScene(1);
    }

    public void onClickTest()
    {
        SceneManager.LoadScene(1);
    }

    public void onClickExit()
    {
        Application.Quit();
    }


};
