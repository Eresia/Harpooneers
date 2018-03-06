using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour {

	public enum State{
		LAUNCHING,
		GRIPPED,
		RETURN
	}

	public State state {get; private set;}

	private HarpoonLauncher parent;

	private Transform selfTransform;

	private Vector3 direction;

	private float maxDistance;

	private float launchSpeed;
	
	private float returnSpeed;

	private void Awake() {
		selfTransform = GetComponent<Transform>();
	}

	private void Update() {
		float distance = Vector3.Distance(selfTransform.position, parent.selfTransform.position);

		switch(state){
			case State.LAUNCHING:
				selfTransform.position += direction * launchSpeed * Time.deltaTime;
				if(distance > maxDistance){
					Cut();
				}
				break;

			case State.GRIPPED:

				break;

			case State.RETURN:
				float movement = returnSpeed * Time.deltaTime;

				if(distance < movement){
					parent.EndReturn();
					Destroy(gameObject);
					return ;
				}
				else{
					selfTransform.position -= direction * movement;
				}
				
				break;
		}
	}

	public void Launch(HarpoonLauncher parent, Vector3 from, Vector3 direction, float maxDistance, float launchSpeed, float returnSpeed){
		this.parent = parent;
		this.direction = direction;
		this.maxDistance = maxDistance;
		this.launchSpeed = launchSpeed;
		this.returnSpeed = returnSpeed;
		this.state = State.LAUNCHING;

		selfTransform.position = from;
	}

	public void Cut(){
		if(state < State.RETURN){
			state = State.RETURN;
		}
	}
}
