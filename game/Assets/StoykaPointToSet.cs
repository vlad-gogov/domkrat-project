using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoykaPointToSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Singleton.Instance.StateManager.GetState() == NameState.RETURN_DOMKRATS)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
