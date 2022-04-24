using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRucka : PlaceForSet
{
    [SerializeField] GameObject Pointer;
    [SerializeField] GameObject down_ruchka;
    public PositionRuchka pos;
    public float TimeWait;
    private bool isSet = false;
    [SerializeField] Animator anim;
    Domkrat domkrat;

    void Start()
    {
        if (pos == PositionRuchka.DOWN)
        {
            domkrat = gameObject.transform.parent.parent.GetComponent<Domkrat>();
        }
        else
        {
            domkrat = gameObject.transform.parent.parent.parent.parent.GetComponent<Domkrat>();
        }
    }

    public override bool SetItem(GameObject other)
    {
        return SetItem(other, /*force=*/false);
    }

    public bool SetItem(GameObject other, bool force=false, bool slowly=false, Action callback = null)
    {
        if (other.tag == "Ruchka")
        {
            if (force || !domkrat.isRuchka)
            {
                domkrat.isRuchka = true;
                if (anim)
                {
                    anim.SetTrigger("Push");
                }
                if (slowly)
                {
                    StartCoroutine(SlowlySet(other, callback));
                }
                else
                {
                    StartCoroutine(WaitForSet(TimeWait, other, callback));
                }
                return true;
            }
            else
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "На этом домкрате уже есть ручка", Weight = ErrorWeight.MINOR });
            }
        }
        return false;
    }

    IEnumerator SlowlySet(GameObject other, Action callback = null)
    {
        var deltaPos = Pointer.transform.position - other.transform.position;
        var deltaAngl = Pointer.transform.localRotation.eulerAngles - other.transform.localEulerAngles;
        other.transform.localEulerAngles = Pointer.transform.localRotation.eulerAngles;

        int steps = 60 * 2;
        float step = 1.0f / steps;
        for (int i=0; i<steps; i++)
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, Pointer.transform.position, step);
            // other.transform.localEulerAngles = Vector3.MoveTowards(other.transform.localEulerAngles, Pointer.transform.localRotation.eulerAngles, step);
            // other.transform.rotation = new Quaternion(other.transform.rotation.x, other.transform.rotation.y + (180 * step), other.transform.rotation.z, other.transform.rotation.w);
            yield return null;
        }

        if (callback != null)
        {
            callback();
        }
    }

    IEnumerator WaitForSet(float time, GameObject other, Action callback = null)
    {
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            if (t >= time / 2 && !isSet)
            {
                down_ruchka.GetComponent<BoxCollider>().enabled = true;
                other.transform.position = Pointer.transform.position;
                other.transform.localEulerAngles = Pointer.transform.localRotation.eulerAngles;
                other.GetComponentInChildren<Ruchka>().curPosition = pos;
                isSet = true;
            }
            yield return null;
        }
        isSet = false;
        if (anim)
            anim.SetTrigger("Idle");

        if (callback != null)
        {
            callback();
        }
    }

    public override void GetInfoMouse(GameObject other)
    {
        if (other.tag == "Ruchka")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы установить ручку");
        }
    }
}
