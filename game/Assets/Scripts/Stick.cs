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
            Debug.Log("JOPA");
            selectable.enabled = false;
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, other.gameObject.transform.rotation.w);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
