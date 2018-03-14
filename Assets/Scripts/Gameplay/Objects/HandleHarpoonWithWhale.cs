using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class HandleHarpoonWithWhale : HandleHarpoonHit
{
    public float damageAmount;

    public Action<float> hitCallback;

    public ParticleSystem hitFX;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoon.Cut();

        // TODO FX and sound
        if(hitFX)
        {
            hitFX.Play();
        }

        // TODO anim ?

        hitCallback(damageAmount);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
