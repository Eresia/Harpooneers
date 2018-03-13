using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DashPattern : BossPattern {

    private DashState dashState;
    private WhalePhaseAI whaleAI;

    public DashPattern(DashState dashState)
    {
        this.dashState = dashState;
    }

    public override void SetBoss(BossAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;
    }

    protected override void ExecutePattern() {
		boss.StartCoroutine(Move());
	}

    private IEnumerator Move()
    {
        Transform target = GameManager.instance.shipMgr.ChoosePlayerToAttack();
		whaleAI.WhaleAnimator.SetBool("Swim", true);
		float time = 0;

		while(true){
			time += Time.deltaTime;
			// if((time >= dashState.targetChangeTime) || ()){
			// 	target = GameManager.instance.shipMgr.ChoosePlayerToAttack();
			// 	time = 0;
			// }
			Vector3 whalePosition = whaleAI.WhaleTransform.position;
			Vector3 targetDirection = target.position - whalePosition;
			Quaternion angleDirection = Quaternion.LookRotation(targetDirection);
			whaleAI.WhaleTransform.rotation = Quaternion.Slerp(whaleAI.WhaleTransform.rotation, angleDirection, Time.deltaTime * dashState.turnSpeed);
			whalePosition += dashState.translateSpeed * whaleAI.WhaleTransform.forward * Time.deltaTime;
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
        // Dash isn't stoppable.
    }
}
