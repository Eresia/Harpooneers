﻿// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using Rewired;

[RequireComponent(typeof(MovementBehaviour))]
public class PlayerInput : MonoBehaviour
{
    public int playerId = 0; // The Rewired player id of this character

    public MovementBehaviour movement;
    public HarpoonLauncher harpoon;

    private Player player; // The Rewired Player
    
    private Vector2 inputDir;

    // Bomb components.
    public ExplosiveBarrel playerBomb;
    
    private Rigidbody _myRigidbody;
    private PlayerDeath _deathScript;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        _myRigidbody = GetComponent<Rigidbody>();
        _deathScript = GetComponent<PlayerDeath>();

        // Register delegates for specific actions.
        player.AddInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
        player.AddInputEventDelegate(ResurrectAlly, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Resurrect");
        player.AddInputEventDelegate(ReleaseRope, UpdateLoopType.Update, "Release Rope");
        player.AddInputEventDelegate(PullingOnRope, UpdateLoopType.Update, "Pull On Rope");
    }

    private void Reset()
    {
        movement = GetComponent<MovementBehaviour>();
        harpoon = GetComponent<HarpoonLauncher>();
    }

    private void Update()
    {
        // Handle movement.
        {
            float moveX = player.GetAxis("Move Horizontal");
            float moveZ = player.GetAxis("Move Vertical");

            Vector3 moveDir = new Vector3(moveX, 0f, moveZ);

            movement.Move(moveDir.normalized);
        }

        // Handle harpoon rotation.
        {
            float rotateX = player.GetAxis("Rotate Horizontal");
            float rotateZ = player.GetAxis("Rotate Vertical");

            Vector3 harpoonDir = new Vector3(rotateX, 0f, rotateZ);

            //Debug.DrawRay(transform.position, harpoonDir.normalized * 2.5f, Color.red, 1f);

            harpoon.LaunchHarpoon(harpoonDir.normalized);
        }

        HandleCutRope();
    }

    private void DropBomb(InputActionEventData data)
    {
        if(data.GetButtonDown() && !playerBomb.gameObject.activeSelf && !_deathScript.isDead)
        {
            // Spawn the bomb behind the boat
            playerBomb.gameObject.SetActive(true);
            playerBomb.SpawnTheBomb(transform.position - 1.25f * transform.forward, _myRigidbody.velocity);
        }
    }

    private void ResurrectAlly(InputActionEventData data)
    {
        if (data.GetButtonDown() && !_deathScript.isDead)
        {
            _deathScript.ResurrectFriend();
        }
    }

    private void ReleaseRope(InputActionEventData data)
    {
        if (data.GetButton() && !_deathScript.isDead)
        {
            harpoon.Release();
        }
    }

    private void PullingOnRope(InputActionEventData data)
    {
        if (data.GetButton() && !_deathScript.isDead)
        {
            harpoon.Pull();
        }
    }

    private void HandleCutRope()
    {
        if (player.GetButtonDown("Pull On Rope") && player.GetButton("Release Rope")
            || player.GetButton("Pull On Rope") && player.GetButtonDown("Release Rope")
            || player.GetButtonDown("Pull On Rope") && player.GetButtonDown("Release Rope")
            )
        {
            Debug.Log("Cut the rope");

            harpoon.Cut();
        }
    }

    private void OnDestroy()
    {
        /*
        if(player != null)
        {
            // Free delegates.
            player.RemoveInputEventDelegate(DropBomb);
            player.RemoveInputEventDelegate(ResurrectAlly);
            player.RemoveInputEventDelegate(ReleaseRope);
            player.RemoveInputEventDelegate(PullingOnRope);
        }
        */
    }
}