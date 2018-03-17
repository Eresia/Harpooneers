/// <summary>
/// Use this to don't destroy harpoon when destroy this gameobject.
/// </summary>
	
public class HandleHarpoonWithDestroyable : HandleHarpoonHit
{
	public UnityEngine.AudioClip hit_sound;

    private Harpoon harpoonAttached;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoonAttached = harpoon;
		GameManager.instance.audioManager.PlaySoundOneTime (hit_sound, 0.1f);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        harpoonAttached = null;
    }

    public void DetachHarpoon()
    {
        if(harpoonAttached != null)
        {
            harpoonAttached.Cut();
        }
    }
}
