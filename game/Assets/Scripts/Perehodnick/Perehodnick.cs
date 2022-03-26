using UnityEngine;
using UnityEngine.UI;

public class Perehodnick : Selectable
{
    [SerializeField] GameObject parent_object;

    public override void Select()
    {
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

    public void Interaction(Collider trigger)
    {
        if (trigger.gameObject.tag == "SetPerehodnick")
        {
            Singleton.Instance.UIManager.SetEnterText("Нажмите E чтобы установить переходник в ТПК");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Interaction(collider);
    }

    public bool Set(Collider trigger)
    {
        GameObject parent = trigger.gameObject;
        if (parent.tag == "SetPerehodnickDomkrat" && Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            //parent.transform.SetParent(gameObject.transform);
            
            GameObject point = parent.transform.GetChild(0).gameObject;
            transform.position = point.transform.position;
            transform.rotation = point.transform.rotation;
            
            Destroy(point);
            Singleton.Instance.UIManager.ClearEnterText();
            Singleton.Instance.StateManager.countPerehodnick++;

            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (Set(collider))
        {
            //collider.enabled = false;
            PlayerRay.playerRay.UnSelectable();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Singleton.Instance.UIManager.ClearEnterText();
    }
}
