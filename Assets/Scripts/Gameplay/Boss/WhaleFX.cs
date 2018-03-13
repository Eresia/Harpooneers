using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleFX : MonoBehaviour {

    public ParticleSystem[] eyeBloodFX;
    public ParticleSystem splashFX;

    public void  PlayEyeBloodFX(int i)
    {
        eyeBloodFX[i].Play();
    }
    public void PlaySplashFX()
    {
        splashFX.Play();
    }

}
