using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmentation : MonoBehaviour
{
    public GameObject Output;

    // Attach to all objects which can be disabled or destroyed.
    private HandleHarpoonWithDestroyable harpoonMgr;

    private void Awake()
    {
        harpoonMgr = GetComponent<HandleHarpoonWithDestroyable>();
    }

    public void OnExplode ()
	{
		Debug.Log("BOOM");

        harpoonMgr.DetachHarpoon();

        Output.SetActive(true);
		gameObject.SetActive(false);
	}
}
