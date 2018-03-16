using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicMove))]
public class MovementBehaviour : MonoBehaviour {

    public CoqueModule coqueModule;

    // Lerp progressif sur la direction du bateau. (Direction desiree (INPUT) et direction actuelle.

    private Quaternion initialDir;
    private Quaternion targetDir;

	public PhysicMove physicMove;

	public AudioClip move_player_sound;

    private float move;

    public bool IsOutOfScreen
    {
        get { return isOutOfScreen; }
    }
    private bool isBumpInScreen;
    private bool isOutOfScreen;

    private PlayerManager playerMgr;

    private void Reset()
    {
        physicMove = GetComponent<PhysicMove>();
    }

    private void Awake()
    {
        initialDir = targetDir = Quaternion.identity;
        playerMgr = GetComponent<PlayerManager>();
    }

    public void Move(Vector3 inputDir) {

        if(inputDir == Vector3.zero)
        {
            move = 0f;

			//GameManager.instance.audioManager.StopPersistantSound (AudioManager.PossibleSound.MOVE,0.02f);

            return;
        }

        move = 1f;

		//GameManager.instance.audioManager.PlayPersistantSound (AudioManager.PossibleSound.MOVE,0.02f);

        initialDir = transform.rotation;
        targetDir = Quaternion.LookRotation(inputDir);
    }

    private void FixedUpdate()
    {
        if(playerMgr.IsDead)
        {
            transform.position = GameManager.instance.boundaryMgr.LimitPosition(transform.position);
            return;
        }

        if(initialDir != targetDir)
        {
            // Turn boat.
            transform.rotation = Quaternion.Lerp(initialDir, targetDir, Time.deltaTime * coqueModule.turnSpeed);
        }

        float acc = (coqueModule.drag * coqueModule.moveSpeedMax) / Time.deltaTime + coqueModule.moveSpeedMax;

        //Debug.Log(acc);

        // Move boat toward.
        physicMove.AddForce(transform.forward * acc * move);

        // Limit position in the boundaries of the screen.
        Vector3 pos = transform.position;

        //transform.position = GameManager.instance.boundaryMgr.LimitPosition(pos);

        // Check if pos is out of screen.
        // Feedback out of screen

        int outOfScreen = GameManager.instance.boundaryMgr.CheckPos(pos);
        
        // FEEDBACK leave screen.
        if (outOfScreen == 1)
        {
            isOutOfScreen = true;
        }

        // DEATH if too far.
        else if (outOfScreen == 2 && !isBumpInScreen)
        {
            isBumpInScreen = true;
            isOutOfScreen = false;

            playerMgr.Death();

            Vector3 dir = (GameManager.instance.boundaryMgr.screenCenterInWorldSpace - transform.position).normalized;

            physicMove.AddForce(-physicMove.Velocity);
            physicMove.AddForce(dir * GameManager.instance.boundaryMgr.forceWhenKillByBOundaries);
        }
    }

    public void IsRez()
    {
        isBumpInScreen = false;
    }

    // Freeze player at his position.
    public void FreezePlayer()
    {
        move = 0f;
    }
}
