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
		boss.StartCoroutine(Move());
	}

    private IEnumerator Move()
    {
        PlayerManager target = GameManager.instance.shipMgr.ChoosePlayerManagerToAttack();
		whaleAI.WhaleAnimator.SetBool("Swim", true);
		float time = 0;

		while(true){
			time += Time.deltaTime;
			if((time >= state.targetChangeTime) || (target.isDead)){
				target = GameManager.instance.shipMgr.ChoosePlayerManagerToAttack();
				time = 0;
			}
			Vector3 whalePosition = whaleAI.WhaleTransform.position;
			Vector3 targetDirection = target.transform.position - whalePosition;
			Quaternion angleDirection = Quaternion.LookRotation(targetDirection);
			whaleAI.WhaleTransform.rotation = Quaternion.Slerp(whaleAI.WhaleTransform.rotation, angleDirection, Time.deltaTime * state.turnSpeed);
			whalePosition += state.translateSpeed * whaleAI.WhaleTransform.forward * Time.deltaTime;
			whaleAI.WhaleTransform.position = GameManager.instance.ground.GetTransformInfo(whalePosition).position;
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

		// whaleAI.WhaleAnimator.SetBool("Swim", false);
    }

    protected override void OnStopPattern()
    {
        // Can't stop pattern
    }
}
