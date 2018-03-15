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

    public HandleHarpoonWithWhale[] hittableScripts;

	public LayerMask toAvoidLayers;

    public Transform waveOrigin;
    
    public void PlaySplashFX()
    {
        splashFX.Play();
    }

    void Start()
    {
      //  StartCoroutine(ContinuousWaves());
    }

    IEnumerator ContinuousWaves()
    {
       yield return new WaitWhile(()=>(GameManager.instance.ground.waveManager == null));
        while(true)
        {
            Vector2 pos = GameManager.instance.ground.GetSeaPosition(waveOrigin.position);
            GameManager.instance.ground.waveManager.CreateTraceImpact(pos, 20f, waveOrigin.rotation.y, 2f, 0.01f, 1f, 5f, 5f);
            Debug.Log(waveOrigin.rotation.y);
            yield return new WaitForSeconds(0.25f);
        }
    }

}
