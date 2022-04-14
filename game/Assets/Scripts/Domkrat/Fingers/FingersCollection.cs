using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingersCollection : Selectable
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Select()
    {
        isSelected = true;

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);

        GameObject up_part = gameObject.transform.parent.gameObject;
        up_part.GetComponent<Animator>().SetTrigger("fingers_off");
        //Hide();

        GameObject domkrat = gameObject.transform.parent.parent.gameObject;
        domkrat.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log("Fingers select");
    }

    public override void Deselect()
    {
        isSelected = false;
    }

    public override GameObject GetSelectObject()
    {
        return gameObject;
    }

    public override void GetInfoMouse()
    {
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы взаимодействовать с шкворнями."); ;
    }
}
