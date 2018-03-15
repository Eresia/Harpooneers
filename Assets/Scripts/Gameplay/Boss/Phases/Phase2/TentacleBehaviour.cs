using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TentacleBehaviour : MonoBehaviour {

    [Header("Main components")]
    public Transform childTransform;

    public ParticleSystem spawningFX;

    public ParticleSystem attackFx;

    public Collider tentacleCollider;
    
    public Animator animator;

    public AudioClip bubbleSound;

    public AudioClip attackSound;

    private void Awake()
    {
        childTransform.gameObject.SetActive(false);
        //tentacleCollider.enabled = false;
    }

    public void Spawning(float spawningDuration)
    {
        gameObject.SetActive(true);
        spawningFX.Play();

        GameManager.instance.audioManager.PlaySoundOneTime(bubbleSound, 0.5f);
    }
    
    public void Emerge(Vector3 startPos, Vector3 endPos, float emergingDuration)
    {
        spawningFX.Stop();
        childTransform.gameObject.SetActive(true);

        animator.SetTrigger("Spawn");

        childTransform.localPosition = startPos;
        childTransform.DOLocalMove(endPos, emergingDuration).SetEase(Ease.InCubic);
    }

    public void FeedbackAttackArea()
    {
        attackFx.Play();
    }

    public void Dive(Vector3 endPos, float divingDuration)
    {
        // TODO Disable colliders when dive.

        animator.SetTrigger("Despawn");
        childTransform.DOLocalMove(endPos, divingDuration);
    }

    public void TriggerAttackAnim()
    {
        attackFx.Stop();
        animator.SetTrigger("Attack");

        //GameManager.instance.audioManager.PlaySoundOneTime(attackSound, 0.2f);
    }

    public void FocusPlayer(float turnDuration)
    {
        Transform target = GameManager.instance.shipMgr.ChoosePlayerToAttack();

        Vector3 dir = (target.position - childTransform.position);
        dir.y = 0f;

        //Debug.DrawRay(phase2.Tentacles[i].transform.position, dir * 5f, Color.white, 1f);

        childTransform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), turnDuration);
    }

    public void ResetTentacle()
    {
        gameObject.SetActive(false);
        childTransform.gameObject.SetActive(false);

        childTransform.localPosition = Vector3.zero;
    }
}
