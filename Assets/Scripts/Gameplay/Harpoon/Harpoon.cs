﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Harpoon : MonoBehaviour {

	public enum State{
		LAUNCHING,
		GRIPPED,
		RETURN
	}

	[SerializeField]
	private string hookLayer;

	[SerializeField]
	private float minDistance;

	[SerializeField]
	private float tractionSpeed;

    public float TractionSpeed {
        get { return tractionSpeed; }
        set { tractionSpeed = value; }
    }

    [SerializeField]
	private float forceBlock;

	[SerializeField]
	private float forceBreak;

	public State state {get; private set;}

	private HarpoonLauncher launcher;

	private LineRenderer lineRenderer;

	private Transform selfTransform;

	private Transform parentTransform;

	private Vector3 direction;

	private float maxDistance;
	private float actualDistance;

	private float launchSpeed;
	
	private float returnSpeed;

    public float slingForce = 100f;
    private bool doSling;

	private void Awake()
    {
		selfTransform = GetComponent<Transform>();
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
    {
		Vector3 selfPos = selfTransform.position;
		Vector3 launcherPos = launcher.selfTransform.position;
		float color = 0;
		float distance = Vector3.Distance(selfPos, launcherPos);
        
		switch(state){
			case State.LAUNCHING:
				selfPos += direction * Time.deltaTime;
				selfTransform.position = selfPos;

				if(distance > maxDistance)
                {
					Cut();
				}
				break;

			case State.GRIPPED:
				if(distance > actualDistance)
                {
					Vector3 normal = selfPos - launcherPos;

					// Vector3 force = (distance - actualDistance) * normal.normalized * forceBlock;
					// if(forceBreak < force.sqrMagnitude){
					// 	Cut();
					// }
					// else{
					// 	launcher.physicMove.AddForce(force);
					// }

					launcher.selfTransform.position = launcherPos + (distance - actualDistance) * normal.normalized;
                }

				else
                {
					color = (actualDistance - distance) / actualDistance;
				}

                break;

			case State.RETURN:
				float movement = returnSpeed * Time.deltaTime;

				if(distance < movement)
                {
					launcher.EndReturn();
					Destroy(gameObject);
					return ;
				}

				else
                {
					Vector3 newDirection = launcherPos - selfPos;
					selfPos += newDirection.normalized * movement;
					selfTransform.position = selfPos;
				}
				
				break;
		}

        lineRenderer.SetPosition(0, selfPos);
        lineRenderer.SetPosition(1, launcherPos);
		lineRenderer.materials[0].color = new Color(color, color, color, 1f);
	}

	public void Launch(HarpoonLauncher launcher, Vector3 from, Vector3 direction, float maxDistance, float returnSpeed){
		this.launcher = launcher;
		this.direction = direction;
		this.maxDistance = maxDistance;
		this.returnSpeed = returnSpeed;
		this.state = State.LAUNCHING;
        
        selfTransform.position = from;

        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, from);
    }

	public void Cut(){

		if(state < State.RETURN)
        {
			selfTransform.parent = launcher.boatFollower;

            if(doSling)
            {
                launcher.physicMove.AddForce(launcher.physicMove.Velocity.normalized * slingForce);
            }

			state = State.RETURN;
		}
	}

	public void Release() {

		if(state == State.GRIPPED)
        {
			actualDistance += tractionSpeed * Time.deltaTime;

			if(actualDistance > maxDistance)
            {
				actualDistance = maxDistance;
                doSling = true;
            }

            else
            {
                doSling = false;
            }
		}
	}

	public void Pull(){
		if(state == State.GRIPPED){
			actualDistance -= tractionSpeed * Time.deltaTime;
			if(actualDistance < minDistance){
				Cut();
			}
		}
	}

    // Try to attach to a collider.
    private void OnTriggerEnter(Collider other)
    {
        if (state == State.LAUNCHING)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(hookLayer))
            {
                HarpoonLauncher otherLauncher = other.GetComponentInParent<HarpoonLauncher>();
                if (launcher.Equals(otherLauncher))
                {
                    return;
                }
            }

            parentTransform = other.GetComponent<Transform>();
            selfTransform.parent = parentTransform;

            Vector3 targetPos = selfTransform.position;
            targetPos.y = 0f;

            selfTransform.position = targetPos;

            maxDistance = Vector3.Distance(selfTransform.position, launcher.selfTransform.position);
            actualDistance = maxDistance;
            state = State.GRIPPED;

            // Notify that the gameobject has been harpooned.
            IHarpoonable harpoonable = other.GetComponent<IHarpoonable>();
            if (harpoonable != null)
            {
                harpoonable.OnHarpoonCollide(this);
            }
        }
    }
}
