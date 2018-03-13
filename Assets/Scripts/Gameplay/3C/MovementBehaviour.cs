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

    private void Reset()
    {
        physicMove = GetComponent<PhysicMove>();
    }

    private void Awake()
    {
        initialDir = targetDir = Quaternion.identity;
		GameManager.instance.audioManager.CreatePersistantSound (AudioManager.PossibleSound.MOVE, move_player_sound,1f);
    }

    public void Move(Vector3 inputDir) {

        if(inputDir == Vector3.zero)
        {
            move = 0f;

			GameManager.instance.audioManager.StopPersistantSound (AudioManager.PossibleSound.MOVE,0.1f);

            return;
        }

        move = 1f;

		GameManager.instance.audioManager.PlayPersistantSound (AudioManager.PossibleSound.MOVE,0.1f);

        initialDir = transform.rotation;
        targetDir = Quaternion.LookRotation(inputDir);
    }

    private void FixedUpdate()
    {
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

        transform.position = GameManager.instance.boundaryMgr.InScreenPosition(pos);
    }

    // Freeze player at his position.
    public void FreezePlayer()
    {
        move = 0f;
    }
}
