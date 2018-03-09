using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(PhysicMove))]
public class HarpoonLauncher : MonoBehaviour {
    
	[SerializeField]
	private Harpoon harpoonPrefab;

	[Space]

	public Transform boatFollower;

	[SerializeField]
	private Transform directionObject;

	[Space]

	[Tooltip("Joystix is considered at 0 behin d this value")]
	[SerializeField]
	private float joystickError;

	[Space]

	[SerializeField]
	private float castTime;

	[Tooltip("Time beetween max distance and launch harpoon")]
	[SerializeField]
	private float castTimeMax;

	[SerializeField]
	private float castDistance;
	
	[SerializeField]
	private float cooldownTime;
    
    [Header("Module")]
    public HarpoonModule harpoonModule;

    public Transform selfTransform {get; private set;}

	public PhysicMove physicMove {get; private set;}

	private Harpoon harpoon;

	private bool isLaunching;

	private float timeBeforeLaunch;

	private float power;

	private Vector3 lastDirection;

	private Mouse mouse;

	private void Awake()
	{
		selfTransform = GetComponent<Transform>();
		physicMove = GetComponent<PhysicMove>();
		mouse = ReInput.controllers.Mouse;
	}

	public void LaunchHarpoon(Vector3 direction){

		if(harpoon == null){
            
			if(direction.sqrMagnitude > joystickError)
            {
				if(!isLaunching)
                {
					BeginLaunching();
				}

				if(power == 1f)
                {
					if(timeBeforeLaunch >= castTimeMax)
                    {
						EndLaunching(direction, power);
						return ;
					}

					timeBeforeLaunch += Time.deltaTime;
				}

				else
                {
					power = Mathf.Min(1f, power + (Time.deltaTime / castTime));
				}

				DisplayLaunching(direction, power);
				lastDirection = direction;
			}

			else if(isLaunching)
            {
				EndLaunching(lastDirection, power);
			}
		}
	}

	public void Cut()
    {
		if(harpoon != null)
        {
			harpoon.Cut();
		}
	}

    public void Release()
    {
        if(harpoon != null)
        {
            harpoon.Release();
        }
    }

    public void Pull()
    {
        if (harpoon != null)
        {
            harpoon.Pull();
        }
    }

    public void EndReturn(){
		harpoon = null;
	}

	private void BeginLaunching(){
		isLaunching = true;
		power = 0f;
		timeBeforeLaunch = 0f;
		directionObject.gameObject.SetActive(true);
	}

	private void DisplayLaunching(Vector3 direction, float power){
		directionObject.localPosition = direction * power * castDistance;
	}

	private void EndLaunching(Vector3 direction, float power){

		isLaunching = false;
		directionObject.gameObject.SetActive(false);
		harpoon = Instantiate<Harpoon>(harpoonPrefab, selfTransform.position, Quaternion.identity);
        harpoon.TractionSpeed = harpoonModule.tractionSpeed;
        
        // The distance depends from the duration of cast
        float distanceToReach = Mathf.Lerp(0f, harpoonModule.fireDistance, power);

        // OSEF
        //float distanceToReach = harpoonModule.fireDistance;

        harpoon.Launch(this, selfTransform.position, direction * harpoonModule.fireSpeed + physicMove.Velocity, distanceToReach, harpoonModule.returnSpeed);
	}
}
