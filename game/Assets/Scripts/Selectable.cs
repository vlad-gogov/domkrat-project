using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected = false;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Select()
    {
        isSelected = true;
        //anim.SetTrigger("Click");

    }

    public void Deselect()
    {
        isSelected = false;
        GetComponent<Renderer>().material.color = Color.gray;
    }


}
