using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingersCollection : Selectable
{
    GameObject domkrat;
    Domkrat domkScript;
    // Start is called before the first frame update
    void Start()
    {
        GameObject domkrat = gameObject.transform.parent.parent.gameObject;
        domkScript = domkrat.GetComponent<Domkrat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Select()
    {
        if (domkScript.tryDisconnect(new GameObject[] {
                gameObject.transform.GetChild(0).gameObject,
                gameObject.transform.GetChild(1).gameObject 
        }))
        {
            isSelected = true;
            Debug.Log("Fingers select");
        }
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
        Singleton.Instance.UIManager.SetEnterText("Нажмите ЛКМ, чтобы взаимодействовать с шкворнями.");
    }
}
