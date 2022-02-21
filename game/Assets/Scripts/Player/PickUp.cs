using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public GameObject myHands;
    bool canpickup;
    GameObject ObjectIwantToPickUp;
    bool hasItem;

    void Start()
    {
        canpickup = false;    //setting both to false
        hasItem = false;
    }

    public void pickUp(Collider other) // to see when the player enters the collider
    {
        if (other.gameObject.tag == "object")
        {
            canpickup = true;
            ObjectIwantToPickUp = other.gameObject;
        }
        if (canpickup && !hasItem)
        {
            if (Input.GetKeyDown(KeyCode.E))  // can be e or any key
            {
                ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                ObjectIwantToPickUp.transform.position = myHands.transform.position; // sets the position of the object to your hand position
                ObjectIwantToPickUp.transform.parent = myHands.transform; //makes the object become a child of the parent so that it moves with the hands
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && hasItem == true) // if you have an item and get the key to remove the object, again can be any key
        {
            ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again
            hasItem = !hasItem;
            canpickup = true;
            ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
        }

    }
}
