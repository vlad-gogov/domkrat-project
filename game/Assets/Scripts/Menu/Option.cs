using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    Toggle AdaptiveRes;
    Dropdown dropDown;
    Resolution[] res;

    void Awake()
    {
        AdaptiveRes = transform.GetChild(0).GetComponent<Toggle>();
        dropDown = transform.GetChild(1).GetComponent<Dropdown>();

        res = Screen.resolutions;
        List<string> resText = new List<string>();
        foreach(var a in res)
        {
            resText.Add(a.width.ToString() + " x " + a.height.ToString());
        }
        dropDown.AddOptions(resText);
    }

    void Start()
    {
        dropDown.gameObject.transform.GetChild(0).GetComponent<Text>().text = Screen.width.ToString() + " x " + Screen.height.ToString();
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].width == Screen.width && res[i].height == Screen.height)
            {
                dropDown.value = i;
                break;
            }
        }
    }

    public void onClickAdaptiveResoulution()
    {
        CrossScenesStorage.isAdaptiveResoulution = !CrossScenesStorage.isAdaptiveResoulution;
        AdaptiveRes.isOn = CrossScenesStorage.isAdaptiveResoulution;
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(res[dropDown.value].width, res[dropDown.value].height, true);
        CrossScenesStorage.resolution = res[dropDown.value];
    }


}
