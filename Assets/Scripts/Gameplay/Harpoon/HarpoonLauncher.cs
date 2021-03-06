﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("GD Features")]

    [Tooltip("Try to autocorrect the target when launch the harpoon. It Needs bigger collider.")]
    public bool autoCorrectDestination = false;
    public LayerMask targetableCollider;

    [Tooltip("Distance depends of the cast time")]
    public bool lockNLoad = true;

    public Transform selfTransform {get; private set;}

	public PhysicMove physicMove {get; private set;}

	private Harpoon harpoon;

	private bool isLaunching;

	private float power;

	private Vector3 lastDirection;
 
    public Transform harpoonPivot;
    public Transform harpoonMuzzle;
    public ParticleSystem harpoonLaunchFX;

    public Image directionImage;
    private int playerID;

   

    private void Awake()
	{
		selfTransform = GetComponent<Transform>();
		physicMove = GetComponent<PhysicMove>();

        playerID = GetComponent<PlayerInput>().playerId;
        switch (playerID)
        {
            case 0:
                directionImage.color = Color.yellow;
                break;

            case 1:
                directionImage.color = Color.red;
                break;

            case 2:
                directionImage.color = Color.magenta;
                break;

            case 3:
                directionImage.color = Color.green;
                break;
        }
    }

	public void LaunchHarpoon(Vector3 direction){

		if(harpoon == null)
        {
			if(direction.sqrMagnitude > joystickError)
            {
				if(!isLaunching)
                {
					BeginLaunching();
				}

                // Cut behaviour.
                {
                    /*
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
                    }*/
                }

				DisplayLaunching(direction, 1f);
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

        harpoonPivot.localRotation = Quaternion.identity;
    }

	private void BeginLaunching(){
		isLaunching = true;
		power = 0f;
		directionObject.gameObject.SetActive(true);
	}

	private void DisplayLaunching(Vector3 direction, float power){
		directionObject.localPosition = direction * power * castDistance;
        directionObject.rotation = Quaternion.LookRotation(direction);
        harpoonPivot.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
    }

	private void EndLaunching(Vector3 direction, float power){

		isLaunching = false;
		directionObject.gameObject.SetActive(false);
		harpoon = Instantiate<Harpoon>(harpoonPrefab, selfTransform.position, Quaternion.identity);
        harpoon.TractionSpeed = harpoonModule.tractionSpeed;
        
        float distanceToReach = 0f;

        // Here The distance depends from the duration of cast
        if (lockNLoad)
        {
            distanceToReach = Mathf.Lerp(0f, harpoonModule.fireDistance, power);
        }

        // Here any duration give max distance.
        else
        {
            distanceToReach = harpoonModule.fireDistance;
        }

        // Auto correct the destination of the harpoon.
        if(autoCorrectDestination)
        {
            direction = TryToAutoAim(direction);
        }

        Debug.DrawRay(harpoonPivot.position, direction * 20f, Color.red, 1f);

        //   harpoon.Launch(this, harpoonPivot.position, direction * harpoonModule.fireSpeed + physicMove.Velocity, distanceToReach, harpoonModule.returnSpeed);
        harpoon.Launch(this, harpoonMuzzle.transform.position, direction * harpoonModule.fireSpeed + new Vector3(physicMove.Velocity.x, 0f, physicMove.Velocity.z), distanceToReach, harpoonModule.returnSpeed);

        harpoonLaunchFX.Play();
    }

    private Vector3 TryToAutoAim(Vector3 currentDir)
    {
        Ray r = new Ray(selfTransform.position, currentDir);
        Vector3 finalDir = currentDir;

        Debug.DrawRay(r.origin, r.direction * 100f, Color.green, 2f);

        RaycastHit hit;
        if(Physics.Raycast(r, out hit, 1000f, targetableCollider)) {

            finalDir = (hit.transform.position - selfTransform.position).normalized;
        }

        return finalDir;
    }
}
