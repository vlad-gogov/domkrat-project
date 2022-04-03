using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    FORWARD = 0,
    RIGHT = 90,
    BACK = 180,
    LEFT = 270
}

public enum WheelState
{
    ROYAL = 0,
    SOOS = 1
}

public class Down_part_rotation : MonoBehaviour
{
    public bool isRotate = false;
    private float step = 40f;
    public Direction dir = Direction.FORWARD;
    public WheelState currentWheelState = WheelState.SOOS;
    [SerializeField] GameObject gear;

    public void RotateDownPart(float angle, bool isGear = false, float k = 1.0f)
    {
        StartCoroutine(Rotate(angle, isGear, k));
    }

    public IEnumerator Rotate(float angle, bool isGear, float k = 1.0f)
    {
        float signed = angle >= 0 ? 1 : -1;
        isRotate = true;
        float prevState = gameObject.transform.localEulerAngles.y;

        for (float t = 0; t <= Mathf.Abs(angle); t += step * Time.deltaTime * k)
        {
            float temp = step * signed * Time.deltaTime * k;
            gameObject.transform.Rotate(temp, 0f, 0f);
            if (isGear)
            {
                gear.transform.Rotate(temp, 0, 0f);
            }
            yield return null;
        }
        isRotate = false;
        gameObject.transform.localEulerAngles = new Vector3(0f, prevState + angle, gameObject.transform.localEulerAngles.z);
        ChangeDir();
    }
    
    public void ChangeDir()
    {
        int temp = (int)gameObject.transform.localEulerAngles.y;
        int transformY = temp >= 0 ? temp % 360 : -temp % 360;
        dir = (Direction)transformY;
        if (currentWheelState == WheelState.ROYAL)
        {
            if (dir == Direction.FORWARD)
            {
                dir = Direction.BACK;
            }
            else if (dir == Direction.BACK)
            {
                dir = Direction.FORWARD;
            }
            else if (dir == Direction.LEFT)
            {
                dir = Direction.RIGHT;
            }
            else if (dir == Direction.RIGHT)
            {
                dir = Direction.LEFT;
            }
        }
    }
}
