using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPKMoving : MonoBehaviour
{
    [SerializeField] GameObject dom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (Input.GetKey(KeyCode.R)) {
            Debug.Log("RRRRRR");
            Vector3 inputVector = transform.TransformVector(new Vector3(0.1f, 0f, 0f));
            foreach (Domkrat domkrat in TPK.TPKObj.attachedDomkrats)
            {
                domkrat.transform.position = domkrat.transform.position + inputVector; 
                //= inputVector  + domkrat.transform.position;
            }
            gameObject.transform.position = inputVector + gameObject.transform.position; 
            //= inputVector * TPK.TPKObj.attachedDomkrats[0].transform.localScale.x + gameObject.transform.position;
        }
    }
}
