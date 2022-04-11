using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    FORWARD = 0,
    LEFT = 90,
    BACK = 180,
    RIGHT = 270
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
    [SerializeField] BoxCollider tormozConnector;
    DomkratMoving domkratMoving;
    TormozSwitcher tormozSwitcher;

    void Start()
    {
        tormozConnector.enabled = false;
        domkratMoving = gameObject.transform.parent.parent.GetComponent<DomkratMoving>();
        tormozSwitcher = gameObject.transform.GetChild(1).GetComponent<TormozSwitcher>();
        ChangeDir();
    }

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
            float temp = step * signed * Time.deltaTime;
            gameObject.transform.Rotate(temp * k, 0f, 0f);
            if (isGear)
            {
                gear.transform.Rotate(temp * k, 0, 0f);
                domkratMoving.RotateWheelForUpdate(temp, false);
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
    }

    public void SwitchBoxColliderTormozConnector()
    {
        tormozConnector.enabled = !tormozConnector.enabled;
    }

    public void EndAnimTormozSwitcher()
    {
        tormozSwitcher.isAnim = false;
    }

    public void EndAnimSwitchRoyal()
    {
        tormozSwitcher.switchRoyal.isAnim = false;
    }
}
