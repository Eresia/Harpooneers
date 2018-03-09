using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleHarpoonWithBarrel : HandleHarpoonHit
{
    public ExplosiveBarrel barrel;

    private void Reset()
    {
        barrel = transform.parent.GetComponent<ExplosiveBarrel>();
    }

    protected override void ActionWhenHarpoon(Harpoon harpoon)
    {
        harpoon.Cut();

        barrel.TriggerExplosion(0f);
    }
}
