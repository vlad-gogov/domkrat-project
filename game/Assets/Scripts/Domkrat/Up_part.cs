using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Makes
{
    UP = 0,
    DOWN = 1
}

public class Up_part : MonoBehaviour
{
    public UnityEvent my_event;
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

    void HideFinger()
    {
        my_event.Invoke();
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
