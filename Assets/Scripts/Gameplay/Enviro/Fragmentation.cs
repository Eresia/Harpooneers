using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmentation : MonoBehaviour
{
    public GameObject baseMesh;
    public GameObject fractMesh;

    private MeshCollider _mycollider;

    void Awake()
    {
        _mycollider = GetComponent<MeshCollider>();
    }

    void Start()
    {
        baseMesh.SetActive(true);
        fractMesh.SetActive(false);
    }

    public void OnExplode ()
	{
        _mycollider.enabled = false; 
        fractMesh.SetActive(true);
        baseMesh.SetActive(false);

        Destroy(gameObject, 3f);
	}

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Whale" || col.gameObject.tag == "Rocks")
        {
            OnExplode();
        }
    }
}
