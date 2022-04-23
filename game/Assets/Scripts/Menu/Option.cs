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
    ushort shiftResoluiton = 0;
    bool doPerformActionOnToggle = true;

    void Awake()
    {
        AdaptiveRes = transform.GetChild(0).GetComponent<Toggle>();
        dropDownResolution = transform.GetChild(1).GetComponent<Dropdown>();
        dropDownQuailty = transform.GetChild(2).GetComponent<Dropdown>();

        res = Screen.resolutions;
        List<string> resText = new List<string>();
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].width < 800)
            {
                shiftResoluiton++;
            }
            else
            {
                resText.Add(res[i].width.ToString() + " x " + res[i].height.ToString());
            }
        }
        dropDownResolution.AddOptions(resText);
        quality = QualitySettings.names;
        dropDownQuailty.AddOptions(new List<string>(quality));
    }

    void Start()
    {
        dropDownResolution.gameObject.transform.GetChild(0).GetComponent<Text>().text = Screen.width.ToString() + " x " + Screen.height.ToString();
        for (int i = shiftResoluiton; i < res.Length; i++)
        {
            if (res[i].width == CrossScenesStorage.resolution.width && res[i].height == CrossScenesStorage.resolution.height)
            {
                dropDownResolution.value = i - shiftResoluiton;
                break;
            }
        }
        dropDownQuailty.value = QualitySettings.GetQualityLevel();
        dropDownQuailty.gameObject.transform.GetChild(0).GetComponent<Text>().text = quality[dropDownQuailty.value];

        // 'doPerformActionOnToggle' - костыль, изменение .isOn у Toggle триггерит эвент onValueChanged,
        // чего мы не хотим при инициализации Toggl'а значением
        doPerformActionOnToggle = false;
        AdaptiveRes.isOn = CrossScenesStorage.isAdaptiveResoulution;
        // Эта строчка тоже очень важная, не трогайте её
        doPerformActionOnToggle = true;
    }

    public void onClickAdaptiveResoulution()
    {
        Debug.Log($"onClickAdaptiveResoulution() : doPerformActionOnToggle = {doPerformActionOnToggle}");
        if (!doPerformActionOnToggle)
        {
            doPerformActionOnToggle = true;
            return;
        }
        CrossScenesStorage.isAdaptiveResoulution = !CrossScenesStorage.isAdaptiveResoulution;
    }

    public void ChangeResolution()
    {
        CrossScenesStorage.resolution = res[dropDownResolution.value + shiftResoluiton];
        Screen.SetResolution(CrossScenesStorage.resolution.width, CrossScenesStorage.resolution.height, true);
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(dropDownQuailty.value, true);
    }


}
