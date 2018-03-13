using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SplashTentaclesPattern : BossPattern {

    private SplashTentacleState state;
    private Phase2AI phase2;

    private Vector3[] spawns;

    public SplashTentaclesPattern(SplashTentacleState state)
    {
        this.state = state;

        spawns = new Vector3[state.tentacleCount];
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
        // Spawn 2 tentacles random around center.
        
        // woob wooob woooob
        boss.StartCoroutine(ActivateTentacles());
    }

    private void SpawnTentacles()
    {
        Vector3 center = phase2.bossMgr.center.position;

        for (int i = 0; i < state.tentacleCount; i++)
        {
            // Random a position on a circle (in X and Z).
            Vector3 randPos = Vector3.zero;
            Vector2 randCircle = Random.insideUnitCircle.normalized * state.spawnRadius;
            randPos.x = randCircle.x;
            randPos.z = randCircle.y;

            spawns[i] = center + randPos;
            phase2.Tentacles[i].childTransform.position = spawns[i] + state.startPos;
        }
    }

    IEnumerator ActivateTentacles()
    {
        for (int attack = 0; attack < state.attackCount; attack++)
        {
            // Play FX !
            for (int i = 0; i < 2; i++)
            {
                phase2.Tentacles[i].spawningFX.Play();
            }

            SpawnTentacles();

            yield return new WaitForSeconds(state.bubblingDuration);
            
            for (int i = 0; i < state.tentacleCount; i++)
            {
                phase2.Tentacles[i].spawningFX.Stop();
                phase2.Tentacles[i].gameObject.SetActive(true);

                // TODO Play animation tournoiement...

                phase2.Tentacles[i].childTransform.DOLocalMove(spawns[i] + state.attackPos, state.emergingDuration);
            }

            yield return new WaitForSeconds(state.emergingDuration);

            // SPLASH
            
            // Focus a player.
            for (int i = 0; i < state.tentacleCount; i++)
            {
                Transform target = GameManager.instance.shipMgr.ChoosePlayerToAttack();

                Vector3 dir = (phase2.Tentacles[i].childTransform.position - target.position).normalized;
                dir.y = 0f;

                //Debug.DrawRay(phase2.Tentacles[i].transform.position, dir * 5f, Color.white, 1f);

                phase2.Tentacles[i].childTransform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), state.turnDuration);
            }

            yield return new WaitForSeconds(state.turnDuration);

            for (int i = 0; i < state.tentacleCount; i++)
            {
                // Play attack animation.
                phase2.Tentacles[i].childTransform.DOLocalMove(spawns[i] + state.startPos, state.divingDuration);
                phase2.Tentacles[i].tentacleCollider.enabled = true;

                //phase2.Tentacles[i].animAttack.Play("");
            }

            // TODO WAIT ANIMATION -> OnStateExit -> 
            //yield return new WaitWhile(() => (phase2.Tentacles[0].animGA.GetBool("End")));
            yield return new WaitForSeconds(2f);

            for (int i = 0; i < state.tentacleCount; i++)
            {
                phase2.Tentacles[i].tentacleCollider.enabled = false;
            }

            // woob wooob woooob

            // Reset pos.
            for (int i = 0; i < state.tentacleCount; i++)
            {
                //phase2.Tentacles[i].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(30f);
        }
    }

    protected override void OnStopPattern()
    {
        throw new System.NotImplementedException();
    }
}
