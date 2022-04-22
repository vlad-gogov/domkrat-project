using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    Toggle AdaptiveRes;
    Dropdown dropDownResolution;
    Dropdown dropDownQuailty;
    Resolution[] res;
    string[] quality;

    void Awake()
    {
        AdaptiveRes = transform.GetChild(0).GetComponent<Toggle>();
        dropDownResolution = transform.GetChild(1).GetComponent<Dropdown>();
        dropDownQuailty = transform.GetChild(2).GetComponent<Dropdown>();

        res = Screen.resolutions;
        List<string> resText = new List<string>();
        foreach(var a in res)
        {
            resText.Add(a.width.ToString() + " x " + a.height.ToString());
        }
        dropDownResolution.AddOptions(resText);
        quality = QualitySettings.names;
        dropDownQuailty.AddOptions(new List<string>(quality));
    }

    void Start()
    {
        dropDownResolution.gameObject.transform.GetChild(0).GetComponent<Text>().text = Screen.width.ToString() + " x " + Screen.height.ToString();
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].width == Screen.width && res[i].height == Screen.height)
            {
                dropDownResolution.value = i;
                break;
            }
        }
        dropDownQuailty.value = QualitySettings.GetQualityLevel();
        dropDownQuailty.gameObject.transform.GetChild(0).GetComponent<Text>().text = quality[dropDownQuailty.value];
    }

    public void onClickAdaptiveResoulution()
    {
        CrossScenesStorage.isAdaptiveResoulution = !CrossScenesStorage.isAdaptiveResoulution;
        AdaptiveRes.isOn = CrossScenesStorage.isAdaptiveResoulution;
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(res[dropDownResolution.value].width, res[dropDownResolution.value].height, true);
        CrossScenesStorage.resolution = res[dropDownResolution.value];
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(dropDownQuailty.value, true);
    }


}
