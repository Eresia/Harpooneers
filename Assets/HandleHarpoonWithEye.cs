using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class HandleHarpoonWithEye : HandleHarpoonHit
{
    public bool leftEye;

    public Action<bool> hitCallback;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
    {
        harpoon.Cut();

        // TODO FX and sound

        // TODO anim

        hitCallback(leftEye);
    }

    protected override void ActionWhenHarpoonDetach()
    {
        // Do nothing.
    }
}
