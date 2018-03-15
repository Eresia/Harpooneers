using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Vector3 launcherPos;

    private float maxDistance;
	private float actualDistance;

	private float launchSpeed;
	
	private float returnSpeed;
    private int playerID;

    public float slingForce = 100f;
    private bool doSling;

    private Vector3 harpoonPivotDir;

    public Transform ropeAttach;

	public AudioClip pull_sound;

    public ParticleSystem bloodFx;
    public ParticleSystem impactFx;

    // Current gameObject where the harpoon is attached.
    private IHarpoonable iHarpoonable;

    private void Awake()
    {
		selfTransform = GetComponent<Transform>();
		lineRenderer = GetComponent<LineRenderer>();
    }

	private void Update()
    {
		Vector3 selfPos = selfTransform.position;
        launcherPos = launcher.transform.position;
		float color = 0;
		float distance = Vector3.Distance(selfPos, launcherPos);
        
		switch(state)
        {
			case State.LAUNCHING:
				selfPos += direction * Time.deltaTime;
				selfTransform.position = selfPos;
                selfTransform.rotation = Quaternion.LookRotation(transform.position - launcherPos);

                if (distance > maxDistance)
                {
					Cut();
				}
				break;

			case State.GRIPPED:
				if(distance > actualDistance)
                {
					Vector3 normal = selfPos - launcherPos;

					launcher.selfTransform.position = launcherPos + (distance - actualDistance) * normal.normalized;
                }

				else
                {
					color = (actualDistance - distance) / actualDistance;
				}

                break;

			case State.RETURN:
				float movement = returnSpeed * Time.deltaTime;
                selfTransform.rotation = Quaternion.LookRotation(transform.position - launcher.harpoonMuzzle.position);

                if (distance < movement)
                {
					launcher.EndReturn();

                    Destroy(gameObject); // TODO Optimize that !
					return;
				}

				else
                {
					Vector3 newDirection = launcherPos - selfPos;
					selfPos += newDirection.normalized * movement;
					selfTransform.position = selfPos;
				}
				
				break;
		}

        // Move the line renderer depending the harpoon and the harpoon muzzle pos.
        lineRenderer.SetPosition(0, ropeAttach.position);
        lineRenderer.SetPosition(1, launcher.harpoonMuzzle.position);

        // TODO remove when material will be set.
		lineRenderer.materials[0].color = new Color(color, color, color, 1f);

        // Update harpoon pivot depending the pos of the harpoon.
        harpoonPivotDir = (selfPos - launcher.harpoonMuzzle.position).normalized;
        harpoonPivotDir.y = 0f;

        launcher.harpoonPivot.rotation = Quaternion.LookRotation(harpoonPivotDir);
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

     //   harpoonMeshGo.transform.LookAt()
    }

	public void Cut(){

        if(iHarpoonable != null)
        {
            iHarpoonable.OnHarpoonDetach();

            iHarpoonable = null;
        }

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
				GameManager.instance.audioManager.PlaySoundOneTime (pull_sound, 0.1f);
            }

            else
            {
                doSling = false;
            }
		}
	}

	public void Pull()
    {
		if(state == State.GRIPPED)
        {
            Cut();
            launcher.physicMove.AddForce((harpoonPivotDir + launcher.physicMove.Velocity.normalized) * 25f);

            GameManager.instance.audioManager.StopPersistantSound(AudioManager.PossibleSound.PULL, 0.1f);

            // Old behaviour.
            /*
			actualDistance -= tractionSpeed * Time.deltaTime;
			if(actualDistance < minDistance){
				Cut();
            }
            */
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
            targetPos.y = launcher.harpoonMuzzle.position.y;

            selfTransform.position = targetPos;

            maxDistance = Vector3.Distance(selfTransform.position, launcher.selfTransform.position);
            actualDistance = maxDistance;
            state = State.GRIPPED;

            // Store and notify that the gameobject has been harpooned.
            iHarpoonable = other.GetComponent<IHarpoonable>();
            if (iHarpoonable != null)
            {
                iHarpoonable.OnHarpoonAttach(this);
            }

            // Feedback on hit
            impactFx.Play();

            // Feedback on whale hit
            if (other.tag == "Whale")
            {
                bloodFx.Play();
            }
        }
    }
}
