using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Makes
{
    UP = 0,
    DOWN = 1
}

public class Up_part : MonoBehaviour
{

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Up()
    {
        animator.SetTrigger("Up");
    }

    public void Down()
    {
       
        animator.SetTrigger("Down");
    }
}
