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
        for (float t = 0; t <= Mathf.Abs(angle); t += step * Time.deltaTime)
        {
            gameObject.transform.Rotate(step * temp * Time.deltaTime, 0f, 0f);
            yield return null;
        }
        isRotate = false;
        Debug.Log(gameObject.transform.rotation);
        // Здесь нужна строчка для установки поворота на точный угол
        //gameObject.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.rotation.z + temp * angle);
        ChangeDir();
    }
    
    private void ChangeDir()
    {
        int transformY = (int)gameObject.transform.rotation.y % 360;
        dir = (Direction)transformY;
    }
}
