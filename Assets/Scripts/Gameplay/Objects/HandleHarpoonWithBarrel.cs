/// <summary>
/// Detach the harpoon before that the barrel explodes.
/// </summary>
public class HandleHarpoonWithBarrel : HandleHarpoonHit
{
    public ExplosiveBarrel barrel;

    private void Reset()
    {
        barrel = transform.parent.GetComponent<ExplosiveBarrel>();
    }

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoon.Cut();

        barrel.TriggerExplosion(0f);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing because harpoon is already detach when the barrel explodes.
    }
}
