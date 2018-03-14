using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleReferences : MonoBehaviour {
    
    public ParticleSystem splashFX;

    public Collider bodyCollider;
    public Collider tailCollider;
    public Transform whaleChildTransform;

    public Animator whaleAnimator;

    public HandleHarpoonWithWhale[] hittableScripts;
    
    public void PlaySplashFX()
    {
        splashFX.Play();
    }

}
