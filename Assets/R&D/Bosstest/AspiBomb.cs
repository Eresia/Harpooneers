using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AspiBomb : MonoBehaviour {

	public Action OnExplodeAction;

	public void OnExplode()
    {
        OnExplodeAction();
    }
}
