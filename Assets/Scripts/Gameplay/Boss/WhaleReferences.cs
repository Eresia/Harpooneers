using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleReferences : MonoBehaviour {

    public ParticleSystem[] eyeBloodFX;
    public ParticleSystem splashFX;

    public Collider bodyCollider;
    public Collider tailCollider;
    public Transform whaleChildTransform;

    public Animator whaleAnimator;

    public HandleHarpoonWithEye[] eyeScript;

    public void  PlayEyeBloodFX(int i)
    {
        eyeBloodFX[i].Play();
    }
    public void PlaySplashFX()
    {
        splashFX.Play();
    }

}
