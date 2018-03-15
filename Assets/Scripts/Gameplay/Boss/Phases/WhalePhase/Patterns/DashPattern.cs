using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DashPattern : BossPattern {

    private DashState state;
    private WhalePhaseAI whaleAI;

    public DashPattern(DashState state)
    {
        this.state = state;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;

        // Callback when a bomb explodes.
        
    }

    protected override void ExecutePattern() {
		whaleAI.Whale.SetActive(true);
		boss.StartCoroutine(Move());
	}

    private IEnumerator Move()
    {
        PlayerManager target = GameManager.instance.shipMgr.ChoosePlayerManagerToAttack();
		whaleAI.WhaleAnimator.SetBool("Swim", true);
		float time = 0;
		float timeTarget = 0;
		float timeBeforeImmerse = 0;
		bool isImmersed = true;
		float targetY = -state.depth;

		

		while(true){
			time += Time.deltaTime;

			if((time >= (timeTarget + state.targetChangeTime)) || (target.IsDead)){
				target = GameManager.instance.shipMgr.ChoosePlayerManagerToAttack();
				timeTarget = time;
			}

			if(time > (timeBeforeImmerse + state.timeinSea)){
				isImmersed = !isImmersed;
				timeBeforeImmerse = time;
			}
			

			Vector3 whalePosition = whaleAI.WhaleTransform.position;
			Vector3 targetDirection = target.transform.position - whalePosition;

			RaycastHit hit;

        	if (Physics.Raycast(whalePosition, targetDirection.normalized, out hit, targetDirection.sqrMagnitude, whaleAI.whaleReferences.toAvoidLayers)){
				Vector3 hitPosition = hit.transform.position;
				float radius = Vector3.Distance(hitPosition, hit.point);
				Vector3 obstacle = whalePosition - hitPosition;
				Quaternion angleToTarget = Quaternion.LookRotation(obstacle, targetDirection - hit.transform.position);
				if(angleToTarget.y > 180){
					angleToTarget.y -= 360;
				}
				float sign = Mathf.Sign(angleToTarget.y);
				float x = obstacle.x * Mathf.Cos(sign*Mathf.PI/2) - obstacle.z * Mathf.Sin(sign*Mathf.PI/2);
				float z = obstacle.z * Mathf.Cos(sign*Mathf.PI/2) + obstacle.x * Mathf.Sin(sign*Mathf.PI/2);
				Vector3 lastDirection = (new Vector3(x, 0f, z)).normalized;
				targetDirection = new Vector3(hitPosition.x + lastDirection.x * radius, 0f, hitPosition.z + lastDirection.z * radius) - whalePosition;
			}

			Debug.DrawRay(whalePosition + targetDirection, Vector3.up * 10f);

			Quaternion angleDirection = Quaternion.LookRotation(targetDirection);
			angleDirection.x = 0f;
			angleDirection.z = 0f;
			whaleAI.WhaleTransform.rotation = Quaternion.Slerp(whaleAI.WhaleTransform.rotation, angleDirection, Time.deltaTime * state.turnSpeed);
			whalePosition += state.translateSpeed * whaleAI.WhaleTransform.forward * Time.deltaTime;

			if(time < state.maxTime){
				if(!isImmersed){
					targetY =  GameManager.instance.ground.GetTransformInfo(whalePosition).position.y;
				}
				else{
					targetY =- state.depth;
				}
			}
			else{
				isImmersed = true;
				targetY = -whaleAI.resetDepth;
				if(time > (state.maxTime + state.endOfPaternTime)){
					break;
				}
			}

			float diff = targetY - whalePosition.y;

			if(Mathf.Abs(diff) > Time.deltaTime * state.ySpeed){
				whalePosition.y += Time.deltaTime * Mathf.Sign(diff) * state.ySpeed;
				// whalePosition.y = targetY;
			}
			
			whaleAI.WhaleTransform.position = whalePosition;
			float y = angleDirection.eulerAngles.y;
			int driftDirection;
			if((y == 0) || (y == 180)){
				driftDirection = 0;
			}
			else if(y < 180){
				driftDirection = -1;
			}
			else{
				driftDirection = 1;
			}
			whaleAI.WhaleAnimator.SetInteger("Drift",  driftDirection);
			yield return null;
		}

		whaleAI.WhaleAnimator.SetBool("Swim", false);

		whaleAI.ResetWhaleTransform(0f);

		OnPatternFinished();

		// whaleAI.WhaleAnimator.SetBool("Swim", false);
    }

    protected override void OnStopPattern()
    {
        // Can't stop pattern
    }
}
