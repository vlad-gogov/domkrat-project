using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public float TimeLogo;

    void Start()
    {
        StartCoroutine(WaitLogo(TimeLogo));
    }

    IEnumerator WaitLogo(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu");
    }
}
