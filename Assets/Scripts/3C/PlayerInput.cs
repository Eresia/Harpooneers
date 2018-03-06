// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(MovementBehaviour))]
public class PlayerInput : MonoBehaviour
{
    public int playerId = 0; // The Rewired player id of this character
    
    private Player player; // The Rewired Player

    private MovementBehaviour movement;
    private Vector2 inputDir;
    private Rigidbody _myRigidbody;

    public ExplosiveBarrel playerBomb;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
        _myRigidbody = GetComponent<Rigidbody>();

        movement = GetComponent<MovementBehaviour>();

        // Register delegates for specific actions.
        player.AddInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
        player.AddInputEventDelegate(ResurrectAlly, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Resurrect");
        player.AddInputEventDelegate(ReleaseRope, UpdateLoopType.Update, "Release Rope");
        player.AddInputEventDelegate(PullingOnRope, UpdateLoopType.Update, "Pull On Rope");
    }

    private void Update()
    {
        // Handle movement.
        {
            float moveX = player.GetAxis("Move Horizontal");
            float moveZ = player.GetAxis("Move Vertical");

            Vector3 moveDir = new Vector3(moveX, 0f, moveZ);

            //Debug.DrawRay(transform.position, realDir);

            movement.Move(moveDir.normalized);
        }

        // Handle harpoon rotation.
        {
            float rotateX = player.GetAxis("Rotate Horizontal");
            float rotateZ = player.GetAxis("Rotate Vertical");

            Vector3 harpoonDir = new Vector3(rotateX, 0f, rotateZ);

            Debug.DrawRay(transform.position, harpoonDir * 2.5f, Color.red, 1f);

            // Pass harpoonDir.normalized
        }

        HandleCutRope();
    }

    private void DropBomb(InputActionEventData data)
    {
        if(data.GetButtonDown() && !playerBomb.gameObject.activeSelf)
        {
            // Spawn the bomb behind the boat
            playerBomb.gameObject.SetActive(true);
            playerBomb.SpawnTheBomb(transform.position - 1.25f * transform.forward, _myRigidbody.velocity);
        }
    }

    private void ResurrectAlly(InputActionEventData data)
    {
        if (data.GetButtonDown())
        {
            Debug.Log("Resurrect Ally");
        }
    }

    private void ReleaseRope(InputActionEventData data)
    {
        if (data.GetButtonDown())
        {
            Debug.Log("Start Release rope");
        }

        else if (data.GetButtonUp())
        {
            Debug.Log("Stop Release rope");
        }
    }

    private void PullingOnRope(InputActionEventData data)
    {
        if (data.GetButtonDown())
        {
            Debug.Log("Start pulling on");
        }

        else if (data.GetButtonUp())
        {
            Debug.Log("Stop pulling on");
        }
    }

    private void HandleCutRope()
    {
        if (player.GetButtonDown("Pull On Rope") && player.GetButton("Release Rope")
            || player.GetButtonDown("Release Rope") && player.GetButton("Release Rope")
            || player.GetButtonDown("Release Rope") && player.GetButtonDown("Release Rope")
            )
        {
            Debug.Log("Cut the rope");
        }
    }

    private void OnDestroy()
    {
        // Free delegates.
        player.RemoveInputEventDelegate(DropBomb);
        player.RemoveInputEventDelegate(ResurrectAlly);
        player.RemoveInputEventDelegate(ReleaseRope);
        player.RemoveInputEventDelegate(PullingOnRope);
    }
}