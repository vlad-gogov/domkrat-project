using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public static PlayerRay playerRay { get; private set; }

    public float Distance;
    private GameObject _selectedObject;
    private GameObject hitObject;
    private LayerMask PickUp;
    private LayerMask Interaction;
    private LayerMask PlaceForItem;
    private Selectable selectable;
    private MovingSelect moving;
    private PlayerMove PlayerMove;
    private PlaceForSet placeForSet;
    public BoxCollider wall;

    void Start()
    {
        PickUp = LayerMask.NameToLayer("PickUp");
        Interaction = LayerMask.NameToLayer("Interaction");
        PlaceForItem = LayerMask.NameToLayer("PlaceForItem");
        playerRay = this;
        PlayerMove = GetComponent<PlayerMove>();
    }

    public void UnSelectable()
    {
        selectable.Deselect();
        selectable = null;
        _selectedObject = null;
        moving = null;
        PlayerMove.isDomkrat = false;
        wall.enabled = false;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        RaycastHit hit;
        if (!_selectedObject)
        {
            if (Physics.Raycast(ray, out hit, Distance, 1 << PickUp.value) || Physics.Raycast(ray, out hit, Distance, 1 << Interaction.value))
            {
                Debug.Log($"hit: {hit}");
                hitObject = hit.collider.gameObject;
                selectable = hitObject.GetComponent<Selectable>();
                Debug.Log($"hitObj: {hitObject}");
                Debug.Log($"selectable: {selectable}");
                Debug.Log($"_selectedObj: {_selectedObject}");
                if (!_selectedObject)
                {
                    Debug.Log("calling getInfoMouse...");
                    selectable.GetInfoMouse();
                }
            }
            else if (!_selectedObject)
            {
                Singleton.Instance.UIManager.ClearEnterText();
                selectable = null;
            }
            checkSelectable();
        }
        else if (_selectedObject.tag == "Ruchka")
        {
            if (Physics.Raycast(ray, out hit, Distance, 1 << PlaceForItem.value))
            {
                placeForSet = hit.collider.GetComponent<PlaceForSet>();
                if (_selectedObject)
                {
                    placeForSet.GetInfoMouse();
                    if (Input.GetMouseButtonDown(0))
                    {
                        placeForSet.SetItem(_selectedObject);
                        UnSelectable();
                    }
                }
            }
            else
            {
                Singleton.Instance.UIManager.ClearEnterText();
                placeForSet = null;
            }
        }

        if (moving)
        {
            moving.Moving();
        }

        //selectable = null;
        placeForSet = null;

    }

    void checkSelectable()
    {
        if (selectable && Input.GetMouseButtonDown(0))
        {
            if (!selectable.isSelected)
            {
                selectable.Select();
                if (hitObject.layer == PickUp)
                {
                    _selectedObject = selectable.GetSelectObject();
                    moving = _selectedObject.GetComponent<MovingSelect>();
                    Singleton.Instance.UIManager.ClearEnterText();
                    if (_selectedObject.tag == "Domkrat")
                    {
                        PlayerMove.isDomkrat = true; 
                        PlayerMove.PickUpDomkrat(_selectedObject);
                    }
                    if (_selectedObject.tag == "Domkrat" || _selectedObject.tag == "Perehodnick")
                    {
                        wall.enabled = true;
                    }
                }
            }
            else if (selectable.Unselect && Input.GetMouseButtonDown(0))
            {
                UnSelectable();
            }
        }
    }

    public GameObject GetSelected()
    {
        return _selectedObject;
    }

}
