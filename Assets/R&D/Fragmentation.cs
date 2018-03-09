using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmentation : MonoBehaviour
{	public GameObject Output;

	public void OnExplode ()
	{
		Debug.Log("BOOM");
		Output.SetActive(true);
		gameObject.SetActive(false);
	}
}
