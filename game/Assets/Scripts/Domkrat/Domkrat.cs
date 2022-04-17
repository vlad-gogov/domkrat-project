using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DomkratType
{
    LEFT = 0,
    RIGHT = 1
}

public class Domkrat : MonoBehaviour
{
    public DomkratType type;
    public OrientationHorizontal curH = OrientationHorizontal.None;
    public OrientationVertical curV = OrientationVertical.None;
    public Down_part_rotation downPartRotation;
    public Rotate_fixator rotateFixator;
    public TechStand techStand;
    public TormozSwitcher tormozSwitch;
    public bool isRuchka = true;
    private float SpeedMove = 1f;
    private MovingHand moveHand;
    MovingMech movingMech;
    private DomkratMoving domkratMoving;
    [SerializeField] private BoxCollider boxHand;

    Animator up_part;
    Animator move_mech;
    Up_part childRuchka;
    Down_part downPart;
    BoxCollider[] fingers;
    BoxCollider MovingHole;
    // Down_part_rotation downPartRotation;
    public bool isTormozConnected = false;

    int id = -1;

    // Переменная, показывающая подключен ли домкрат в ТПК
    public bool isAttachedToTPK = false;
    // Переменная, показывающая подключен ли домкрат к стойке
    public bool isAttachedToStoika = true;

    void Start()
    {
        boxHand.enabled = false;
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        up_part = child.GetComponent<Animator>();
        move_mech = child.transform.GetChild(0).gameObject.GetComponent<Animator>();
        childRuchka = transform.GetChild(0).gameObject.GetComponent<Up_part>();
        // Так надо, пусть ручка будет включена с самого начала, если есть вопросы по этому поводу пиши в ВК Димасику
        childRuchka.ruchka.GetComponent<BoxCollider>().enabled = true;
        downPart = transform.GetChild(1).GetComponent<Down_part>();
        moveHand = boxHand.gameObject.GetComponent<MovingHand>();
        movingMech = transform.GetChild(0).GetChild(0).GetComponent<MovingMech>();
        tormozSwitch = gameObject.transform.GetChild(1).GetChild(5).GetChild(1).GetComponent<TormozSwitcher>();
        domkratMoving = gameObject.GetComponent<DomkratMoving>();
        domkratMoving.OffCooliderWheel(false);
        fingers = transform.GetChild(0).GetChild(2).GetComponents<BoxCollider>();
        MovingHole = transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>();
        // downPartRotation = transform.GetChild(1).GetChild(5).GetComponent<Down_part_rotation>();
        // Замени false на true чтобы иметь возможность снимать домкраты со стойки с самого начала сцены
        GetDomkrats(false);
    }

    public void GetDomkrats(bool signal)
    {
        foreach(var finger in fingers)
        {
            finger.enabled = signal;
        }
        MovingHole.enabled = signal;
    }

