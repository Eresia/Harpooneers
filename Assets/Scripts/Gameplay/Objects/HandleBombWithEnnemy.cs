using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class HandleBombWithEnnemy : MonoBehaviour {

    public float bombDamage;

    public Action<float> OnBombExplode;

	private void OnExplode()
    {
        OnBombExplode(bombDamage);
    }
}
