using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AspirationTentaclesPattern : BossPattern {

    private AspirationTentacleState state;
    private Phase2AI phase2;

	private AspiTentacleBehaviour aspiTentacle;
    private TentacleBehaviour[] swipperTentacles;

	private int swipperCount;

	private Coroutine[] tentacleCoroutines;

	private float baseAmplitude;

	private int waveId;

	private WaveOptions wave;

    public AspirationTentaclesPattern(AspirationTentacleState state)
    {
        this.state = state;
		swipperCount = 2;
		tentacleCoroutines = new Coroutine[2];
		waveId = GameManager.instance.ground.ZoneWaveId;
		wave = GameManager.instance.ground.waveManager.Waves[waveId];
		baseAmplitude = wave.amplitude;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
		boss.StartCoroutine(BeginPatternCoroutine());
    }

	private IEnumerator BeginPatternCoroutine(){
		float time = 0;

		do{
			wave.amplitude = baseAmplitude + ((state.waveAmplitude - baseAmplitude) / state.waveDuration) * time;
			GameManager.instance.ground.waveManager.ChangeWave(waveId, wave);
			yield return null;
			time += Time.deltaTime;
		}while(time < state.waveDuration);

		SpawnTentacle();
		tentacleCoroutines[0] = boss.StartCoroutine(ActivateSwipper());
        tentacleCoroutines[1] =  boss.StartCoroutine(ActivateAspi());

		time = 0;

		do{
			time += Time.deltaTime;
			yield return null;
		}
		while(time < state.maxTime);

		OnStopPattern();
	}

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacle()
    {
        swipperTentacles = new TentacleBehaviour[swipperCount];
		Vector3 position = GameManager.instance.bossMgr.east.position;
		position.x -= state.offset;
		aspiTentacle = phase2.TentaclesAspi[0] as AspiTentacleBehaviour;
		aspiTentacle.transform.position = position;
		aspiTentacle.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
		aspiTentacle.aspiBomb.OnExplodeAction = Hit;

		position.z -= state.distanceBeetweenTentacles;
		swipperTentacles[0] = phase2.TentaclesSwipper[0];
		swipperTentacles[0].transform.position = position;
		swipperTentacles[0].transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

		position.z += 2*state.distanceBeetweenTentacles;
		swipperTentacles[1] = phase2.TentaclesSwipper[1];
		swipperTentacles[1].transform.position = position;
		swipperTentacles[1].transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
	}

	private IEnumerator ActivateSwipper(){

		for (int i = 0; i < swipperCount; i++)
        {
            swipperTentacles[i].Spawning(state.bubblingDuration);
        }

        yield return new WaitForSeconds(state.bubblingDuration);

        for (int i = 0; i < swipperCount; i++)
        {
            swipperTentacles[i].Emerge(state.startPos, state.attackPos, state.emergingDuration);
        }

        yield return new WaitForSeconds(state.emergingDuration);

		while(true){
			for (int i = 0; i < swipperCount; i++)
			{
				swipperTentacles[i].FeedbackAttackArea();
			}

			yield return new WaitForSeconds(state.waitBeforeAttack);

			for (int i = 0; i < swipperCount; i++)
			{
				swipperTentacles[i].TriggerAttackAnim();
			}

			yield return new WaitWhile(() => (swipperTentacles[0].animator.GetBool("IsAttacking")));

		}
	}

    private IEnumerator ActivateAspi()
    {
        aspiTentacle.Spawning(state.bubblingDuration);

        yield return new WaitForSeconds(state.bubblingDuration);

        aspiTentacle.Emerge(state.startPos, state.attackPos, state.emergingDuration);

        yield return new WaitForSeconds(state.emergingDuration);

		aspiTentacle.BeginAttack();
    }

	public IEnumerator EndPatternCoroutine(){
		aspiTentacle.animator.SetTrigger("Hit");
		yield return new WaitWhile(() => aspiTentacle.animator.GetBool("End"));
		yield return new WaitUntil(() => aspiTentacle.animator.GetBool("End"));

		for (int i = 0; i < swipperCount; i++)
        {
            swipperTentacles[i].animator.SetBool("End", false);
            swipperTentacles[i].attackCollider.enabled = false;
            swipperTentacles[i].Dive(state.startPos, state.divingDuration);
        }

		aspiTentacle.animator.SetBool("End", false);
		aspiTentacle.attackCollider.enabled = false;
		aspiTentacle.Dive(state.startPos, state.divingDuration);

        yield return new WaitForSeconds(state.divingDuration);

        // Reset pos.
        swipperTentacles[0].ResetTentacle();
		swipperTentacles[1].ResetTentacle();
		aspiTentacle.ResetTentacle();

		float time = 0;

		do{
			wave.amplitude = state.waveAmplitude + ((baseAmplitude - state.waveAmplitude) / state.waveDuration) * time;
			GameManager.instance.ground.waveManager.ChangeWave(waveId, wave);
			yield return null;
			time += Time.deltaTime;
		}while(time < state.waveDuration);

		OnPatternFinished();
	}

	public void Hit(){
		boss.HitBoss(aspiTentacle.bombDamages);
		OnStopPattern();
	}

    protected override void OnStopPattern()
    {
        boss.StopCoroutine(tentacleCoroutines[0]);
		boss.StopCoroutine(tentacleCoroutines[1]);
		boss.StartCoroutine(EndPatternCoroutine());
    }
}
