using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domkrat : MonoBehaviour
{

    public OrientationHorizontal myOrientation;
    private float SpeedRotation = 80f;
    private float SpeedMove = 0.007f;
    private Vector3 prev;
    [SerializeField] private GameObject LeftWheel;
    [SerializeField] private GameObject RightWheel;
    [SerializeField] private GameObject BackWheel;

    Animator up_part;
    Animator move_mech;
    Up_part childRuchka;

    int id = -1;

    // Переменная, показывающая подключен ли домкрат в ТПК
    public bool isAttachedToTPK = false;

    void Start()
    {
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        up_part = child.GetComponent<Animator>();
        move_mech = child.transform.GetChild(0).gameObject.GetComponent<Animator>();

        childRuchka = transform.GetChild(0).gameObject.GetComponent<Up_part>();
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

        GameObject BeginPoint = parent.transform.GetChild(0).gameObject;
        GameObject EndPoint = parent.transform.GetChild(1).gameObject;

        var ptConfig = BeginPoint.GetComponent<Basic>();


        if (parent.tag == "SetPerehodnickDomkrat" && Input.GetKey(KeyCode.E))
        {
            if (ptConfig.curH != myOrientation)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Неправильная ориентация домкрата", Weight = ErrorWeight.LOW });
                return false;
            }

            transform.position = BeginPoint.transform.position;
            transform.rotation = new Quaternion(transform.rotation.x, BeginPoint.transform.rotation.y, BeginPoint.transform.rotation.z, BeginPoint.transform.rotation.w);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            float begin = BeginPoint.transform.position.z;
            float end = EndPoint.transform.position.z;
            StartCoroutine(MoveSet(end - begin));
            up_part.SetTrigger("Finger_past");
            move_mech.SetTrigger("Up");

            return true;
        }
        return false;
    }

    IEnumerator MoveSet(float delta)
    {
        float shift = SpeedMove;
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
            isAttachedToTPK = true;
            Singleton.Instance.StateManager.countDomkrats++;
            id = TPK.TPKObj.AddDomkrat(this);
            PlayerRay.playerRay.UnSelectable();
        }
    }

    public void Notify(State state)
    {
        if (state == State.CHECK_DOMKRATS)
        {
            gameObject.SetActive(true);
        }
    }

    public void LiftUp(bool liftTPK=true)
    {
        childRuchka.RealUp(liftTPK);
    }

    public void LiftDown(bool liftTPK = true)
    {
        childRuchka.RealDown(liftTPK);
    }

    void Update()
    {
        //RotateWheel();
    }
}
