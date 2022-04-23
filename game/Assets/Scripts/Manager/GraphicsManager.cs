using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    static float resolutionScaler = 1.0f;
    static float EPS = 10e-6f;

    public static void LowerSettings()
    {
        // Если настройки графики минимальные, то уменьшаем разрешение
        if (QualitySettings.GetQualityLevel() != 0) 
        {
            LowerQuality();
        }
        else
        {
            LowerResolition();
        }
    }

    public static void IncreaseSettings()
    {
        // В первую очередь увеличиваем разрешение - потом графику
        if (Math.Abs(resolutionScaler - 1.0f) < EPS)
        {
            IncreaseQuality();
        }
        else
        {
            IncreaseResolution();
        }
    }

    // Пока что не работает
    public static void LowerResolition()
    {
        resolutionScaler = Math.Max(0.5f, resolutionScaler - 0.1f);
        // ScalableBufferManager.ResizeBuffers(resolutionScaler, resolutionScaler);
    }

    // Пока что не работает
    public static void IncreaseResolution()
    {
        resolutionScaler = Math.Min(1.0f, resolutionScaler + 0.1f);
        // ScalableBufferManager.ResizeBuffers(resolutionScaler, resolutionScaler);
    }

    public static void LowerQuality()
    {
        int curLevel = QualitySettings.GetQualityLevel();
        if (curLevel == 0)
        {
            return;
        }
        QualitySettings.SetQualityLevel(curLevel - 1);
    }

    public static void IncreaseQuality()
    {
        int curLevel = QualitySettings.GetQualityLevel();
        if (curLevel == QualitySettings.names.Length - 1)
        {
            return;
        }
        QualitySettings.SetQualityLevel(curLevel + 1);
    }
}
