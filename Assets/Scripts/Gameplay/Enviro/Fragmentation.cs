using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmentation : MonoBehaviour
{
    public GameObject baseMesh;
    public GameObject fractMesh;
    public Collider[] harpoonColliders;

    private MeshCollider _mycollider;
    private PhysicMove _physicScript;
    private ConstantMovement _movementScript;
    

    void Awake()
    {
        _mycollider = GetComponent<MeshCollider>();
        _physicScript = GetComponent<PhysicMove>();
        _movementScript = GetComponent<ConstantMovement>();
    }

    void Start()
    {
        baseMesh.SetActive(true);
        fractMesh.SetActive(false);
    }

    public void OnExplode ()
	{
        foreach(Collider col in harpoonColliders)
        {
            col.enabled = false;
        }

        _movementScript.enabled = false;
        _physicScript.enabled = false;
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
