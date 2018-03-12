using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHarpoonable {

    void OnHarpoonAttach(Harpoon harpoon);

    void OnHarpoonDetach();
}
