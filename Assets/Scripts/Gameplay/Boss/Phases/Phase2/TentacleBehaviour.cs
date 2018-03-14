﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TentacleBehaviour : MonoBehaviour {

    [Header("Main components")]
    public Transform childTransform;

    public ParticleSystem spawningFX;

    public Collider tentacleCollider;
    
    public Animator animAttack;

    private void Awake()
    {
        childTransform.gameObject.SetActive(false);
        //tentacleCollider.enabled = false;
    }

    public void Spawning(float spawningDuration)
    {
        gameObject.SetActive(true);
        spawningFX.Play();
    }
    
    public void Emerge(Vector3 startPos, Vector3 endPos, float emergingDuration)
    {
        spawningFX.Stop();
        childTransform.gameObject.SetActive(true);

        animAttack.SetTrigger("Spawn");

        childTransform.localPosition = startPos;
        childTransform.DOLocalMove(endPos, emergingDuration).SetEase(Ease.InCubic);
    }

    public void Dive(Vector3 endPos, float divingDuration)
    {
        childTransform.DOLocalMove(endPos, divingDuration);
    }

    public void TriggerAttackAnim(string attack)
    {
        animAttack.SetTrigger(attack);

        // TODO Play attack animation.
        // phase2.Tentacles[i].animAttack.Play("");

        // TODO WAIT ANIMATION -> OnStateExit ->
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