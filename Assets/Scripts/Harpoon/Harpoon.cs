using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour {

	public enum State{
		LAUNCHING,
		GRIPPED,
		RETURN
	}

	[SerializeField]
	private string hookLayer;

	public State state {get; private set;}

	private HarpoonLauncher launcher;

	private Transform selfTransform;

	private Transform parentTransform;

	private Vector3 direction;

	private float maxDistance;

	private float launchSpeed;
	
	private float returnSpeed;

	private void Awake() {
		selfTransform = GetComponent<Transform>();
	}

	private void Update() {
		Vector3 selfPos = selfTransform.position;
		Vector3 launcherPos = launcher.selfTransform.position;
		float distance = Vector3.Distance(selfPos, launcherPos);

		switch(state){
			case State.LAUNCHING:
				selfTransform.position += direction * launchSpeed * Time.deltaTime;
				if(distance > maxDistance){
					Cut();
				}
				break;

			case State.GRIPPED:
				if(distance > maxDistance){
					Debug.Log("test");
					Vector3 normal = selfPos - launcherPos;
					launcher.selfRigidbody.AddForce((distance - maxDistance) * normal, ForceMode.Acceleration);
				}
				break;

			case State.RETURN:
				float movement = returnSpeed * Time.deltaTime;

				if(distance < movement){
					launcher.EndReturn();
					Destroy(gameObject);
					return ;
				}
				else{
					Vector3 newDirection = launcherPos - selfPos;
					selfTransform.position += newDirection.normalized * movement;
				}
				
				break;
		}
	}

	public void Launch(HarpoonLauncher launcher, Vector3 from, Vector3 direction, float maxDistance, float launchSpeed, float returnSpeed){
		this.launcher = launcher;
		this.direction = direction;
		this.maxDistance = maxDistance;
		this.launchSpeed = launchSpeed;
		this.returnSpeed = returnSpeed;
		this.state = State.LAUNCHING;

		selfTransform.position = from;
	}

	public void Cut(){
		if(state < State.RETURN){
			selfTransform.parent = null;
			state = State.RETURN;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(state == State.LAUNCHING){
			if(other.gameObject.layer == LayerMask.NameToLayer(hookLayer)){
				HarpoonLauncher otherLauncher = other.GetComponentInParent<HarpoonLauncher>();
				if(launcher.Equals(otherLauncher)){
					return ;
				}
			}
			parentTransform = other.GetComponent<Transform>();
			selfTransform.parent = parentTransform;
			maxDistance = Vector3.Distance(selfTransform.position, launcher.selfTransform.position);
			state = State.GRIPPED;
		}
		
	}
}
