﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(PhysicMove))]
public class HarpoonLauncher : MonoBehaviour {
    
    [SerializeField]
	private int playerId;

	[Space]

	[SerializeField]
	private Harpoon harpoonPrefab;

	[Space]

	public Transform boatFollower;

	[SerializeField]
	private Transform directionObject;

	[Space]

	[Tooltip("Joystix is considered at 0 behin d this value")]
	[SerializeField]
	private float joystixError;

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

	private void Update() {
		if(GameManager.instance.actualPlayer == playerId){
			if(mouse.GetButton(0)){
				Vector3 boatPosition = Camera.main.WorldToScreenPoint(selfTransform.position);
				Vector3 direction = Input.mousePosition - boatPosition;
				direction.z = direction.y;
				direction.y = 0;
				LaunchHarpoon(direction.normalized);
			}
			else{
				LaunchHarpoon(Vector2.zero);
			}

			if(mouse.GetButtonDown(1)){
				Cut();
			}

			if(harpoon != null){
				if(Input.GetKey(KeyCode.A)){
					harpoon.Pull();
				}
				else if(Input.GetKey(KeyCode.Z)){
					harpoon.Release();
				}
			}
		}
	}

	public void LaunchHarpoon(Vector3 direction){

		if(harpoon == null){
            
			if(direction.sqrMagnitude > joystixError)
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

	public void Cut(){
		if(harpoon != null){
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

        float speedBonus = physicMove.velocity.sqrMagnitude;

        // THe distance depends from the duration of cast
        //float distanceToReach = Mathf.Lerp(0f, harpoonModule.fireDistance, power);

        // OSEF
        float distanceToReach = harpoonModule.fireDistance;

        harpoon.Launch(this, selfTransform.position, direction, distanceToReach, harpoonModule.fireSpeed + speedBonus, harpoonModule.returnSpeed);
	}
}
