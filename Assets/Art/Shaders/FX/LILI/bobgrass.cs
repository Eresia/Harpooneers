using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobgrass : MonoBehaviour {
    
    public GameObject targetobject;
    public Material instancedMaterial;

    void Start()
    {
        //
    }
    // Update is called once per frame
    void Update ()
    {
       instancedMaterial.SetVector("_humanPos", targetobject.transform.position);
    }
}
