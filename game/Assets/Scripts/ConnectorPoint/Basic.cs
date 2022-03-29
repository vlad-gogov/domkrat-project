using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrientationHorizontal
{
    Left,
    Right,
    None
};

public enum OrientationVertical
{
    Up,
    Down,
    None
};


public class Basic : MonoBehaviour
{
    public DomkratType type;
    public OrientationHorizontal curH;
    public OrientationVertical curV;
}
