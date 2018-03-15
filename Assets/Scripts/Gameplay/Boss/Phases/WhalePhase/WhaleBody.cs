using UnityEngine;

using System;

/// <summary>
/// Handle bomb damage with the whale body.
/// </summary>
public class WhaleBody : MonoBehaviour {

    public float damageWithBomb = 10f;

    public Action<float> OnWhaleExplode;

	public void OnExplode()
    {
        OnWhaleExplode(damageWithBomb);
    }
}
