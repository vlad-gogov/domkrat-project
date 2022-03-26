using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Настроить корутину и разобрать все возможные случаи установки домкарата в переходники
/// </summary>

public class Domkrat : MonoBehaviour
{

    private float SpeedRotation = 80f;
    private float SpeedMove = 80f;
    private Vector3 prev;
    [SerializeField] private GameObject LeftWheel;
    [SerializeField] private GameObject RightWheel;
    [SerializeField] private GameObject BackWheel;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void RotateWheel()
    {
        float temp = transform.position.x - prev.x;
        if (temp != 0)
        {
            float rot = (temp > 0 ? SpeedRotation : -SpeedRotation) * Time.deltaTime;
            LeftWheel.transform.Rotate(0f, rot, 0f);
            RightWheel.transform.Rotate(0f, rot, 0f);
            BackWheel.transform.Rotate(0f, 0f, -rot);
        }
        prev = transform.position;
    }

    public bool Set(Collider trigger)
    {
        GameObject parent = trigger.gameObject;
        if (parent.tag == "SetPerehodnickDomkrat" && Input.GetKeyDown(KeyCode.E))
        {
            GameObject BeginPoint = parent.transform.GetChild(0).gameObject;
            GameObject EndPoint = parent.transform.GetChild(1).gameObject;
            transform.position = BeginPoint.transform.position;
            transform.rotation = new Quaternion(transform.rotation.x, BeginPoint.transform.rotation.y, BeginPoint.transform.rotation.z, BeginPoint.transform.rotation.w);
            GetComponent<Rigidbody>().isKinematic = true;
            float begin = BeginPoint.transform.position.z <= EndPoint.transform.position.z ? BeginPoint.transform.position.z : EndPoint.transform.position.z;
            float end = BeginPoint.transform.position.z >= EndPoint.transform.position.z ? BeginPoint.transform.position.z : EndPoint.transform.position.z;
            Debug.Log(begin +  " " + end);
            StartCoroutine(MoveSet(begin, end));
            GetComponent<BoxCollider>().enabled = false;
            return true;
        }
        return false;
    }

    IEnumerator MoveSet(float begin, float end)
    {
        Debug.Log(end - begin);
        for(float i = 0; i <= end - begin; i += SpeedMove * Time.deltaTime)
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime);
            Debug.Log(gameObject.transform.position.z);
            yield return null;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (Set(collider))
        {
            collider.enabled = false;
            PlayerRay.playerRay.UnSelectable();
        }
    }

    void Update()
    {
        //RotateWheel();
    }
}
