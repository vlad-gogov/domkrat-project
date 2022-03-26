using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domkrat : MonoBehaviour
{

    private float SpeedRotation = 80f;
    private float SpeedMove = 0.1f;
    private Vector3 prev;
    [SerializeField] private GameObject LeftWheel;
    [SerializeField] private GameObject RightWheel;
    [SerializeField] private GameObject BackWheel;
    [SerializeField] private GameObject MovingMechanism;

    void Start()
    {
        //anim = GetComponent<Animator>();
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
            GetComponent<BoxCollider>().enabled = false;
            float begin = BeginPoint.transform.position.z;
            float end = EndPoint.transform.position.z;
            StartCoroutine(MoveSet(end - begin));
            GetComponent<BoxCollider>().enabled = false;

            return true;
        }
        return false;
    }

    IEnumerator MoveSet(float delta)
    {
        float shift = SpeedMove * Time.deltaTime;
        for (float i = 0; i <= Mathf.Abs(delta); i += shift)
        {
            gameObject.transform.Translate(Vector3.forward * shift);
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
