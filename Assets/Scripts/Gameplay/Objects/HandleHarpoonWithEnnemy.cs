using UnityEngine;

using System;

public class HandleHarpoonWithEnnemy : HandleHarpoonHit
{
    public float damageAmount;

    public Action<float> hitCallback;
    public ParticleSystem hitFX;
	public AudioClip hit_sound;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoon.Cut();

        // Play FX and sound
        if(hitFX)
        {
            hitFX.Play();
        }

        if(hit_sound)
        {
            GameManager.instance.audioManager.PlaySoundOneTime(hit_sound, 0.2f);
        }

        if(hitCallback != null)
        {
            hitCallback(damageAmount);
        }
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
