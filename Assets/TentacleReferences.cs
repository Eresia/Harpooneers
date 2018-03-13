using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleReferences : MonoBehaviour {

    [Header("Main components")]
    public Transform childTransform;

    public ParticleSystem spawningFX;

    public Collider tentacleCollider;
    
    public Animator animAttack;

    public Animator animGA;
}
