using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public bool isSelected = false;
    public bool Unselect = true;

    public abstract void Select();

    public abstract void Deselect();

    public abstract GameObject GetSelectObject();

    public abstract void GetInfoMouse();
}
