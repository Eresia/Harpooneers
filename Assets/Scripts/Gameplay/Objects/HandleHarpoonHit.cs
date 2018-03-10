using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandleHarpoonHit : MonoBehaviour, IHarpoonable {

    public void OnHarpoonAttach(Harpoon harpoon)
    {
        ActionWhenHarpoonAttach(harpoon);
    }

    public void OnHarpoonDetach()
    {
        ActionWhenHarpoonDetach();
    }

    protected abstract void ActionWhenHarpoonAttach(Harpoon harpoon);

    protected abstract void ActionWhenHarpoonDetach();
}
