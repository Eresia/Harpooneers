using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class HandleHarpoonWithPlayer : HandleHarpoonHit
{
    public ParticleSystem hitFX;
	public AudioClip hit_sound;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        // Play FX and sound
        if(hitFX)
        {
            hitFX.Play();
        }

		GameManager.instance.audioManager.PlaySoundOneTime (hit_sound, 0.2f);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
