﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public float TimeLogo;

    void Start()
    {
        var resulutions = Screen.resolutions;
        // Устанавливаем максимально возможное разрешение при запуске (поумолчанию, он запускается при том, при котором была закрыта игра)
        Screen.SetResolution(resulutions[resulutions.Length - 1].width, resulutions[resulutions.Length - 1].height, true);
        StartCoroutine(WaitLogo(TimeLogo));
    }

    IEnumerator WaitLogo(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu");
    }
}
