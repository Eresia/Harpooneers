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
			state.test = isImmersed;
			state.test2 = targetY;

			time += Time.deltaTime;

			if((time >= (timeTarget + state.targetChangeTime)) || (target.isDead)){
				target = GameManager.instance.shipMgr.ChoosePlayerManagerToAttack();
				timeTarget = time;
			}

			if(time > (timeBeforeImmerse + state.timeinSea)){
				isImmersed = !isImmersed;
				timeBeforeImmerse = time;
			}
			

			Vector3 whalePosition = whaleAI.WhaleTransform.position;
			Vector3 targetDirection = target.transform.position - whalePosition;
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
