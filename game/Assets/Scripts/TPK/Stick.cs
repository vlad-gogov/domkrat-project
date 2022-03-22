using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] GameObject hand;

    void OnTriggerStay(Collider other)
    {
        Selectable selectable = hand.GetComponent<Selectable>();
        if (other.gameObject.name == "Domkrat" && !selectable.isSelected)
        {
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, other.gameObject.transform.rotation.w);
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Selectable selectable = hand.GetComponent<Selectable>();
        if (other.gameObject.name == "Domkrat")
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
