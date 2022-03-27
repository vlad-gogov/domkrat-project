using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPKMoving : MonoBehaviour
{
    Animator anim;
    bool is_front_was = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (Input.GetKeyDown(KeyCode.R)) {

            if (!is_front_was)
            {
                anim.SetTrigger("Moving_front");
                is_front_was = true;
            }
            else {
                anim.SetTrigger("Valim_bokom");
            }
        }
    }
}
