using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMBack : MonoBehaviour
{
    private DMHandler _dmHandler; 
    // Start is called before the first frame update
    void Start()
    {
        _dmHandler = FindObjectOfType<DMHandler>();
        _dmHandler.backFromDM += DestroyThisObject;
    }

    void DestroyThisObject()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
