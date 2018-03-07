// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using Rewired;

[RequireComponent(typeof(MovementBehaviourNewPhysic))]
public class PlayerInputNewPhysic : MonoBehaviour
{
    public int playerId = 0; // The Rewired player id of this character

    public MovementBehaviourNewPhysic movement;
    public HarpoonLauncher harpoon;

    private Player player; // The Rewired Player
    
    private Vector2 inputDir;

    // Bomb components.
    public ExplosiveBarrel playerBomb;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        // Register delegates for specific actions.
        player.AddInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
        player.AddInputEventDelegate(ResurrectAlly, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Resurrect");
        player.AddInputEventDelegate(ReleaseRope, UpdateLoopType.Update, "Release Rope");
        player.AddInputEventDelegate(PullingOnRope, UpdateLoopType.Update, "Pull On Rope");
    }

    private void Reset()
    {
        movement = GetComponent<MovementBehaviourNewPhysic>();
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
        if(data.GetButtonDown() && !playerBomb.gameObject.activeSelf)
        {
            // Spawn the bomb behind the boat
            playerBomb.gameObject.SetActive(true);
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
        if (data.GetButton())
        {
            harpoon.Release();
        }
    }

    private void PullingOnRope(InputActionEventData data)
    {
        if (data.GetButton())
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