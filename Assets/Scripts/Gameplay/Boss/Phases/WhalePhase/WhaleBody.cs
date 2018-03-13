using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class WhaleBody : MonoBehaviour {

    public Action OnWhaleExplode;

	public void OnExplode()
    {
        OnWhaleExplode();
    }
}