    // Возвращает можно ли сейчас отсоеденить домкрат от стойки/ТПК
    public bool canDisconnect()
    {
        if (downPart.curPosition != Makes.DOWN)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Сначала опустите колеса, перед тем как отсоединять домкрат", Weight = ErrorWeight.MEDIUM });
            return false;
        }
        if (movingMech.isUp)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Сначала опустите ручку, перед тем как отсоединять домкрат", Weight = ErrorWeight.MEDIUM });
            return false;
        }
        if (isAttachedToStoika && downPart.curPosition == Makes.DOWN && downPart.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Дождитесь пока колеса полностью опустяться на землю", Weight = ErrorWeight.MEDIUM });
            return false;
        }
        return true;
    }

    void RotateDownPartAutomaticly(float angle, Action callback = null)
    {
        // Вместо того, чтобы отключать кучу коллайдеров, просто блочим игрока ставя игру на паузу на время анимации
        Singleton.Instance.StateManager.Pause();

        var ruchkaPos = transform.GetChild(1).GetChild(2).GetComponent<SetRucka>();
        var ruchka = transform.GetChild(0).GetChild(1).GetChild(2).gameObject;
        ruchkaPos.SetItem(ruchka, /*force=*/true, /*slowly=*/true, /*callback=*/() => {
            downPartRotation.RotateDownPart(angle, /*isGear=*/true);
            ruchka.transform.GetChild(0).GetComponent<Animator>().SetTrigger("LittleMove");
            Singleton.Timer.SetTimer(3f, () =>
            {
                var rchk = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<SetRucka>();
                rchk.SetItem(ruchka, /*force=*/true, /*slowly=*/true, /*callback=*/() => {
                    Singleton.Instance.StateManager.Resume();
                    if (callback != null)
                    {
                        callback();
                    }
                });
            });
        });
    }

    private void Update()
    {
        // Debug.Log($"normTime: {downPart.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime} | {downPart.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length}");
        // МБ пригодится для дебага
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    RotateDownPartAutomaticly(90);
        //}
    }

    public bool tryDisconnect(GameObject[] fingers)
    {
        if (!canDisconnect())
        {
            return false;
        }
        float duration = 0f;
        if (isAttachedToStoika)
        {
            duration = 4.3f;
        }
        else if (isAttachedToTPK)
        {
            duration = 3.3f;
        }

        GetDomkrats(false);
        up_part.SetTrigger("fingers_off");
        Singleton.Timer.SetTimer(1.3f, () =>
        {
            if (isAttachedToStoika)
            {
                // Блокируем rotation по X во время опускания домкрата, чтобы он не упал назад
                gameObject.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeRotationX;
                downPart.Up();
                domkratMoving.OffCooliderWheel(true);
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                isAttachedToStoika = false;
                // duration = 3f;
            }
            else if (isAttachedToTPK)
            {
                isAttachedToTPK = false;
                Singleton.Instance.StateManager.countDomkrats--;
                TPK.TPKObj.RemoveDomkrat(id);
                if (downPartRotation.dir != Direction.BACK && downPartRotation.dir != Direction.FORWARD)
                {
                    RotateDownPartAutomaticly(/*angle=*/90, /*callback=*/() => StartCoroutine(MoveSet(2, false, false)));
                    duration += 4f;
                }
                else
                {
                    StartCoroutine(MoveSet(2, false, false));
                }
                // duration = 6f;
            }
        });


        //foreach (var finger in fingers)
        //{
        //    finger.gameObject.SetActive(false);
        //}
        // После окончания анимации включаем rotation обратно
        Singleton.Timer.SetTimer(duration, () => {
            gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotationX;
            Singleton.Instance.StateManager.DomkratStoikaDisconnect();
            boxHand.enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<BoxCollider>().enabled = true;
            domkratMoving.OffCooliderWheel(true);
            // Пиздец костыль...
            downPart.curPosition = Makes.DOWN;
            childRuchka.curPosition = Makes.DOWN;
        });

        return true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject trigger = collider.gameObject;
        if (trigger.tag == "SetDomkratOnStoyka")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите E, чтобы установить домкрат на стойку");
            return;
        }
        var p = trigger.GetComponent<PointToSet>();
        if (p == null)
        {
            return;
        }
        if (p.isPerehodnick && trigger.tag == "SetPerehodnickDomkrat")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите E, чтобы установить домкрат в переходник");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Singleton.Instance.UIManager.ClearEnterText();
    }

    private void OnTriggerStay(Collider collider)
    {
        GameObject trigger = collider.gameObject;
        if (moveHand.isSelected && trigger.tag == "SetDomkratOnStoyka")
        {
            SetOnStoyka(trigger);
            return;
        }
        PointToSet p = trigger.GetComponent<PointToSet>();
        if (p == null)
        {
            return;
        }
        if (!p.isPerehodnick || p.isDomkrat)
        {
            return;
        }
        if (moveHand.isSelected && Set(collider))
        {
            p.isDomkrat = true;
            collider.enabled = false;
            isAttachedToTPK = true;
            GetDomkrats(false);
            // мб пригодится для дебага
            // GetDomkrats(true);
            movingMech.isUp = true;
            boxHand.enabled = false;
            Singleton.Instance.StateManager.countDomkrats++;
            id = TPK.TPKObj.AddDomkrat(this);
        }
    }

    public bool Set(Collider parent)
    {
        GameObject BeginPoint = parent.transform.GetChild(0).gameObject;
        GameObject EndPoint = parent.transform.GetChild(1).gameObject;

        var ptConfig = BeginPoint.GetComponent<Basic>();


        if (parent.tag == "SetPerehodnickDomkrat" && Input.GetKey(KeyCode.E))
        {
            if (ptConfig.type != type)
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "Неправильная ориентация домкрата", Weight = ErrorWeight.LOW });
                return false;
            }

            curH = ptConfig.curH;
            curV = ptConfig.curV;

            PlayerRay.playerRay.UnSelectable();
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            domkratMoving.OffCooliderWheel();

            transform.position = BeginPoint.transform.position;
            transform.rotation = BeginPoint.transform.rotation;

            float begin = BeginPoint.transform.position.z;
            float end = EndPoint.transform.position.z;
            StartCoroutine(MoveSet(end - begin));
            Destroy(BeginPoint);
            Destroy(EndPoint);

            return true;
        }
        return false;
    }

    void SetOnStoyka(GameObject parent)
    {
        GameObject BeginPoint, EndPoint;
        // Не убирайте плз этот try-catch, это костыль на котором держится весь процесс установки домкрата на стойку
        try
        {
            BeginPoint = parent.transform.GetChild(0).gameObject;
            EndPoint = parent.transform.GetChild(1).gameObject;
        } catch
        {
            Debug.Log("pizda");
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            //movingMech.isUp = true;
            //boxHand.enabled = false;
            PlayerRay.playerRay.UnSelectable();
            // domkratMoving.OffCooliderWheel();

            transform.position = BeginPoint.transform.position;
            // АХАХАХАХАХАХАХА БЛЯЯЯЯЯЯ 180 ПО Y
            transform.rotation = new Quaternion(0, 180, 0, 0);

            float begin = BeginPoint.transform.position.z;
            float end = EndPoint.transform.position.z;
            StartCoroutine(MoveSet(end - begin, /*performAnimation=*/false, /*isForward=*/true, /*isOnStoika=*/true));
            Destroy(BeginPoint);
            Destroy(EndPoint);
        }
    }

    IEnumerator MoveSet(float delta, bool performAnimation=true, bool isForward=true, bool isOnStoika=false)
    {
        float shift = SpeedMove * Time.deltaTime;
        Vector3 direction = isForward? Vector3.forward : Vector3.back;

        for (float i = 0; i <= Mathf.Abs(delta); i += shift)
        {
            gameObject.transform.Translate(direction * shift);
            domkratMoving.RotateWheelForUpdate(shift);
            yield return null;
        }
        if (performAnimation)
        {
            // var fngrs = new GameObject[] { transform.GetChild(0).GetChild(2).GetChild(0).gameObject, transform.GetChild(0).GetChild(2).GetChild(1).gameObject };
            //foreach (var finger in fngrs)
            //{
            //    finger.SetActive(true);
            //}
            up_part.SetTrigger("Finger_past");
            move_mech.SetTrigger("Up");
        }
        if (isOnStoika)
        {
            // Блокируем rotation по X во время опускания домкрата, чтобы он не упал назад
            gameObject.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeRotationX;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            Singleton.Timer.SetTimer(2f, () =>
            {
                move_mech.SetTrigger("Up");
                downPart.Down();
                Singleton.Timer.SetTimer(4f, () => {
                    //var fngrs = new GameObject[] { transform.GetChild(0).GetChild(2).GetChild(0).gameObject, transform.GetChild(0).GetChild(2).GetChild(1).gameObject };
                    //foreach (var finger in fngrs)
                    //{
                    //    finger.SetActive(true);
                    //}
                    up_part.SetTrigger("Finger_past");
                    Singleton.Timer.SetTimer(2f, () =>
                    {
                        gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        downPart.Up();
                        Singleton.Timer.SetTimer(4f, () => Singleton.Instance.StateManager.Finish());
                    });
                });
            });
        }
    }

    public void Notify(NameState state)
    {
        if (state == NameState.SET_DOMKRATS)
        {
            // boxHand.enabled = true;
            // GetDomkrats(false);
        }
        else if (state == NameState.CHECK_DOMKRATS)
        {
            // childRuchka.ruchka.GetComponent<BoxCollider>().enabled = true;
            // gameObject.SetActive(true);
        }
        else if (state == NameState.GET_DOMKRATS)
        {
            GetDomkrats(true);
        }
        else if (state == NameState.RETURN_DOMKRATS)
        {
            GetDomkrats(true);
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

    public void SwitchIntecration(bool flag)
    {
        rotateFixator.gameObject.GetComponent<BoxCollider>().enabled = flag;
        downPartRotation.SwitchBoxColliderTormozConnector();
        tormozSwitch.gameObject.GetComponent<BoxCollider>().enabled = flag;
        childRuchka.ruchka.GetComponent<BoxCollider>().enabled = flag;
    }
}
