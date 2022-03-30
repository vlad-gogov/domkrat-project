using UnityEngine;
using UnityEngine.SceneManagement;


public partial class Menu : MonoBehaviour
{
    public void onClickFlat()
    {
        Debug.Log("Launching flat");
        CrossScenesStorage.typeArea = TypeArea.FLAT;
        SceneManager.LoadScene("Flat");
    }

    public void onClickUp()
    {
        Debug.Log("Launching up");
        CrossScenesStorage.typeArea = TypeArea.UP;
        SceneManager.LoadScene("Flat");
    }

    public void onClickDown()
    {
        Debug.Log("Launching down");
        CrossScenesStorage.typeArea = TypeArea.DOWN;
        SceneManager.LoadScene("Flat");
    }

    public void onClickBack()
    {
        SceneManager.LoadScene("Menu");
    }


};
