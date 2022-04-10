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
    MovingSelect mv_obj;
    private List<MovingSelect> moving = new List<MovingSelect>();
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
        GetComponent<Rigidbody>().mass = 0;
        moving.Remove(mv_obj);
        PlayerMove.isDomkrat = false;

        wall.enabled = false;
    }

    public void Add(MovingSelect movable)
    {
        moving.Add(movable);
    }

    public void Remove(MovingSelect movable)
    {
        moving.Remove(movable);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (!_selectedObject)
        {
            if (Physics.Raycast(ray, out hit, Distance, 1 << PickUp.value) || Physics.Raycast(ray, out hit, Distance, 1 << Interaction.value))
            {
                hitObject = hit.collider.gameObject;
                selectable = hitObject.GetComponent<Selectable>();
                if (!_selectedObject)
                {
                    selectable.GetInfoMouse();
                }
            }
            else if (!_selectedObject)
            {
                Singleton.Instance.UIManager.ClearEnterText();
                selectable = null;
            }
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
                        if (placeForSet.SetItem(_selectedObject))
                        {
                            UnSelectable();
                        }
                    }
                }
            }
            else
            {
                Singleton.Instance.UIManager.ClearEnterText();
                placeForSet = null;
            }
        }

        checkSelectable();

        foreach (var movable in moving)
        {
            movable.Moving();
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
                    mv_obj = _selectedObject.GetComponent<MovingSelect>();
                    moving.Add(mv_obj);
                    Singleton.Instance.UIManager.ClearEnterText();
                    if (_selectedObject.tag == "Domkrat")
                    {
                        PlayerMove.isDomkrat = true;
                        GetComponent<Rigidbody>().mass = 10f;
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
