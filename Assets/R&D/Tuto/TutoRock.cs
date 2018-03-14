using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRock : HandleHarpoonHit
{
	public bool HarpoonHit
	{
		get {return _harpoonHit;}
	}
	private bool _harpoonHit = false;

	public bool RockExplode
	{
		get {return _rockExplode;}
	}
	private bool _rockExplode = false;

    protected override void ActionWhenHarpoonAttach(Harpoon harpoon)
	{
		_harpoonHit = true;
	}

    protected override void ActionWhenHarpoonDetach()
	{
		_harpoonHit = false;
	}

	public void OnExplode()
	{
			_rockExplode = true;
	}
}
