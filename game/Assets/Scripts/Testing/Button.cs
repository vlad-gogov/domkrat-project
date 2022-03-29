using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject VPR;
    public Color gr;
    public Color fl;
    public Image th;
    public Text t;
    public Tasks task;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Check()
    {
        if(t.text == task.True)
        {
            th.color = gr;
            VPR.active = false;
        }
        else
        {
            VPR.active = false;
            th.color = fl;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
