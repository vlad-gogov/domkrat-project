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

    public override void SetItem(GameObject other)
    {
        if (other.tag == "Ruchka")
        {
            if (anim)
            {
                anim.SetTrigger("Push");
            }
            StartCoroutine(WaitForSet(TimeWait, other));
        }
    }

    IEnumerator WaitForSet(float time, GameObject other)
    {
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            if (t >= time / 2 && !isSet)
            {
                down_ruchka.GetComponent<BoxCollider>().enabled = true;
                other.transform.position = Pointer.transform.position;
                other.transform.localRotation = Pointer.transform.localRotation;
                other.GetComponentInChildren<Ruchka>().curPosition = pos;
                isSet = true;
            }
            yield return null;
        }
        isSet = false;
        if (anim)
            anim.SetTrigger("Idle");
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы установить ручку");
    }
}
