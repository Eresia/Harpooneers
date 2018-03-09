using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandleHarpoonHit : MonoBehaviour, IHarpoonable {

    public void OnHarpoonCollide(Harpoon harpoon)
    {
        ActionWhenHarpoon(harpoon);
    }

    protected abstract void ActionWhenHarpoon(Harpoon harpoon);
}
