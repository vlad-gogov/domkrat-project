using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrientationHorizontal
{
    Left,
    Right,
    Gay
};

public enum OrientationVertical
{
    Up,
    Down,
    Gay
};

public class Basic : MonoBehaviour
{

    public OrientationHorizontal curH;
    public OrientationVertical curV;
}
