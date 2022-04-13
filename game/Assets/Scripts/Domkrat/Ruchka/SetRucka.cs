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
        if (other.tag == "Ruchka")
        {
            if (!domkrat.isRuchka)
            {
                domkrat.isRuchka = true;
                if (anim)
                {
                    anim.SetTrigger("Push");
                }
                StartCoroutine(WaitForSet(TimeWait, other));
                return true;
            }
            else
            {
                Singleton.Instance.StateManager.onError(new Error() { ErrorText = "На этом домкрате уже есть ручка", Weight = ErrorWeight.MINOR });
            }
        }
        return false;
    }

    IEnumerator WaitForSet(float time, GameObject other)
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
    }

    public override void GetInfoMouse(GameObject other)
    {
        if (other.tag == "Ruchka")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы установить ручку");
        }
    }
}
