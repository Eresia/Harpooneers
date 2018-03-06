using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour {

	[SerializeField]
	private HarpoonLauncher parent;

	[SerializeField]
	private float maxDistance;

	private float actualDistance;
}
