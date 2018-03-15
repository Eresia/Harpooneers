using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRock : HandleHarpoonHit
{
	public PhysicMove Mover;

	public bool HarpoonHit
	{
		get {return _harpoonHit;}
	}
	private bool _harpoonHit = false;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
	{
		_harpoonHit = true;
	}

    protected override void ActionWhenHarpoonDetach()
	{
		_harpoonHit = false;
	}
}
