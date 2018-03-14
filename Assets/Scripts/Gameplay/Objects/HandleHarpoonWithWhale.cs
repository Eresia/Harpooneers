using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class HandleHarpoonWithWhale : HandleHarpoonHit
{
    public bool eye;

	public AudioClip hit_sound;
	public AudioClip scream_sound;

    public Action<bool> hitCallback;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoon.Cut();

        // TODO FX and sound
		GameManager.instance.audioManager.PlaySoundOneTime (hit_sound, 0.2f);
		GameManager.instance.audioManager.PlaySoundOneTime (scream_sound, 0.3f);

        // TODO anim

        hitCallback(eye);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
