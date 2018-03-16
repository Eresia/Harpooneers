using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingLD : MonoBehaviour {

    private float startX;
    public float minX;

	// Use this for initialization
	void Start ()
    {
        startX = transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {

        if(transform.position.x < minX)
        {
            ResetPos();
        }
		
	}

    public void ResetPos()
    {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
    }
}
