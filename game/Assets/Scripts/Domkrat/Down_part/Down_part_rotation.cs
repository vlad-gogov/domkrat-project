﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    FORWARD = 0,
    RIGHT = 90,
    BACK = 180,
    LEFT = 270
}

public class Down_part_rotation : MonoBehaviour
{
    public bool isRotate = false;
    private float step = 40f;
    public Direction dir = Direction.FORWARD;

    
    public void RotateDownPart(float angle)
    {
        StartCoroutine(Rotate(angle));
    }

    public IEnumerator Rotate(float angle)
    {
        float temp = angle >= 0 ? 1 : -1;
        isRotate = true;
        float prevState = gameObject.transform.localEulerAngles.y;

        for (float t = 0; t <= Mathf.Abs(angle); t += step * Time.deltaTime)
        {
            gameObject.transform.Rotate(step * temp * Time.deltaTime, 0f, 0f);
            yield return null;
        }
        isRotate = false;
        gameObject.transform.localEulerAngles = new Vector3(0f, prevState + angle, gameObject.transform.localEulerAngles.z);
        ChangeDir();
    }
    
    private void ChangeDir()
    {
        int temp = (int)gameObject.transform.localEulerAngles.y;
        int transformY = temp >= 0 ? temp % 360 : -temp % 360;
        dir = (Direction)transformY;
    }
}
