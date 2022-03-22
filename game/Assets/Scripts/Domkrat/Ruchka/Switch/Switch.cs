using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Switch : MonoBehaviour
{
    public TypeMode curType = TypeMode.Off;
    public float[] rotate;
    private float step = 40f;

    public IEnumerator RotateSwitch(float angel)
    {
        float temp = angel >= 0 ? -1 : 1;
        for(float t = 0; t <= Mathf.Abs(angel); t += step * Time.deltaTime)
        {
            gameObject.transform.Rotate(0f, 0f, step * temp * Time.deltaTime);
            yield return null;
        }
    }

    public void ChangeState(TypeMode nextType)
    {
        int currentIndex = (int)curType;
        int nextIndex = (int)nextType;
        StartCoroutine(RotateSwitch(rotate[currentIndex] - rotate[nextIndex]));
        curType = nextType;
    }

    public new TypeMode GetType()
    {
        return curType;
    }
}
