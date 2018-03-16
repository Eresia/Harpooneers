using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TentacleBehaviour : MonoBehaviour {

    [Header("Main components")]
    public Transform childTransform;

    public ParticleSystem spawningFX;

    public ParticleSystem attackFx;
    
    public Animator animator;

    public AudioClip bubbleSound;

    public AudioClip attackSound;

    public Collider attackCollider;
    public Collider bodyCollider;
    public HandleHarpoonWithEnnemy harpoonScript;

    protected void Awake()
    {
        childTransform.gameObject.SetActive(false);

        if(attackCollider)
        {
            attackCollider.enabled = false;
        }
    }

    public void Spawning(float spawningDuration)
    {
        gameObject.SetActive(true);
		if(spawningFX != null){
			spawningFX.Play();
		}

        GameManager.instance.audioManager.PlaySoundOneTime(bubbleSound, 0.5f);
    }
    
    public void Emerge(Vector3 startPos, Vector3 endPos, float emergingDuration)
    {
		if(spawningFX != null)
        {
        	spawningFX.Stop();
		}

        childTransform.gameObject.SetActive(true);

		if(bodyCollider){
			bodyCollider.enabled = true;
		}

        animator.SetTrigger("Spawn");

        childTransform.localPosition = startPos;
        childTransform.DOLocalMove(endPos, emergingDuration).SetEase(Ease.InCubic);
    }

    public void FeedbackAttackArea()
    {
		if(attackFx != null){
			attackFx.Play();
		}
    }

    public void Dive(Vector3 endPos, float divingDuration)
    {
        if(attackCollider)
        {
            attackCollider.enabled = false;
        }

        animator.SetTrigger("Despawn");
        childTransform.DOLocalMove(endPos, divingDuration);
    }

    public void TriggerAnim(string anim)
    {
        animator.SetTrigger(anim);
    }

    public void TriggerAttackAnim(bool doWave = false)
    {
        animator.SetTrigger("Attack");
        GameManager.instance.audioManager.PlaySoundOneTime(attackSound, 0.2f);

        StartCoroutine(Attack(doWave));
    }

    private IEnumerator Attack(bool doWave = false)
    {
        yield return new WaitUntil(() => (animator.GetBool("IsAttacking")));
        
        if(attackFx)
        {
            attackFx.Stop();
        }

        if(attackCollider)
        {
            attackCollider.enabled = true;

            if(doWave)
            {
                Vector2 pos = GameManager.instance.ground.GetSeaPosition(attackCollider.transform.position + attackCollider.transform.forward * 5f);
                GameManager.instance.ground.waveManager.CreateImpact(pos, 5f, 0f, 0.04f, 2f, 1f, 10f);
            }
           
        }

        yield return new WaitUntil(() => (!animator.GetBool("IsAttacking")));

        attackCollider.enabled = false;
    }

    public void FocusPlayer(float turnDuration)
    {
        Transform target = GameManager.instance.shipMgr.ChoosePlayerToAttack();

        Vector3 dir = (target.position - childTransform.position);
        dir.y = 0f;

        childTransform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), turnDuration);
    }

    public void ResetTentacle()
    {
        gameObject.SetActive(false);

		if(bodyCollider){
			bodyCollider.enabled = false;
		}

        //transform.rotation = Quaternion.identity;
        //transform.position = Vector3.zero;

        childTransform.gameObject.SetActive(false);
        childTransform.localPosition = Vector3.zero;
        childTransform.localRotation = Quaternion.identity;
    }
}
