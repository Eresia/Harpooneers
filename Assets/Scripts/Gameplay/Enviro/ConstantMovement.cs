using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour {

    public Vector3 direction;
    public float speed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position += direction.normalized * speed * Time.deltaTime;
		
	}
}
