using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_part_rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (curPosition == Makes.UP)
        {
            if (!isRotate)
            {
                if (Input.GetKeyDown(KeyCode.E)) // 200$ c Vladika
                {
                    //gameObject.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y"), Space.World);
                    StartCoroutine(Rotate(90));
                }
            }

        }

    }

    public IEnumerator Rotate(float angel)
    {
        float temp = angel >= 0 ? -1 : 1;
        isRotate = true;
        for (float t = 0; t <= Mathf.Abs(angel); t += step * Time.deltaTime)
        {
            gameObject.transform.Rotate(0f, step * temp * Time.deltaTime, 0f);
            yield return null;
        }
        isRotate = false;
    }
}
