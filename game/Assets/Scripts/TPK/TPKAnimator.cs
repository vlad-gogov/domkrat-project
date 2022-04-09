using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPKAnimator : MonoBehaviour
{
    public void SetUpTPK()
    {
        TPK.TPKObj.state = StateTPK.UP;
    }

    public void SetDownTPK()
    {
        TPK.TPKObj.state = StateTPK.DOWN;
    }
}
