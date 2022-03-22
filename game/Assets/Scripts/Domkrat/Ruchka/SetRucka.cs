using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRucka : PlaceForSet
{
    [SerializeField] GameObject Pointer;
    public override void SetItem(GameObject gameObject)
    {
        if (gameObject.tag == "Ruchka")
        {
            gameObject.transform.position = Pointer.transform.position;
            gameObject.transform.rotation = Pointer.transform.rotation;
        }
    }
}
