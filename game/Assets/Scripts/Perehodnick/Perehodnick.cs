using UnityEngine;
using UnityEngine.UI;

public class Perehodnick : Selectable
{
    [HideInInspector] public bool isConnect = true;
    public override void Select()
    {
        if (isConnect)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isConnect = false;
        }
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
        Singleton.Instance.UIManager.ClearHelperText();
    }

    public override GameObject GetSelectObject()
    {
        return gameObject;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы поднять переходник");
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject trigger = collider.gameObject;
        if (!trigger.GetComponent<PointToSet>().isPerehodnick && trigger.tag == "SetPerehodnickDomkrat")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите E, чтобы установить переходник в ТПК");
        }
    }

    public bool Set(GameObject trigger)
    {
        GameObject parent = trigger.gameObject;
        if (parent.tag == "SetPerehodnickDomkrat" && Input.GetKey(KeyCode.E))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            isConnect = true;

            GameObject point = parent.transform.GetChild(0).gameObject;
            transform.position = point.transform.position;
            transform.rotation = point.transform.rotation;
            
            Destroy(point);
            Singleton.Instance.UIManager.ClearEnterText();

            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider collider)
    {
        GameObject trigger = collider.gameObject;
        PointToSet p = trigger.GetComponent<PointToSet>();
        if (p.isPerehodnick)
        {
            return;
        }
        if (Set(trigger))
        {
            p.isPerehodnick = true;
            Singleton.Instance.StateManager.countPerehodnick++;
            //collider.enabled = false;
            PlayerRay.playerRay.UnSelectable();
        }

    }

    private void OnTriggerExit(Collider collider)
    {
        Singleton.Instance.UIManager.ClearEnterText();
    }
}
