using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleReferences : MonoBehaviour {
    
    public ParticleSystem splashFX;

	public WhaleBody whaleBody;
    public Collider bodyCollider;
    public Collider tailCollider;
    public Transform whaleChildTransform;

    public Animator whaleAnimator;

    public HandleHarpoonWithEnnemy[] hittableScripts;

	public LayerMask toAvoidLayers;
    
    public void PlaySplashFX()
    {
        splashFX.Play();
    }

}
