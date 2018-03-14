using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour {

    public Vector3 direction;
    public float speed;

    public bool forwardDirection;


	// Update is called once per frame
	void Update () {

        if(!forwardDirection)
          transform.position += direction.normalized * speed * Time.deltaTime;	
        else
          transform.position += transform.forward * speed * Time.deltaTime;
    }
}
