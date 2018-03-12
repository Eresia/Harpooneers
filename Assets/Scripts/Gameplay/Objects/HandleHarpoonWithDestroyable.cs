/// <summary>
/// Use this to don't destroy harpoon when destroy this gameobject.
/// </summary>
public class HandleHarpoonWithDestroyable : HandleHarpoonHit
{
    private Harpoon harpoonAttached;
    
    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoonAttached = harpoon;
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
