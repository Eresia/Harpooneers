using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class HandleHarpoonWithWhale : HandleHarpoonHit
{
    public bool eye;

    public Action<bool> hitCallback;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoon.Cut();

        // TODO FX and sound

        // TODO anim

        hitCallback(eye);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
