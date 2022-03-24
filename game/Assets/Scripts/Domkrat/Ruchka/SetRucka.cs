using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRucka : PlaceForSet
{
    [SerializeField] GameObject Pointer;
    [SerializeField] GameObject down_ruchka;
    public override void SetItem(GameObject gameObject)
    {
        if (gameObject.tag == "Ruchka")
        {
            down_ruchka.GetComponent<Collider>().enabled = true;
            gameObject.transform.position = Pointer.transform.position;
            gameObject.transform.rotation = Pointer.transform.rotation;
        }
    }
}
