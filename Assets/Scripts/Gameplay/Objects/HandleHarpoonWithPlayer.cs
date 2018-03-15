using UnityEngine;

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

		Debug.Log ("HIT");

		GameManager.instance.audioManager.PlaySoundOneTime (hit_sound, 1f);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
