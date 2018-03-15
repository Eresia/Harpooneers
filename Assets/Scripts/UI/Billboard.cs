using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    private Camera gameCamera;

	// Update is called once per frame

    void Awake()
    {
        gameCamera = GameManager.instance.camMgr.cam;
    }
	void Update () {

        transform.LookAt(gameCamera.transform.position);
	}
}
