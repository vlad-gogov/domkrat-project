using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModeSwitch {
    LOADED = 0,
    WITHOUTLOAD = 1
}
public class Switch : MonoBehaviour
{
    public ModeSwitch mode;
    public TypeMode curType = TypeMode.Off;
    public float[] rotate;
    private float speedRotation = 40f;

    public IEnumerator RotateSwitch(float angel)
    {
        float temp = angel >= 0 ? -1 : 1;
        Vector3 prev = gameObject.transform.eulerAngles;
        for(float t = 0; t <= Mathf.Abs(angel); t += speedRotation * Time.deltaTime)
        {
            gameObject.transform.Rotate(0f, 0f, speedRotation * temp * Time.deltaTime);
            yield return null;
        }
        Vector3 newAngles = new Vector3(0, 0, Mathf.Abs(angel));
        gameObject.transform.eulerAngles = prev + temp * newAngles;
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
