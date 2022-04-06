using UnityEngine;
using UnityEngine.SceneManagement;


public partial class Menu : MonoBehaviour
{
    public void onClickFlat()
    {
        CrossScenesStorage.typeArea = TypeArea.FLAT;
        SceneManager.LoadScene("Flat");
    }

    public void onClickUp()
    {
        CrossScenesStorage.typeArea = TypeArea.UP;
        SceneManager.LoadScene("UpBlyat");
    }

    public void onClickDown()
    {
        CrossScenesStorage.typeArea = TypeArea.DOWN;
        SceneManager.LoadScene("DownScene");
    }

    public void onClickBack()
    {
        SceneManager.LoadScene("Menu");
    }


};
