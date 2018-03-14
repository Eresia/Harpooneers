using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TentacleBehaviour : MonoBehaviour {

    [Header("Main components")]
    public Transform childTransform;

    public ParticleSystem spawningFX;

    public Collider tentacleCollider;
    
    public Animator animAttack;

    public Animator animGA;
    
    public IEnumerator Spawning(float spawningDuration)
    {
        gameObject.SetActive(true);
        spawningFX.Play();

        yield return new WaitForSeconds(spawningDuration);
    }
    
    public IEnumerator Emerge(Vector3 startPos, Vector3 endPos, float emergingDuration)
    {
        spawningFX.Stop();
        childTransform.gameObject.SetActive(true);

        // TODO Play animation tournoiement...

        childTransform.localPosition = startPos;
        childTransform.DOLocalMove(endPos, emergingDuration);

        yield return new WaitForSeconds(emergingDuration);
    }

    public IEnumerator Dive(Vector3 endPos, float divingDuration)
    {
        childTransform.DOLocalMove(endPos, divingDuration);

        yield return new WaitForSeconds(divingDuration);
    }

    public IEnumerator Attack()
    {
        tentacleCollider.enabled = true;

        // TODO Play attack animation.
        // phase2.Tentacles[i].animAttack.Play("");

        // TODO WAIT ANIMATION -> OnStateExit -> 
        //yield return new WaitWhile(() => (phase2.Tentacles[0].animGA.GetBool("End")));
        yield return new WaitForSeconds(2f);

        tentacleCollider.enabled = false;
    }

    public IEnumerator FocusPlayer(float turnDuration)
    {
        Transform target = GameManager.instance.shipMgr.ChoosePlayerToAttack();

        Vector3 dir = (childTransform.position - target.position).normalized;
        dir.y = 0f;

        //Debug.DrawRay(phase2.Tentacles[i].transform.position, dir * 5f, Color.white, 1f);

        childTransform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), turnDuration);

        yield return new WaitForSeconds(turnDuration);
    }

    public void ResetTentacle()
    {
        gameObject.SetActive(false);

        childTransform.localPosition = Vector3.zero;
    }
}
