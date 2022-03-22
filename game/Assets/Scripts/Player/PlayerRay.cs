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

    /// <summary>
    /// TODO:
    /// Нужно реализовать систему установки предметов в нужные места
    /// 1. Нужно подумать над полями selectable
    /// </summary>

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
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Distance, 1 << PickUp.value) || Physics.Raycast(ray, out hit, Distance, 1 << Interaction.value))
        {
            hitObject = hit.collider.gameObject;
            selectable = hitObject.GetComponent<Selectable>();
            if (!_selectedObject)
                selectable.GetInfoMouse();
        }
        else if (!_selectedObject)
        {
            Singleton.Instance.UIManager.ClearEnterText();
        }

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
                    }
                }
            }
            else if (selectable.Unselect)
            {
                UnSelectable();
            }
        }

        if (Physics.Raycast(ray, out hit, Distance, 1 << PlaceForItem.value))
        {
            placeForSet = hit.collider.GetComponent<PlaceForSet>();
        }

        if (placeForSet && Input.GetMouseButtonDown(0))
        {
            if (_selectedObject)
            {
                placeForSet.SetItem(_selectedObject);
                UnSelectable();
            }
        }

        if (_selectedObject)
        {
            moving.Moving();
        }

        //selectable = null;
        placeForSet = null;

    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public GameObject GetSelected()
    {
        return _selectedObject;
    }

}
