using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SharkTentaclesPattern : BossPattern {

    private SharkTentacleState state;
    private Phase2AI phase2;

	private SharkTentacleBehaviour sharkTentacle;

	private Transform target;

    public SharkTentaclesPattern(SharkTentacleState state)
    {
        this.state = state;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
		SpawnTentacle();
		boss.StartCoroutine(ActivateTentacles());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacle()
    {
		target = GameManager.instance.shipMgr.ChoosePlayerToAttack();
		sharkTentacle = phase2.TentacleShark as SharkTentacleBehaviour;
		sharkTentacle.transform.position = target.position;
		sharkTentacle.aspiBomb.OnExplodeAction = Hit;
	}

    private IEnumerator ActivateTentacles()
    {
        sharkTentacle.Spawning(state.bubblingDuration);

        yield return new WaitForSeconds(state.bubblingDuration);

        sharkTentacle.Emerge(state.startPos, state.attackPos, state.emergingDuration);

		yield return new WaitForSeconds(state.emergingDuration);

        float time = 0;

		do{
			sharkTentacle.transform.Rotate(0f, state.rotationSpeed * Time.deltaTime, 0f);
			if(state.follow){
				float y = sharkTentacle.transform.position.y;
				sharkTentacle.transform.position = new Vector3(target.position.x, y, target.position.z);
			}
			yield return null;
			time += Time.deltaTime;
		}while(time < state.waitBeforeAttack);

		yield return new WaitForSeconds(state.waitBeforeUp);

		sharkTentacle.transform.DOMove(sharkTentacle.transform.position + new Vector3(0, state.attackHeight, 0), state.attackUpTime);

		foreach(Collider c in sharkTentacle.aspiBomb.Colliders){
			c.enabled = true;
		}

		sharkTentacle.animator.SetTrigger("Eat");

		yield return new WaitForSeconds(state.waitAfterEat);

		sharkTentacle.animator.SetBool("End", false);
		sharkTentacle.Dive(state.startPos, state.divingDuration);

        yield return new WaitForSeconds(state.divingDuration);

        // Reset pos.
		sharkTentacle.ResetTentacle();

		OnPatternFinished();
    }

	public void Hit(){
		boss.HitBoss(sharkTentacle.bombDamages);
		sharkTentacle.animator.SetTrigger("Hit");
	}

    protected override void OnStopPattern()
    {
        
    }
}
