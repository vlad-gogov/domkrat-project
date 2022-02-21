using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{

    public float Distance;
    [SerializeField] private GameObject Pointer;
    private GameObject _selectedObject;
    [SerializeField] private GameObject Wall;

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Distance))
        {
            GameObject hitObject = hit.collider.gameObject;
            Selectable selectable = hitObject.GetComponent<Selectable>();
            Debug.Log(selectable);
            if (selectable && Input.GetMouseButtonDown(0))
            {
                if (!_selectedObject)
                {
                    _selectedObject = hitObject.transform.parent.gameObject;
                    Wall.GetComponent<BoxCollider>().enabled = true;
                    selectable.Select();
                }
                else
                {
                    Wall.GetComponent<BoxCollider>().enabled = false;
                    selectable.Deselect();
                    _selectedObject = null;
                }
            }
        }
        if (_selectedObject)
        {
            Vector3 position = new Vector3(Pointer.transform.position.x, _selectedObject.transform.position.y, Pointer.transform.position.z);
            _selectedObject.transform.position = position;
            _selectedObject.transform.rotation = Pointer.transform.rotation;

            /*
            isCollision = _selectedObject.GetComponent<Domkrat>().isCollision;
            if (!isCollision)
            {
                Vector3 position = new Vector3(Pointer.transform.position.x, _selectedObject.transform.position.y, Pointer.transform.position.z);
                _selectedObject.transform.position = position;
                _selectedObject.transform.rotation = Pointer.transform.rotation;
            }
            else
            {
                isCollision = false;
                transform.position = new Vector3(transform.rotation.x, transform.position.y, transform.position.z - offset);
            }
            */
        }

    }

}
